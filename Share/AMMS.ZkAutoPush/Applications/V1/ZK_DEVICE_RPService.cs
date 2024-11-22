using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Data;
using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using EventBus.Messages;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications.V1;
public class ZK_DEVICE_RPService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;

    private readonly DeviceAutoPushDbContext _deviceAutoPushDbContext;
    private readonly DeviceCacheService _deviceCacheService;
    private readonly DeviceCommandCacheService _deviceCommandCacheService;

    public ZK_DEVICE_RPService(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter, IConfiguration configuration
        , DeviceCacheService deviceCacheService, DeviceCommandCacheService deviceCommandCacheService
        )
    {
        _deviceAutoPushDbContext = deviceAutoPushDbContext;
        _eventBusAdapter = eventBusAdapter;
        _configuration = configuration;
        _deviceCacheService = deviceCacheService;
        _deviceCommandCacheService = deviceCommandCacheService;
    }
    static int countData = 0;
    public async Task ProcessData(ZK_DEVICE_RP data)
    {
        try
        {
            Logger.Information(data.SN + " : " + data.Content);
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

                            //Xoá lệnh khỏi danh sách
                            var x = await _deviceCommandCacheService.GetByCode(data.SN, ID);

                            if (x == null)
                                return;

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
                                string ip = GetValueFromEqual("IPAddress", _ListParams);
                                string mac = GetValueFromEqual("MAC", _ListParams);
                                string userQty = GetValueFromEqual("UserCount", _ListParams);
                                string fpQty = GetValueFromEqual("FPCount", _ListParams);
                                string faceQty = GetValueFromEqual("FaceCount", _ListParams);
                                string palmQty = GetValueFromEqual("PvCount", _ListParams);
                                string transactionQty = GetValueFromEqual("TransactionCount", _ListParams);
                                //Cập nhật vào CSDL
                                try
                                {
                                    var thietBiUpdate = ZK_SV_PUSHService.ListTerminal.FirstOrDefault(o => o.sn == data.SN);
                                    //var thietBiUpdate = await _deviceCacheService.Get( data.SN);
                                    if (thietBiUpdate != null)
                                    {
                                        thietBiUpdate.face_count = int.Parse(faceQty);
                                        thietBiUpdate.user_count = int.Parse(userQty);
                                        thietBiUpdate.fv_count = int.Parse(fpQty);
                                        thietBiUpdate.last_activity = DateTime.Now;
                                        thietBiUpdate.push_time = DateTime.Now;
                                        thietBiUpdate.online_status = true;
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
                                    Logger.Error(e);
                                }

                                //Gửi thông tin lại máy chủ
                                TA_DeviceStatus tA_DeviceStatus = new TA_DeviceStatus();
                                tA_DeviceStatus.ConnectStatus = true;
                                tA_DeviceStatus.SerialNumber = data.SN;
                                tA_DeviceStatus.UserCount = int.Parse(userQty);
                                tA_DeviceStatus.FaceCount = int.Parse(faceQty);
                                tA_DeviceStatus.FingerCount = int.Parse(fpQty);

                                string strContent = JsonConvert.SerializeObject(tA_DeviceStatus);

                                RB_ServerResponse response = new RB_ServerResponse()
                                {
                                    ReponseType = RB_ServerResponseType.DeviceInfo,
                                    Action = ServerRequestAction.ActionGetDeviceInfo,
                                    Content = strContent,
                                    Id = x.DataId,
                                    RequestId = x.DataId,
                                    IsSuccessed = true,
                                    DateTimeResponse = DateTime.Now,
                                    Message = true ? RB_ServerResponseMessage.Complete : RB_ServerResponseMessage.InComplete,
                                };

                                var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Device_Auto_Push_D2S}");
                                await aa.Send(response);

                                await _deviceCommandCacheService.Remove(data.SN, ID);

                            }
                            else
                            {
                                countData++;
                                Console.WriteLine("Số bản ghi: " + countData.ToString());
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
                                }

                                await UpdateCommand(x, elm);


                                //Đẩy dữ liệu lại rabbitmq cho sv
                                if (x.DataId == null || x.ParentId != null)
                                {
                                    RB_ServerResponse responseFace = new RB_ServerResponse()
                                    {
                                        ReponseType = RB_ServerResponseType.UserInfo,
                                        Action = x.Action,
                                        Content = elm,
                                        Id = x.ParentId,
                                        RequestId = x.ParentId,
                                        IsSuccessed = x.IsSuccessed,
                                        DateTimeResponse = x.RevicedTime,
                                        Message = x.IsSuccessed ? RB_ServerResponseMessage.Complete : RB_ServerResponseMessage.InComplete,
                                    };

                                    var aaFace = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Device_Auto_Push_D2S}");
                                    await aaFace.Send(responseFace);

                                    await _deviceCommandCacheService.Remove(data.SN, ID);

                                    return;
                                }
                                RB_ServerResponse response = new RB_ServerResponse()
                                {
                                    ReponseType = RB_ServerResponseType.UserInfo,
                                    Action = x.Action,
                                    Content = elm,
                                    Id = x.DataId,
                                    RequestId = x.DataId,
                                    IsSuccessed = x.IsSuccessed,
                                    DateTimeResponse = x.RevicedTime,
                                    Message = x.IsSuccessed ? RB_ServerResponseMessage.Complete : RB_ServerResponseMessage.InComplete,
                                };

                                var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Device_Auto_Push_D2S}");
                                await aa.Send(response);
                                Console.WriteLine("Gửi rabitmq: " + countData.ToString());

                                await _deviceCommandCacheService.Remove(data.SN, ID);

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
            data.online_status = terminal.online_status;
            data.ip_address = terminal.ip_address;
            data.fv_count = terminal.fv_count;
            data.user_count = terminal.user_count;
            data.face_count = terminal.face_count;
            data.push_time = terminal.push_time;
            data.last_activity = terminal.last_activity;
            data.name = terminal.name;
            data.port = terminal.port;
            data.time_online = terminal.time_online;
            data.time_offline = terminal.time_offline;
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
    public string GetValueFromEqual(string key, List<string> sources)
    {
        try
        {
            foreach (var x in sources)
            {
                if (!string.IsNullOrEmpty(x))
                {
                    var arr = x.Split('=');
                    if (arr != null && arr.Length > 1)
                    {
                        if (key == arr[0])
                        {
                            return x.Replace(key + "=", "");
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return "";
    }

}
