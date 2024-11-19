using AMMS.DeviceData.RabbitMq;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Persons;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Students;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using AutoMapper;
using EventBus.Messages;
using Microsoft.EntityFrameworkCore;
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

}


