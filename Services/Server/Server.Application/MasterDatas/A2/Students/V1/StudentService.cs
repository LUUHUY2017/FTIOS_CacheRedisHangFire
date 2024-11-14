using AMMS.DeviceData.RabbitMq;
using AutoMapper;
using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.Students;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;
using System.Drawing;
using System.Globalization;

namespace Server.Application.MasterDatas.A2.Students.V1;

public class StudentService
{
    private readonly IMapper _map;
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;

    private readonly IPersonRepository _personRepository;
    private readonly IStudentRepository _studentRepository;

    private readonly IMasterDataDbContext _dbContext;

    public StudentService(
        IBus bus,
        IMapper map,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        IPersonRepository personRepository,
        IStudentRepository studentRepository,
        IMasterDataDbContext dbContext

        )
    {
        _map = map;
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;

        _personRepository = personRepository;
        _studentRepository = studentRepository;


        _dbContext = dbContext;
    }

    /// <summary>
    /// RabbitMQ: Gửi thông tin 1 học sinh xuống các thiết bị qua RabbitMQ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<RB_ServerRequest>> PushStudentsByEventBusAsync(Student data)
    {
        try
        {
            var retval = await Sync1Student2Devices(data);
            if (retval.Any())
            {
                foreach (var item in retval)
                {
                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_S2D}");
                    await aa.Send(item);
                }
            }
            return new Result<RB_ServerRequest>($"Gửi thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<RB_ServerRequest>($"Gửi email lỗi: {e.Message}", false);
        }
    }
    /// <summary>
    /// RabbitMQ: Gửi tất cả học sinh   xuống 1 thiết bị qua RabbitMQ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<RB_ServerRequest>> PushStudentsByEventBusAsync(Device dev)
    {
        try
        {
            var retval = await SyncStudents2Device(dev);
            if (retval.Any())
            {
                foreach (var item in retval)
                {
                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_S2D}");
                    await aa.Send(item);
                }
            }
            return new Result<RB_ServerRequest>($"Gửi thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<RB_ServerRequest>($"Gửi email lỗi: {e.Message}", false);
        }
    }
    /// <summary>
    /// Đẩy 1 học sinh xuống 1 thiết bị
    /// </summary>
    /// <param name="dev"></param>
    /// <param name="stu"></param>
    /// <returns></returns>
    public async Task<Result<RB_ServerRequest>> PushStudentByEventBusAsync(Device dev, Student stu)
    {
        try
        {
            var retval = await Sync1Student2Device(dev, stu);
            if (retval.Any())
            {
                foreach (var item in retval)
                {
                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_S2D}");
                    await aa.Send(item);
                }
            }
            return new Result<RB_ServerRequest>($"Gửi thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<RB_ServerRequest>($"Gửi email lỗi: {e.Message}", false);
        }
    }


    /// <summary>
    /// Lấy danh sách học sinh AMMS
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IQueryable<DtoStudentResponse>> GetAlls(StudentSearchRequest request)
    {
        try
        {
            bool actived = request.Actived == "1" ? true : false;
            var _data = (from _do in _dbContext.Student

                         join _la in _dbContext.PersonFace on _do.Id equals _la.PersonId into K
                         from la in K.DefaultIfEmpty()

                         where _do.Actived == actived
                            && ((!string.IsNullOrWhiteSpace(request.OrganizationId) && request.OrganizationId != "0") ? _do.OrganizationId == request.OrganizationId : true)

                         orderby _do.ClassName ascending, _do.Name ascending

                         select new DtoStudentResponse()
                         {
                             Id = _do.Id,
                             Actived = _do.Actived,
                             CreatedDate = _do.CreatedDate,
                             LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,
                             StudentCode = _do.StudentCode,
                             SyncCode = _do.SyncCode,
                             ReferenceId = _do.ReferenceId,
                             OrganizationId = _do.OrganizationId,

                             FullName = _do.FullName,
                             Name = _do.Name,
                             DateOfBirth = _do.DateOfBirth,

                             GenderCode = _do.GenderCode,
                             ImageSrc = _do.ImageSrc,
                             ClassId = _do.ClassId,
                             ClassName = _do.ClassName,

                             IsExemptedFull = _do.IsExemptedFull,
                             StatusCode = _do.StatusCode,
                             Status = _do.Status,
                             FullNameOther = _do.FullNameOther,
                             EthnicCode = _do.EthnicCode,
                             PolicyTargetCode = _do.PolicyTargetCode,
                             PriorityEncourageCode = _do.PriorityEncourageCode,

                             SyncCodeClass = _do.SyncCodeClass,
                             IdentifyNumber = _do.IdentifyNumber,
                             StudentClassId = _do.StudentClassId,
                             SortOrder = _do.SortOrder,
                             SortOrderByClass = _do.SortOrderByClass,
                             GradeCode = _do.GradeCode,

                             //ImageBase64 = la != null ? (!string.IsNullOrWhiteSpace(la.FaceData) ? la.FaceData : null) : null,
                             FaceUrl = la != null ? (!string.IsNullOrWhiteSpace(la.FaceUrl) ? "\\" + la.FaceUrl : "") : "",
                             IsFace = la != null ? (!string.IsNullOrWhiteSpace(la.FaceUrl) ? true : false) : false,
                             IsFaceName = la != null ? (!string.IsNullOrWhiteSpace(la.FaceUrl) ? "Có" : "Không") : "Không",
                         });

            return _data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    /// <summary>
    /// Bộ lọc tìm kiếm
    /// </summary>
    /// <param name="query"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IQueryable<DtoStudentResponse>> ApplyFilter(IQueryable<DtoStudentResponse> query, FilterItems filter)
    {
        switch (filter.PropertyName.ToLower())
        {
            case "fullname":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.FullName.Contains(filter.Value.Trim()));
                break;
            case "studentcode":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.StudentCode.Contains(filter.Value.Trim()));
                break;
            case "classid":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.ClassId.Contains(filter.Value.Trim()));
                break;
            case "organizationid":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.OrganizationId.Contains(filter.Value.Trim()));
                break;
            case "classname":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.ClassName.Contains(filter.Value.Trim()));
                break;
            case "synccode":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.SyncCode.Contains(filter.Value.Trim()));
                break;

            case "gradecode":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.GradeCode.Contains(filter.Value.Trim()));
                break;

            case "status":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.Status.Contains(filter.Value.Trim()));
                break;

            case "isfacename":
                if (!string.IsNullOrWhiteSpace(filter.Value))
                    query = query.Where(p => p.IsFaceName.Contains(filter.Value.Trim()));
                break;
            case "identifynumber":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.IdentifyNumber.Contains(filter.Value.Trim()));
                break;



            default:
                break;
        }
        return query;
    }


    /// <summary>
    /// Cập nhật trạng thái đồng bộ từ RabbitMQ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> SaveStatuSyncDevice(RB_ServerResponse request)
    {
        bool statusSync = false;
        try
        {
            var _devis = await _dbContext.PersonSynToDevice.FirstOrDefaultAsync(o => o.Id == request.Id);
            if (_devis != null)
            {
                _devis.SynStatus = request.IsSuccessed;
                _devis.SynFaceStatus = request.IsSuccessed;
                _devis.SynMessage = request.Message;
                _devis.SynFaceMessage = request.Content;
                _devis.LastModifiedDate = DateTime.Now;
            }
            await _dbContext.SaveChangesAsync();
            statusSync = true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return statusSync;
    }


    /// <summary>
    /// Lưu thông tin học sinh vào AMMS, ảnh khuôn mặt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<DtoStudentRequest>> SaveFromWeb(DtoStudentRequest request)
    {
        try
        {
            var stu = _map.Map<Student>(request);


            //var res = await _studentRepository.SaveDataAsync(stu);
            //if (res.Succeeded)
            //{
            //    var per = new Person()
            //    {
            //        Id = res.Data.Id,
            //        Actived = true,
            //        PersonCode = request.StudentCode,
            //        FirstName = request.Name,
            //        LastName = request.FullName,
            //        CitizenId = request.IdentifyNumber,
            //    };
            //    var data = await _personRepository.SaveAsync(per);
            //}


            DateTime dateStudent = DateTime.Now;
            string dateString = stu.DateOfBirth;
            string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy", "MM-dd-yyyy", "dd/MM/yyyy", "MM/dd/yyyy", "dd/MM/yyyy", };
            bool success = DateTime.TryParseExact(
                dateString,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateStudent
            );


            var imageFolder = Common.GetImageDatePathFolder(dateStudent, "images\\students");
            var imageFullFolder = Common.GetImageDateFullFolder(dateStudent, "images\\students");

            string imageName = stu.Id + ".jpg";
            string fileName = imageFullFolder + imageName;
            string folderName = imageFolder + imageName;

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                Image img = Common.Base64ToImage(request.ImageBase64);
                if (File.Exists(fileName))
                    File.Delete(fileName);
                //img.Save(fileName);
                Common.SaveJpeg1(fileName, img, 100);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(request.ImageSrc))
                {
                    // Lưu ảnh online
                    //await Common.DownloadAndSaveImage(request.ImageSrc, fileName);

                    /// Parse ảnh sàng base64
                    var rootFolder = Common.GetCurentFolder();
                    string fileNames = rootFolder + request.ImageSrc;
                    request.ImageBase64 = Common.ConvertFileImageToBase64(fileNames);
                }
            }


            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
                await _personRepository.SaveImageAsync(stu.Id, request.ImageBase64, folderName);

            stu.ImageSrc = request.ImageBase64;
            var revt = await PushStudentsByEventBusAsync(stu);

            return new Result<DtoStudentRequest>($"Cập nhật thành công", true);
        }
        catch (Exception e)
        {
            Logger.Warning(e.Message);
            return new Result<DtoStudentRequest>($"Gửi email lỗi: {e.Message}", false);
        }

    }
    /// <summary>
    /// Lưu thông tin học sinh từ SMAS
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<DtoStudentRequest>> SaveFromService(Student request)
    {
        try
        {
            var res = await _studentRepository.SaveDataAsync(request);
            if (!res.Succeeded)
                return new Result<DtoStudentRequest>($"Cập nhật không thành công", false);

            var per = new Person()
            {
                Id = res.Data.Id,
                Actived = true,
                PersonCode = request.StudentCode,
                FirstName = request.Name,
                LastName = request.FullName,
                CitizenId = request.IdentifyNumber,
            };
            var data = await _personRepository.SaveAsync(per);
            return new Result<DtoStudentRequest>($"Cập nhật thành công", true);

        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<DtoStudentRequest>($"Gửi email lỗi: {e.Message}", false);
        }

    }
    /// <summary>
    /// Lưu ảnh học sinh từ Hanet
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<DtoStudentRequest>> SaveImagePerson(TA_PersonInfo request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.PersonCode))
                return new Result<DtoStudentRequest>($"Phải có mã học sinh", false);

            var stu = await _dbContext.Student.FirstOrDefaultAsync(o => o.StudentCode == request.PersonCode.ToString());
            if (stu == null)
                return new Result<DtoStudentRequest>($"Không tìm thấy thông tin học sinh", false);

            var per = await _dbContext.Person.FirstOrDefaultAsync(o => o.Id == stu.Id);
            if (per == null)
                return new Result<DtoStudentRequest>($"Không tìm thấy thông tin người", false);


            DateTime dateStudent = DateTime.Now;
            string dateString = stu.DateOfBirth;
            string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy", "MM-dd-yyyy", "dd/MM/yyyy", "MM/dd/yyyy", "dd/MM/yyyy", };
            bool success = DateTime.TryParseExact(
                dateString,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateStudent
            );

            var imageFolder = Common.GetImageDatePathFolder(dateStudent, "images\\students");
            var imageFullFolder = Common.GetImageDateFullFolder(dateStudent, "images\\students");

            string imageName = stu.Id + ".jpg";
            string fileName = imageFullFolder + imageName;
            string folderName = imageFolder + imageName;

            if (!string.IsNullOrWhiteSpace(request.UserFace))
            {
                Image img = Common.Base64ToImage(request.UserFace);
                if (File.Exists(fileName))
                    File.Delete(fileName);
                //img.Save(fileName);
                Common.SaveJpeg1(fileName, img, 100);
            }
            await _personRepository.SaveImageAsync(stu.Id, request.UserFace, folderName);

            //stu.ImageSrc = request.UserFace;
            //var revt = await PushPersonByEventBusAsync(stu);

            return new Result<DtoStudentRequest>($"Cập nhật thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<DtoStudentRequest>($"Gửi email lỗi: {e.Message}", false);
        }

    }



    /// <summary>
    /// Đẩy 1 học sinh xuống toàn bộ thiết bị
    /// </summary>
    /// <param name="stu"></param>
    /// <returns></returns>
    public async Task<List<RB_ServerRequest>> Sync1Student2Devices(Student stu)
    {
        List<RB_ServerRequest> list_Sync = new List<RB_ServerRequest>();

        try
        {
            var _devis = _dbContext.Device.Where(o => o.Actived == true && stu.OrganizationId == o.OrganizationId).ToList();
            if (_devis == null || _devis.Count == 0)
                return list_Sync;

            foreach (var device in _devis)
            {
                var item = await _dbContext.PersonSynToDevice.Where(o => o.DeviceId == device.Id && o.PersonId == stu.Id).FirstOrDefaultAsync();

                if (item == null)
                {
                    item = new PersonSynToDevice()
                    {
                        DeviceId = device.Id,
                        PersonId = stu.Id,
                        SynAction = ServerRequestAction.ActionAdd,
                        LastModifiedDate = DateTime.Now
                    };
                    await _dbContext.PersonSynToDevice.AddAsync(item);
                }
                else
                {
                    item.SynAction = ServerRequestAction.ActionAdd;
                    item.SynStatus = null;
                    item.SynFaceStatus = null;
                    item.SynMessage = null;
                }
                await _dbContext.SaveChangesAsync();

                var _TA_PersonInfo = new TA_PersonInfo()
                {
                    Id = stu.Id,
                    DeviceId = device.Id,
                    DeviceModel = device.DeviceModel,
                    FisrtName = stu.Name,
                    FullName = stu.FullName,
                    PersonCode = stu.StudentCode,
                    SerialNumber = device.SerialNumber,
                    UserFace = stu.ImageSrc
                };

                var param = JsonConvert.SerializeObject(_TA_PersonInfo);
                var list_SyncItem = new RB_ServerRequest()
                {
                    Id = item.Id,
                    Action = ServerRequestAction.ActionAdd,
                    SerialNumber = device.SerialNumber,
                    DeviceId = device.Id,
                    DeviceModel = device.DeviceModel,
                    RequestType = ServerRequestType.UserInfo,
                    RequestParam = param,
                    SchoolId = device.OrganizationId,
                };

                list_Sync.Add(list_SyncItem);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return list_Sync;
    }

    /// <summary>
    /// Đẩy toàn bộ học sinh xuống 1 thiết bị
    /// </summary>
    /// <param name="stu"></param>
    /// <returns></returns>
    public async Task<List<RB_ServerRequest>> SyncStudents2Device(Device dev)
    {
        List<RB_ServerRequest> list_Sync = new List<RB_ServerRequest>();

        try
        {
            var imageFullFolder = Common.GetCurentFolder();

            var students = await _dbContext.Student.Where(o => o.Actived == true && o.OrganizationId == dev.OrganizationId).ToListAsync();
            var faces = await _dbContext.PersonFace.Where(o => o.Actived == true && o.OrganizationId == dev.OrganizationId).ToListAsync();

            if (students == null || students.Count == 0)
                return list_Sync;


            foreach (var stu in students)
            {
                var item = _dbContext.PersonSynToDevice.FirstOrDefault(o => o.DeviceId == dev.Id && o.PersonId == stu.Id);
                var face = faces.Where(o => o.Actived == true && o.PersonId == stu.Id).FirstOrDefault();

                if (item == null)
                {
                    item = new PersonSynToDevice()
                    {
                        DeviceId = dev.Id,
                        PersonId = stu.Id,
                        SynAction = ServerRequestAction.ActionAdd,
                        LastModifiedDate = DateTime.Now
                    };
                    await _dbContext.PersonSynToDevice.AddAsync(item);
                }
                else
                {
                    item.SynAction = ServerRequestAction.ActionAdd;
                    item.SynStatus = null;
                    item.SynFaceStatus = null;
                    item.SynMessage = null;
                }
                await _dbContext.SaveChangesAsync();


                string imageBase64 = string.Empty;
                if (face != null && face.FaceUrl != null)
                {
                    string fileName = imageFullFolder + face.FaceUrl;
                    imageBase64 = Common.ConvertFileImageToBase64(fileName);
                }

                var _TA_PersonInfo = new TA_PersonInfo()
                {
                    Id = stu.Id,
                    DeviceId = dev.Id,
                    DeviceModel = dev.DeviceModel,
                    FisrtName = stu.Name,
                    FullName = stu.FullName,
                    PersonCode = stu.StudentCode,
                    SerialNumber = dev.SerialNumber,
                    //UserFace = face?.FaceData
                    UserFace = imageBase64
                };
                var param = JsonConvert.SerializeObject(_TA_PersonInfo);


                var list_SyncItem = new RB_ServerRequest()
                {
                    Id = item.Id,
                    DeviceId = dev.Id,
                    Action = ServerRequestAction.ActionAdd,
                    SerialNumber = dev.SerialNumber,
                    SchoolId = dev.OrganizationId,
                    DeviceModel = dev.DeviceModel,
                    RequestType = ServerRequestType.UserInfo,
                    RequestParam = param,
                };
                list_Sync.Add(list_SyncItem);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return list_Sync;
    }

    /// <summary>
    /// Đẩy 1 học sinh xuống 1 thiết bị
    /// </summary>
    /// <param name="stu"></param>
    /// <returns></returns>
    public async Task<List<RB_ServerRequest>> Sync1Student2Device(Device dev, Student stu)
    {
        List<RB_ServerRequest> list_Sync = new List<RB_ServerRequest>();

        try
        {
            var imageFullFolder = Common.GetCurentFolder();
            var face = await _dbContext.PersonFace.Where(o => o.Actived == true && o.PersonId == stu.Id).FirstOrDefaultAsync();
            var item = _dbContext.PersonSynToDevice.FirstOrDefault(o => o.DeviceId == dev.Id && o.PersonId == stu.Id);

            if (item == null)
            {
                item = new PersonSynToDevice()
                {
                    DeviceId = dev.Id,
                    PersonId = stu.Id,
                    SynAction = ServerRequestAction.ActionAdd,
                    LastModifiedDate = DateTime.Now
                };
                await _dbContext.PersonSynToDevice.AddAsync(item);
            }
            else
            {
                item.SynAction = ServerRequestAction.ActionAdd;
                item.SynStatus = null;
                item.SynFaceStatus = null;
                item.SynMessage = null;
            }
            await _dbContext.SaveChangesAsync();



            string imageBase64 = string.Empty;
            if (face != null && face.FaceUrl != null)
            {
                string fileName = imageFullFolder + face.FaceUrl;
                imageBase64 = Common.ConvertFileImageToBase64(fileName);
            }

            var _TA_PersonInfo = new TA_PersonInfo()
            {
                Id = stu.Id,
                DeviceId = dev.Id,
                DeviceModel = dev.DeviceModel,
                FisrtName = stu.Name,
                FullName = stu.FullName,
                PersonCode = stu.StudentCode,
                SerialNumber = dev.SerialNumber,
                //UserFace = face?.FaceData
                UserFace = imageBase64
            };
            var param = JsonConvert.SerializeObject(_TA_PersonInfo);


            var list_SyncItem = new RB_ServerRequest()
            {
                Id = item.Id,
                DeviceId = dev.Id,
                Action = ServerRequestAction.ActionAdd,
                SerialNumber = dev.SerialNumber,
                SchoolId = dev.OrganizationId,
                DeviceModel = dev.DeviceModel,
                RequestType = ServerRequestType.UserInfo,
                RequestParam = param,
            };
            list_Sync.Add(list_SyncItem);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return list_Sync;
    }


    public async Task<Result<PersonFace>> GetFaceByPersonId(string personId)
    {
        try
        {
            var _devis = await _dbContext.PersonFace.FirstOrDefaultAsync(o => o.PersonId == personId);
            if (_devis == null)
                return new Result<PersonFace>($"Không có ảnh khuôn mặt", false);

            return new Result<PersonFace>(_devis, $"Cập nhật thành công", true);
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
            return new Result<PersonFace>($"Lỗi: " + ex.Message, false);
        }
    }
}