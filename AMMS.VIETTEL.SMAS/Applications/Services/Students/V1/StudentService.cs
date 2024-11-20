using AMMS.DeviceData.RabbitMq;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Persons;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Students;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using AutoMapper;
using EventBus.Messages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;
using System.Drawing;
using System.Globalization;

namespace AMMS.VIETTEL.SMAS.Applications.Services.Students.V1;


public class StudentService
{
    private readonly IMapper _map;
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;

    private readonly IPersonRepository _personRepository;
    private readonly IStudentRepository _studentRepository;

    private readonly ViettelDbContext _dbContext;

    public StudentService(
        IMapper map,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,

        IPersonRepository personRepository,
        IStudentRepository studentRepository,
        ViettelDbContext dbContext

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
            return new Result<RB_ServerRequest>($"Lỗi: {e.Message}", false);
        }
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
    public async Task<Result<DtoStudentRequest>> SaveFromSmasWeb(DtoStudentRequest request)
    {
        try
        {
            var stu = _map.Map<Student>(request);

            DateTime dateStudent = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(stu.DateOfBirth))
            {
                string dateString = stu.DateOfBirth;
                string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy", "MM-dd-yyyy", "dd/MM/yyyy", "MM/dd/yyyy", "dd/MM/yyyy", };
                bool success = DateTime.TryParseExact(
                    dateString,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateStudent
                );
            }

            var imageFolder = Common.GetImageDatePathFolder(dateStudent, "images\\students");
            var imageFullFolder = Common.GetImageDateFullFolder(dateStudent, "images\\students");

            string imageName = stu.Id + ".jpg";
            string fileName = imageFullFolder + imageName;
            string folderName = imageFolder + imageName;


            if (!string.IsNullOrWhiteSpace(request.ImageSrc))
            {
                // Lưu ảnh online
                await Common.DownloadAndSaveImage(request.ImageSrc, fileName);
                request.ImageBase64 = Common.ConvertFileImageToBase64(folderName);
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
            return new Result<DtoStudentRequest>($"Lỗi: {e.Message}", false);
        }

    }
    /// <summary>
    /// Lưu thông tin học sinh từ SMAS
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<Student>> SaveFromService(Student request)
    {
        try
        {
            var res = await _studentRepository.SaveDataAsync(request);
            if (!res.Succeeded)
                return new Result<Student>($"Cập nhật không thành công", false);

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
            return new Result<Student>(res.Data, $"Cập nhật thành công", true);

        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<Student>($"Lỗi: {e.Message}", false);
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

            if (!string.IsNullOrWhiteSpace(stu.DateOfBirth))
            {
                string dateString = stu.DateOfBirth;
                string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy", "MM-dd-yyyy", "dd/MM/yyyy", "MM/dd/yyyy", "dd/MM/yyyy", };
                bool success = DateTime.TryParseExact(
                    dateString,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateStudent
                );
            }
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
            return new Result<DtoStudentRequest>($"Lỗi: {e.Message}", false);
        }

    }
    /// <summary>
    /// Lưu thông tin học sinh vào AMMS, ảnh khuôn mặt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<Student>> SaveImageFromService(Student stu)
    {
        try
        {
            DateTime dateStudent = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(stu.DateOfBirth))
            {
                string dateString = stu.DateOfBirth;
                string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy", "MM-dd-yyyy", "dd/MM/yyyy", "MM/dd/yyyy", "dd/MM/yyyy", };
                bool success = DateTime.TryParseExact(
                    dateString,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateStudent
                );
            }

            var imageFolder = Common.GetImageDatePathFolder(dateStudent, "images\\students");
            var imageFullFolder = Common.GetImageDateFullFolder(dateStudent, "images\\students");

            string imageName = stu.Id + ".jpg";
            string fileName = imageFullFolder + imageName;
            string folderName = imageFolder + imageName;


            if (!string.IsNullOrWhiteSpace(stu.ImageSrc))
            {
                // Lưu ảnh online
                await Common.DownloadAndSaveImage(stu.ImageSrc, fileName);
                await _personRepository.SaveImageAsync(stu.Id, "", folderName);
            }

            return new Result<Student>($"Cập nhật thành công", true);
        }
        catch (Exception e)
        {
            Logger.Warning(e.Message);
            return new Result<Student>($"Lỗi: {e.Message}", false);
        }
    }
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
                        OrganizationId = stu.OrganizationId,
                        PersonId = stu.Id,
                        SynAction = ServerRequestAction.ActionAdd,
                        LastModifiedDate = DateTime.Now
                    };
                    await _dbContext.PersonSynToDevice.AddAsync(item);
                }
                else
                {
                    item.SynAction = ServerRequestAction.ActionAdd;
                    item.OrganizationId = stu.OrganizationId;
                    item.SynStatus = null;
                    item.SynFaceStatus = null;
                    item.SynMessage = null;
                    item.LastModifiedDate = DateTime.Now;
                }
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
                    RequestId = DateTime.Now.TimeOfDay.Ticks,
                };

                list_Sync.Add(list_SyncItem);
            }

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return list_Sync;
    }

    /// <summary>
    /// Lưu thông tin học sinh vào AMMS, ảnh khuôn mặt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<Student>> PushImageToRabbit(Student stu)
    {
        try
        {
            RB_DataResponse rB_Response = new RB_DataResponse()
            {
                Id = stu.Id.ToString(),
                Content = JsonConvert.SerializeObject(stu),
                ReponseType = RB_DataResponseType.UserInfo,
            };
            var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.SMAS_Auto_Push_Server}");
            await aa.Send(rB_Response);

            return new Result<Student>($"Cập nhật thành công", true);
        }
        catch (Exception e)
        {
            Logger.Warning(e.Message);
            return new Result<Student>($"Lỗi: {e.Message}", false);
        }
    }
}


