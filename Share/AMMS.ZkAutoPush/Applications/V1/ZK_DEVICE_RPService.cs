using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Data;
using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using EventBus.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications.V1;
public class ZK_DEVICE_RPService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;

    private readonly DeviceAutoPushDbContext _deviceAutoPushDbContext;
    public ZK_DEVICE_RPService(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter, IConfiguration configuration)
    {
        _deviceAutoPushDbContext = deviceAutoPushDbContext;
        _eventBusAdapter = eventBusAdapter;
        _configuration = configuration;
    }
    public async Task ProcessData(ZK_DEVICE_RP data)
    {
        try
        {
            var content = data.Content;
            var contentArr1 = content.Split('\n');
            if (contentArr1.Length > 0)
            {
                foreach (var elm in contentArr1)
                {
                    if (!string.IsNullOrEmpty(elm))
                    {
                        var contentArr = elm.Split('&');
                        if (contentArr.Length == 3)
                        {
                            var ID = contentArr[0].Split('=')[1];
                            var Return = contentArr[1].Split('=')[1];
                            var CMD = contentArr[2].Split('=')[1];
                            //Là thông tin thiết bị
                            if (CMD == "INFO")
                            {
                                List<string> _ListParams = new List<string>();
                                StringReader strReader = new StringReader(content);
                                string line = "";
                                do
                                {
                                    line = strReader.ReadLine();
                                    _ListParams.Add(line);
                                } while (!string.IsNullOrEmpty(line));
                                string ip = IclockOperarion.GetValueFromEqual("IPAddress", _ListParams);
                                string mac = IclockOperarion.GetValueFromEqual("MAC", _ListParams);
                                string userQty = IclockOperarion.GetValueFromEqual("UserCount", _ListParams);
                                string fpQty = IclockOperarion.GetValueFromEqual("FPCount", _ListParams);
                                string faceQty = IclockOperarion.GetValueFromEqual("FaceCount", _ListParams);
                                string palmQty = IclockOperarion.GetValueFromEqual("PvCount", _ListParams);
                                string transactionQty = IclockOperarion.GetValueFromEqual("TransactionCount", _ListParams);

                                //Cập nhật vào CSDL
                                try
                                {
                                    var thietBiUpdate = ZK_SV_PUSHService.ListTerminal.FirstOrDefault(o => o.sn == data.SN);
                                    if (thietBiUpdate != null)
                                    {
                                        thietBiUpdate.face_count = int.Parse(faceQty);
                                        thietBiUpdate.user_count = int.Parse(userQty);
                                        thietBiUpdate.fv_count = int.Parse(fpQty);
                                        thietBiUpdate.last_activity = DateTime.Now;
                                        thietBiUpdate.push_time = DateTime.Now;
                                        thietBiUpdate.isconnect = true;
                                        //Cập nhật vào csdl
                                        try
                                        {
                                            await SaveDevice(thietBiUpdate);
                                        }
                                        catch (Exception e)
                                        {

                                        }
                                    }


                                }
                                catch (Exception e)
                                {

                                }

                            }
                            //Xoá lệnh khỏi danh sách
                            var x = ZK_SV_PUSHService.ListIclockCommand.FirstOrDefault(o => o.SerialNumber == data.SN && o.Id.ToString() == ID);
                            if (x != null)
                            {

                                if (Return == "0")//Thiet bi thuc hien thanh cong lenh
                                {
                                    x.returnCode = 0;
                                    x.IsSuccessed = true;
                                }
                                else// Co loi khi thuc hienh lenh
                                {
                                    int returnCode = 0;
                                    int.TryParse(Return, out returnCode);
                                    x.returnCode = returnCode;
                                    x.IsSuccessed = false;
                                    //Logger.ShowLog("Co loi khi thuc hienh lenh");
                                    //Logger.ShowLog(sn);
                                    //Logger.ShowLog(content);
                                }
                                await UpdateCommand(x, content);
                                ZK_SV_PUSHService.ListIclockCommand.Remove(x);


                                //Đẩy dữ liệu lại rabbitmq cho sv
                                if (x.DataId == null || x.ParentId != null)
                                    return;
                                RB_ServerResponse response = new RB_ServerResponse()
                                {
                                    Action = x.Action,
                                    Content = content,
                                    Id = x.DataId,
                                    RequestId = x.DataId,
                                    IsSuccessed = x.IsSuccessed,
                                    DateTimeResponse = x.RevicedTime,
                                    Message = x.IsSuccessed ? RB_ServerResponseMessage.Complete : RB_ServerResponseMessage.InComplete,
                                };

                                var aa = await _eventBusAdapter.GetSendEndpointAsync(EventBusConstants.DataArea + EventBusConstants.Device_Auto_Push_D2S);
                                await aa.Send(response);

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            throw;
        }
    }

    public async Task SaveDevice(zk_terminal terminal)
    {
        try
        {
            bool add = false;
            var data = _deviceAutoPushDbContext.zk_terminal.FirstOrDefault(x => x.sn == terminal.sn);
            if (data == null)
            {
                add = true;
                data = new zk_terminal();
                data.Id = terminal.Id;
                data.create_time = DateTime.Now;
                data.sn = terminal.sn;
            }
            data.isconnect = terminal.isconnect;
            data.ip_address = terminal.ip_address;
            data.fv_count = terminal.fv_count;
            data.user_count = terminal.user_count;
            data.face_count = terminal.face_count;
            data.push_time = terminal.push_time;
            data.last_activity = terminal.last_activity;
            data.name = terminal.name;
            data.port = terminal.port;
            if (add)
            {
                await _deviceAutoPushDbContext.zk_terminal.AddAsync(data);
            }
            await _deviceAutoPushDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
            return;
        }
    }
    public async Task UpdateCommand(IclockCommand command, string content)

    {
        try
        {
            var data = _deviceAutoPushDbContext.zk_terminalcommandlog.OrderByDescending(m => m.transfer_time).FirstOrDefault(x => x.request_id == command.DataId);
            if (data == null)
            {
                return;
            }
            data.return_time = command.RevicedTime;
            data.successed = command.IsSuccessed;
            data.return_content = content;
            data.commit_time = command.CommitTime;
            data.return_value = command.returnCode;
            await _deviceAutoPushDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
            return;
        }
    }

}
