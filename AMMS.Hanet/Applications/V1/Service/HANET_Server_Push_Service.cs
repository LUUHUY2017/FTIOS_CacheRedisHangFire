using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Data;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using EventBus.Messages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using System.Drawing.Imaging;

namespace AMMS.Hanet.Applications.V1.Service
{
    public class HANET_Server_Push_Service
    {
        private readonly IEventBusAdapter _eventBusAdapter;
        private readonly DeviceCacheService _deviceCacheService;
        private readonly IConfiguration _configuration;
        private readonly HANET_API_Service _hanetAPIService;
        HANET_Device_Reponse_Service _HANET_Device_Reponse_Service;

        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public HANET_Server_Push_Service(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter, DeviceCacheService deviceCacheService, IConfiguration configuration1, HANET_API_Service hanetAPIService, HANET_Device_Reponse_Service hANET_Device_Reponse_Service)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
            _eventBusAdapter = eventBusAdapter;
            _deviceCacheService = deviceCacheService;
            _configuration = configuration1;
            _hanetAPIService = hanetAPIService;
            _HANET_Device_Reponse_Service = hANET_Device_Reponse_Service;
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
                    commit_time = DateTime.Now,
                    successed = false,
                };
                if (!string.IsNullOrEmpty(rB_ServerRequest.RequestParam))
                {
                    //Lưu thông tin người dùng
                    if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                    {
                        conmandlog = await UpdatePerson(conmandlog);
                    }
                    //Xoá người dùng
                    else if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                    {
                        conmandlog = await RemovePerson(conmandlog);
                    }
                    //Thêm thiết bị
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
                    //Thêm người dùng
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
                    //Lấy lại thông tin chấm công
                    else if (rB_ServerRequest.Action == ServerRequestAction.ActionGetData && rB_ServerRequest.RequestType == ServerRequestType.TAData)
                    {
                        conmandlog = await GetDataByTime(conmandlog);
                    }
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
                Logger.Error(e);
            }
        }

        #endregion

        /// <summary>
        /// cập nhật thông tin người dùng
        /// </summary>
        /// <param name="conmandlog"></param>
        /// <returns></returns>
        public async Task<hanet_commandlog> UpdatePerson(hanet_commandlog conmandlog)
        {
            try
            {
                TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(conmandlog.content);

                if (data == null)
                {
                    conmandlog.return_time = DateTime.Now;
                    conmandlog.return_content = "Thông tin người dùng không hợp lệ";
                    conmandlog.return_value = 0;
                    conmandlog.successed = false;
                }
                else if (string.IsNullOrEmpty(data.UserFace))
                {
                    conmandlog.return_time = DateTime.Now;
                    conmandlog.return_content = "Không có thông tin khuôn mặt";
                    conmandlog.return_value = 0;
                    conmandlog.successed = false;
                }
                else
                {
                    Hanet_User user = new Hanet_User()
                    {
                        base64file = data.UserFace,
                        name = data.FullName,
                        aliasID = data.PersonCode,
                        placeID = HanetParam.PlaceId,
                        departmentID = data.SerialNumber,
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
                }
                await AddCommand(conmandlog);
                return conmandlog;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return conmandlog;
            }
        }
        /// <summary>
        /// Xoá người dùng
        /// </summary>
        /// <param name="conmandlog"></param>
        /// <returns></returns>
        public async Task<hanet_commandlog> RemovePerson(hanet_commandlog conmandlog)
        {
            try
            {
                TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(conmandlog.content);

                if (data == null)
                    return conmandlog;
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
                return conmandlog;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return conmandlog;
            }
        }

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
        /// <summary>
        /// Lấy lại thông tin chấm công 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<hanet_commandlog> GetDataByTime(hanet_commandlog conmandlog)
        {
            try
            {
                TA_AttendenceHistoryRequest? data = JsonConvert.DeserializeObject<TA_AttendenceHistoryRequest>(conmandlog.content);
                if (data == null || data.StartDate == null || data.EndDate == null)
                {
                    conmandlog.return_time = DateTime.Now;
                    conmandlog.return_content = "Thiếu thông tin dữ liệu truyền vào";
                    conmandlog.return_value = 0;
                    conmandlog.successed = false;
                }
                else
                {
                    var countData = 1;
                    var totalData = 0;
                    var totalValidData = 0;
                    int page = 0;
                    while (countData > 0)
                    {
                        page++;

                        var top100Data = await _hanetAPIService.GetCheckinByTime(data.StartDate.Value, data.EndDate.Value, page, 50);

                        foreach (var item in top100Data)
                        {
                            var dataLog = await _HANET_Device_Reponse_Service.AddTransactionHistoryLog(item);

                            if (dataLog != null && item != null && !string.IsNullOrEmpty(item.aliasID))
                            {
                                long milliSec = item.checkinTime.Value;
                                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(milliSec);
                                DateTime utc7DateTime = dateTimeOffset.ToOffset(TimeSpan.FromHours(7)).DateTime;
                                //Tìm học sinh
                                var user = await _HANET_Device_Reponse_Service.FindUserFromHanet(item.aliasID);

                                if (user != null)
                                {
                                    TA_AttendenceHistory tA_AttendenceHistory = new TA_AttendenceHistory()
                                    {
                                        Id = dataLog.id,
                                        DeviceId = dataLog.deviceID,
                                        PersonCode = user.StudentCode,
                                        SerialNumber = item.deviceName,
                                        TimeEvent = utc7DateTime,
                                        Type = (int)TA_AttendenceType.Face,
                                    };

                                    RB_DataResponse rB_Response = new RB_DataResponse()
                                    {
                                        Id = tA_AttendenceHistory.Id,
                                        Content = JsonConvert.SerializeObject(tA_AttendenceHistory),
                                        ReponseType = RB_DataResponseType.AttendenceHistory,
                                    };

                                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Data_Auto_Push_D2S}");

                                    await aa.Send(rB_Response);

                                    //Đẩy lại ảnh
                                    var image = _HANET_Device_Reponse_Service.ConvertImage(item.avatar, dataLog.id + ".jpg", ImageFormat.Jpeg);


                                    TA_AttendenceImage tA_AttendenceImage = new TA_AttendenceImage()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ImageBase64 = image,
                                        PersonCode = user.StudentCode,
                                        SerialNumber = dataLog.deviceID,
                                        TimeEvent = utc7DateTime,
                                        AttendenceHistoryId = dataLog.id,
                                    };

                                    RB_DataResponse rB_DataResponse = new RB_DataResponse()
                                    {
                                        Id = tA_AttendenceImage.Id,
                                        Content = JsonConvert.SerializeObject(tA_AttendenceImage),
                                        ReponseType = RB_DataResponseType.AttendenceImage,
                                    };

                                    var aa2 = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Data_Auto_Push_D2S}");
                                    await aa2.Send(rB_DataResponse);
                                    totalValidData++;
                                }
                            }
                        }
                        countData = top100Data.Count;
                        totalData += countData;
                    }
                    conmandlog.return_time = DateTime.Now;
                    conmandlog.return_content = "Thành công " + totalValidData + " bản ghi hợp lệ trong " + totalData + " bản ghi.";
                    conmandlog.return_value = 1;
                    conmandlog.successed = true;

                    await AddCommand(conmandlog);
                }

                return conmandlog;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                conmandlog.return_content = ex.Message;
                return conmandlog;
            }

        }
    }

}
