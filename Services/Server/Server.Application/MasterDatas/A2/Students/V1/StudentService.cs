using AMMS.DeviceData.RabbitMq;
using AutoMapper;
using EventBus.Messages;
using MassTransit;
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
    /// RabbitMQ: Gửi thông tin đồng bộ học sinh  qua RabbitMQ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<RB_ServerRequest>> PushPersonByEventBusAsync(Student data)
    {
        try
        {
            var retval = await SyncToDevice(data);

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
    /// Lưu thông tin học sinh vào AMMS
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<DtoStudentRequest>> SaveFromWeb(DtoStudentRequest request)
    {
        try
        {
            var stu = _map.Map<Student>(request);
            var res = await _studentRepository.SaveDataAsync(stu);
            if (res.Succeeded)
            {
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
            }

            try
            {
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
                        await Common.DownloadAndSaveImage(request.ImageSrc, fileName);
                        request.ImageBase64 = Common.ConvertFileImageToBase64(fileName);
                    }
                }
                await _personRepository.SaveImageAsync(stu.Id, request.ImageBase64);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            stu.ImageSrc = request.ImageBase64;
            var revt = await PushPersonByEventBusAsync(stu);
            return new Result<DtoStudentRequest>($"Cập nhật thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<DtoStudentRequest>($"Gửi email lỗi: {e.Message}", false);
        }

    }

    /// <summary>
    /// Lấy thông tin thiết bị đồng bộ
    /// </summary>
    /// <param name="stu"></param>
    /// <returns></returns>
    public async Task<List<RB_ServerRequest>> SyncToDevice(Student stu)
    {
        List<PersonSynToDevice> list = new List<PersonSynToDevice>();
        List<RB_ServerRequest> list_Sync = new List<RB_ServerRequest>();

        try
        {
            // orrgniaztionId
            var _devis = _dbContext.Device.Where(o => o.Actived == true).ToList();
            if (_devis.Any())
            {
                foreach (var device in _devis)
                {
                    var item = _dbContext.PersonSynToDevice.FirstOrDefault(o => o.DeviceId == device.Id && o.PersonId == stu.Id);
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
                    }
                    await _dbContext.SaveChangesAsync();
                    list.Add(item);

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
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return list_Sync;
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
            var _devis = _dbContext.PersonSynToDevice.FirstOrDefault(o => o.Id == request.Id);
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
                         && ((!string.IsNullOrWhiteSpace(request.OrganizationId)) ? _do.OrganizationId == request.OrganizationId : true)

                         orderby _do.CreatedDate descending
                         select new DtoStudentResponse()
                         {
                             Id = _do.Id,
                             Actived = _do.Actived,
                             CreatedDate = _do.CreatedDate,
                             LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,
                             StudentCode = _do.StudentCode,
                             ReferenceId = _do.ReferenceId,

                             FullName = _do.FullName,
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
                             SyncCode = _do.SyncCode,
                             SyncCodeClass = _do.SyncCodeClass,
                             IdentifyNumber = _do.IdentifyNumber,
                             StudentClassId = _do.StudentClassId,
                             SortOrder = _do.SortOrder,
                             Name = _do.Name,
                             SortOrderByClass = _do.SortOrderByClass,
                             GradeCode = _do.GradeCode,
                             ImageBase64 = la != null ? (!string.IsNullOrWhiteSpace(la.FaceData) ? la.FaceData : null) : null
                         });

            return _data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
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

            case "gradecode":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.GradeCode.Contains(filter.Value.Trim()));
                break;

            case "status":
                if (filter.Comparison == 0)
                    query = query.Where(p => p.Status.Contains(filter.Value.Trim()));
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




    public async Task<Result<DtoStudentRequest>> SaveFromService(DtoStudentRequest request)
    {
        try
        {
            var stu = _map.Map<Student>(request);
            await _studentRepository.SaveDataAsync(stu);

            var per = new Person()
            {
                Id = request.Id,
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
}