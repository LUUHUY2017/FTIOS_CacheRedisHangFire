using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Data;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using EventBus.Messages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using Shared.Core.Loggers;
using System.Text.Json.Serialization;

namespace AMMS.Hanet.Applications.V1.Service
{
    public class HANET_Server_Push_Service
    {
        private readonly IEventBusAdapter _eventBusAdapter;
        private readonly DeviceCacheService _deviceCacheService;
        private readonly IConfiguration _configuration;
        private readonly HANET_API_Service _hanetAPIService;
        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public HANET_Server_Push_Service(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter, DeviceCacheService deviceCacheService, IConfiguration configuration1, HANET_API_Service hanetAPIService)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
            _eventBusAdapter = eventBusAdapter;
            _deviceCacheService = deviceCacheService;
            _configuration = configuration1;
            _hanetAPIService = hanetAPIService;
        }

        #region Pust data user
        public async Task ProcessDataServerPush(RB_ServerRequest rB_ServerRequest)
        {
            try
            {
                Logger.Warning("Data :" + rB_ServerRequest.RequestType + " - " + rB_ServerRequest.Action);

                //Tạo lệnh
                hanet_commandlog conmandlog = new hanet_commandlog()
                {
                    create_time = DateTime.Now,
                    Id = rB_ServerRequest.Id,
                    terminal_sn = rB_ServerRequest.SerialNumber,
                    terminal_id = rB_ServerRequest.DeviceId,
                    content = rB_ServerRequest.RequestParam,
                    command_ation = rB_ServerRequest.Action,
                    command_type = rB_ServerRequest.RequestType,
                    successed = false,
                };

                if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                {
                    TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(rB_ServerRequest.RequestParam);

                    if (data == null)
                        return;
                    Hanet_User user = new Hanet_User()
                    {
                        base64file = data.UserFace,
                        name = data.FullName,
                        aliasID = data.PersonCode,
                        //placeID = rB_ServerRequest.SchoolId,
                        placeID = HanetParam.PlaceId,
                    };

                    if (await _hanetAPIService.CheckUser(user))
                    {
                        var result = await _hanetAPIService.UpdateFaceToHanet(user);
                        if (result != null)
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = result.returnMessage;
                            conmandlog.return_value = result.returnCode;
                            if (result.returnCode == Hanet_Response_Static.SUCCESSCode)
                                conmandlog.successed = true;
                            else
                                conmandlog.successed = false;


                        }
                        else
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = "Không kết nối được server hanet";
                            conmandlog.return_value = null;
                            conmandlog.successed = false;

                        }
                    }
                    else
                    {

                        var result = await _hanetAPIService.AddUserToHanet(user);
                        if (result != null)
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = result.returnMessage;
                            conmandlog.return_value = result.returnCode;
                            conmandlog.successed = true;

                        }
                        else
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = "Không kết nối được server hanet";
                            conmandlog.return_value = null;
                            conmandlog.successed = false;

                        }
                    }
                    await AddCommand(conmandlog);
                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                {
                    TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(rB_ServerRequest.RequestParam);

                    if (data == null)
                        return;
                    Hanet_User user = new Hanet_User()
                    {
                        base64file = data.UserFace,
                        name = data.FullName,
                        aliasID = data.PersonCode,
                        //placeID = rB_ServerRequest.SchoolId,
                        placeID = HanetParam.PlaceId,
                    };
                    if (await _hanetAPIService.RemoveUser(user))
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Thành công";
                        conmandlog.return_value = 1;
                        conmandlog.successed = true;
                    }
                    else
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Không thành công";
                        conmandlog.return_value = 0;
                        conmandlog.successed = false;
                    }


                    await AddCommand(conmandlog);

                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.Device)
                {
                    TA_Device? data = JsonConvert.DeserializeObject<TA_Device>(rB_ServerRequest.RequestParam);
                    if (data == null)
                        return;
                    if (await SaveDevice(data))
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Thành công";
                        conmandlog.return_value = 1;
                        conmandlog.successed = true;

                        //Lưu thông tin caches
                        var terminal = await _deviceAutoPushDbContext.hanet_terminal.FirstOrDefaultAsync(x => x.Id == data.Id);
                        if (terminal != null)
                            await _deviceCacheService.Save(terminal);
                    }
                    else
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Không thành công";
                        conmandlog.return_value = 0;
                        conmandlog.successed = false;
                    }
                    await AddCommand(conmandlog);

                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.Device)
                {
                    TA_Device? data = JsonConvert.DeserializeObject<TA_Device>(rB_ServerRequest.RequestParam);
                    if (data == null)
                        return;
                    if (await RemoveDevice(data))
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Thành công";
                        conmandlog.return_value = 1;
                        conmandlog.successed = true;

                        //Xoá thông tin caches
                        await _deviceCacheService.Remove(data.SerialNumber);

                    }
                    else
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Không thành công";
                        conmandlog.return_value = 0;
                        conmandlog.successed = false;
                    }
                    await AddCommand(conmandlog);

                }

                //Trả lại lệnh cho máy chủ
                RB_ServerResponse response = new RB_ServerResponse()
                {
                    Action = conmandlog.command_ation,
                    Content = conmandlog.return_content,
                    Id = conmandlog.Id,
                    RequestId = conmandlog.Id,
                    IsSuccessed = conmandlog.successed,
                    DateTimeResponse = conmandlog.return_time,
                    Message = conmandlog.successed == true ? RB_ServerResponseMessage.Complete : RB_ServerResponseMessage.InComplete,
                };

                var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Device_Auto_Push_D2S}");
                await aa.Send(response);

            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
       
        #endregion
        /// <summary>
        /// Lưu thông tin lệnh
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task AddCommand(hanet_commandlog command)
        {
            try
            {
                bool add = false;

                var data = _deviceAutoPushDbContext.hanet_commandlog.FirstOrDefault(x => x.Id == command.Id);
                if (data == null)
                {
                    add = true;
                    data = new hanet_commandlog();
                    data.Id = command.Id;
                    data.create_time = DateTime.Now;
                }
                data.transfer_time = DateTime.Now;
                data.terminal_id = command.terminal_id;
                data.terminal_sn = command.terminal_sn;
                data.content = command.content;
                data.command_type = command.command_type;
                data.command_ation = command.command_ation;
                data.successed = command.successed;
                data.commit_time = command.commit_time;
                data.return_content = command.return_content;
                data.return_value = command.return_value;
                data.return_time = command.return_time;
                data.successed = command.successed;

                if (add)
                {
                    _deviceAutoPushDbContext.hanet_commandlog.Add(data);
                }
                _deviceAutoPushDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return;
            }
        }
        /// <summary>
        /// Lưu thông tin thiết bị
        /// </summary>
        /// <param name="tA_Device"></param>
        /// <returns></returns>
        public async Task<bool> SaveDevice(TA_Device tA_Device)
        {
            try
            {
                bool add = false;
                var data = _deviceAutoPushDbContext.hanet_terminal.FirstOrDefault(x => x.Id == tA_Device.Id);
                if (data == null)
                {
                    add = true;
                    data = new hanet_terminal();
                    data.create_time = DateTime.Now;
                    data.Id = tA_Device.Id;
                }
                data.Id = tA_Device.Id;
                data.sn = tA_Device.SerialNumber;
                data.name = tA_Device.DeviceName;
                if (add)
                {
                    await _deviceAutoPushDbContext.hanet_terminal.AddAsync(data);
                }
                await _deviceAutoPushDbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Lưu thông tin thiết bị
        /// </summary>
        /// <param name="tA_Device"></param>
        /// <returns></returns>
        public async Task<bool> RemoveDevice(TA_Device tA_Device)
        {
            try
            {
                var data = _deviceAutoPushDbContext.hanet_terminal.FirstOrDefault(x => x.Id == tA_Device.Id);
                if (data == null)
                {
                    return true;
                }
                _deviceAutoPushDbContext.hanet_terminal.Remove(data);
                await _deviceAutoPushDbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return false;
            }
        }

    }

}
