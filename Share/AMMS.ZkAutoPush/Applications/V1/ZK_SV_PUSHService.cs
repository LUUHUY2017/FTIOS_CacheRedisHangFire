using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace AMMS.ZkAutoPush.Applications.V1
{
    public class ZK_SV_PUSHService
    {
        private readonly DeviceAutoPushDbContext _deviceAutoPushDbContext;
        private readonly DeviceCacheService _deviceCacheService;
        private readonly DeviceCommandCacheService _deviceCommandCacheService;

        /// <summary>
        /// Danh sách thiết bị (sẽ chuyển sang caches)
        /// </summary>
        public static List<zk_terminal> ListTerminal { get; set; } = new List<zk_terminal>();
        /// <summary>
        /// Danh sách lệnh gửi xuống thiết bị (sẽ chuyển sang caches)
        /// </summary>
        //public static List<IclockCommand> ListIclockCommand { get; set; } = new List<IclockCommand>();

        private readonly IConfiguration _configuration;


        public ZK_SV_PUSHService(DeviceAutoPushDbContext deviceAutoPushDbContext, IConfiguration configuration, DeviceCacheService deviceCacheService, DeviceCommandCacheService deviceCommandCacheService)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
            _configuration = configuration;
            _deviceCacheService = deviceCacheService;
            _deviceCommandCacheService = deviceCommandCacheService;
        }
        public async Task Process(RB_ServerRequest rB_ServerRequest)
        {
            try
            {
                IclockCommand command = null;

                //var thietbi = ListTerminal.FirstOrDefault(x => x.sn == rB_ServerRequest.SerialNumber);
                var sn = rB_ServerRequest.SerialNumber.Trim();
                if (sn == null)
                    return;
                Logger.Warning("Data :" + rB_ServerRequest.RequestParam);
                IclockCommand command2 = null;

                if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                {
                    TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(rB_ServerRequest.RequestParam);

                    if (data == null)
                        return;
                    await SaveUserInfo(data);

                    command = IclockOperarion.CommandUploadUser(sn, data.PersonCode, data.FullName, "", "0", data.UserCard, rB_ServerRequest.Id);
                    //Đẩy thêm ảnh
                    if (!string.IsNullOrEmpty(data.UserFace))
                    {
                        //if (rB_ServerRequest.SerialNumber == "PYA8241500003")
                        //{
                        data.UserFace = ConvertBase64ToPngBase64(data.UserFace);
                        command2 = IclockOperarion.CommandUploadUserFaceV3(sn, data.PersonCode, data.UserFace);

                        if (command2 != null)
                        {
                            command2.DataId = Guid.NewGuid().ToString();
                            command2.ParentId = rB_ServerRequest.Id;

                            await AddCommand(rB_ServerRequest, command);

                        }
                    }
                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                {
                    TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(rB_ServerRequest.RequestParam);

                    if (data == null)
                        return;
                    await DeleteUserInfo(data);

                    command = IclockOperarion.CommandDeleteUser(sn, data.PersonCode);
                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.UserFace)
                {
                    TA_PersonFace? data = JsonConvert.DeserializeObject<TA_PersonFace>(rB_ServerRequest.RequestParam);
                    if (data == null)
                        return;
                    await SaveUserFace(data);
                    command = IclockOperarion.CommandUploadUserFaceV3(sn, data.PersonCode, data.UserFace);

                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.Device)
                {
                    TA_Device? data = JsonConvert.DeserializeObject<TA_Device>(rB_ServerRequest.RequestParam);
                    if (data == null)
                        return;
                    await SaveDevice(data);

                    //Lưu thông tin caches
                    var terminal = await _deviceAutoPushDbContext.zk_terminal.FirstOrDefaultAsync(x => x.Id == data.Id);
                    if (terminal != null)
                        await _deviceCacheService.Save(terminal);


                    return;
                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.Device)
                {
                    TA_Device? data = JsonConvert.DeserializeObject<TA_Device>(rB_ServerRequest.RequestParam);
                    if (data == null)
                        return;
                    await RemoveDevice(data);
                    //Xoá thông tin caches
                    await _deviceCacheService.Remove(data.SerialNumber);
                    //Xoá lệnh của thiết bị
                    await _deviceCommandCacheService.RemoveAll(data.SerialNumber);

                    return;
                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionGetDeviceInfo && rB_ServerRequest.RequestType == ServerRequestType.Device)
                {
                    TA_Device? data = JsonConvert.DeserializeObject<TA_Device>(rB_ServerRequest.RequestParam);

                    command = IclockOperarion.GetDeviceInfo(data.SerialNumber);
                    return;
                }

                if (command == null)
                    return;
                command.DataId = rB_ServerRequest.Id;
                command.Action = rB_ServerRequest.Action;
                command.SerialNumber = sn;
                if (command.DataId != null)
                {
                    await AddCommand(rB_ServerRequest, command);
                }

                await _deviceCommandCacheService.Set(command);
                //Thêm lệnh ảnh nếu có
                if (command2 != null)
                {
                    await _deviceCommandCacheService.Set(command2);
                }
            }
            catch (Exception e)
            {
                Logger.Warning(e.Message);
                throw;
            }
        }
        public async Task SaveUserInfo(TA_PersonInfo tA_Person)
        {
            try
            {
                bool add = false;
                var person = _deviceAutoPushDbContext.zk_user.FirstOrDefault(x => x.user_code == tA_Person.PersonCode);
                if (person == null)
                {
                    add = true;
                    person = new zk_user();
                    person.Id = tA_Person.Id;
                }
                person.first_name = tA_Person.FisrtName;
                person.last_name = tA_Person.LastName;
                person.full_name = tA_Person.FullName;
                person.user_code = tA_Person.PersonCode;
                person.card_no = tA_Person.UserCard;
                person.privilege = 0;
                if (add)
                {
                    var person2 = _deviceAutoPushDbContext.zk_user.FirstOrDefault(x => x.user_code == tA_Person.PersonCode);
                    if (person2 == null)
                    {
                        await _deviceAutoPushDbContext.zk_user.AddAsync(person);
                        await _deviceAutoPushDbContext.SaveChangesAsync();

                    }
                }


            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return;
            }
        }

        public async Task DeleteUserInfo(TA_PersonInfo tA_Person)
        {
            try
            {
                bool add = false;
                var person = _deviceAutoPushDbContext.zk_user.FirstOrDefault(x => x.Id == tA_Person.Id);
                if (person == null)
                {
                    return;
                }
                else
                {
                    _deviceAutoPushDbContext.zk_user.Remove(person);
                    await _deviceAutoPushDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return;
            }
        }
        public async Task SaveUserFace(TA_PersonFace tA_PersonFace)
        {
            try
            {
                bool add = false;
                var face = _deviceAutoPushDbContext.zk_biophoto.FirstOrDefault(x => x.PersonId == tA_PersonFace.PersonCode);
                if (face == null)
                {
                    add = true;
                    face = new zk_biophoto();
                    face.Id = tA_PersonFace.Id;
                    face.PersonId = tA_PersonFace.PersonCode;
                }

                if (add)
                {
                    await _deviceAutoPushDbContext.zk_biophoto.AddAsync(face);
                }
                else
                    _deviceAutoPushDbContext.Update(face);
                await _deviceAutoPushDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return;
            }
        }

        public async Task AddCommand(RB_ServerRequest request, IclockCommand command)
        {
            try
            {
                bool add = false;

                var data = _deviceAutoPushDbContext.zk_terminalcommandlog.FirstOrDefault(x => x.Id == request.Id);
                if (data == null)
                {
                    add = true;
                    data = new zk_terminalcommandlog();
                    data.Id = request.Id;
                    data.successed = null;
                }
                data.transfer_time = DateTime.Now;
                data.terminal_id = request.DeviceId;
                data.terminal_sn = request.SerialNumber;
                data.parent_id = command.ParentId;
                data.request_id = request.Id;
                data.content = command.Command;
                data.command_id = command.Id;
                data.command_type = request.RequestType;
                data.command_ation = request.Action;
                if (add)
                {
                    _deviceAutoPushDbContext.zk_terminalcommandlog.Add(data);
                }
                _deviceAutoPushDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return;
            }
        }

        public async Task SaveDevice(TA_Device tA_Device)
        {
            try
            {
                bool add = false;
                var data = _deviceAutoPushDbContext.zk_terminal.FirstOrDefault(x => x.Id == tA_Device.Id);
                if (data == null)
                {
                    add = true;
                    data = new zk_terminal();
                    data.Id = tA_Device.Id;
                }
                data.device_id = tA_Device.Id;
                data.ip_address = tA_Device.IpAdress;
                data.port = tA_Device.Port;
                data.sn = tA_Device.SerialNumber;
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
        public async Task RemoveDevice(TA_Device tA_Device)
        {
            try
            {
                var data = _deviceAutoPushDbContext.zk_terminal.FirstOrDefault(x => x.Id == tA_Device.Id);
                if (data == null)
                {
                    return;
                }
                _deviceAutoPushDbContext.zk_terminal.Remove(data);
                await _deviceAutoPushDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return;
            }
        }

        public string ConvertBase64ToPngBase64(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Image image = Image.FromStream(ms))
                {
                    using (MemoryStream pngStream = new MemoryStream())
                    {
                        image.Save(pngStream, ImageFormat.Jpeg);
                        byte[] pngBytes = pngStream.ToArray();
                        return Convert.ToBase64String(pngBytes);
                    }
                }
            }
        }


    }
}
