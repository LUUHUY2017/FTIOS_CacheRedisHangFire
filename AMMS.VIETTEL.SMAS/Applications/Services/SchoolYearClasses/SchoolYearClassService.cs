using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ClassRooms;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.SchoolYears;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.StudentClassRoomYears;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using AutoMapper;
using EventBus.Messages;
using MassTransit;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace AMMS.VIETTEL.SMAS.Applications.Services.SchoolYearClasses;

public class SchoolYearClassService
{
    private readonly IMapper _map;
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;

    private readonly ISchoolYearRepository _scchoolYearRepository;
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IStudentClassRoomYearRepository _stuClasRomYearRepository;

    private readonly ViettelDbContext _dbContext;

    public SchoolYearClassService(
        IBus bus,
        IMapper map,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        ISchoolYearRepository scchoolYearRepository,
        IClassRoomRepository classRoomRepository,
        IStudentClassRoomYearRepository stuClasRomYearRepository,

        ViettelDbContext dbContext

        )
    {
        _map = map;
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;

        _scchoolYearRepository = scchoolYearRepository;
        _classRoomRepository = classRoomRepository;
        _stuClasRomYearRepository = stuClasRomYearRepository;

        _dbContext = dbContext;
    }


    public async Task<Result<StudentClassRoomYear>> SaveFromService(Student req)
    {
        try
        {
            DateTime now = DateTime.Now;

            var schoolYear = new SchoolYear()
            {
                Actived = true,
                Name = now.Year.ToString(),
                Start = new DateTime(now.Year, 1, 1),
                End = new DateTime(now.Year, 12, 31),
                OrganizationId = req.OrganizationId,
                Depcription = req.OrganizationId,
            };
            var sc = await _scchoolYearRepository.SaveDataAsync(schoolYear);
            if (!sc.Succeeded)
                return new Result<StudentClassRoomYear>($"Lỗi 1", false);

            var classRoom = new ClassRoom()
            {
                Actived = true,
                ReferenceId = req.ClassId,
                SchoolId = req.OrganizationId,
                Name = req.ClassName,
                OrganizationId = req.OrganizationId,
            };
            var cl = await _classRoomRepository.SaveDataAsync(classRoom);
            if (!cl.Succeeded)
                return new Result<StudentClassRoomYear>($"Lỗi 2", false);

            var stuclsr = new StudentClassRoomYear()
            {
                Actived = true,
                Name = cl.Data.Name,
                OrganizationId = req.OrganizationId,

                SchoolId = req.OrganizationId,
                ClassRoomId = cl.Data.Id,
                SchoolYearId = sc.Data.Id,

            };
            var stdcl = await _stuClasRomYearRepository.SaveDataAsync(stuclsr);
            if (!stdcl.Succeeded)
                return new Result<StudentClassRoomYear>($"Lỗi 3", false);

            return new Result<StudentClassRoomYear>(stdcl.Data, $"Cập nhật thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<StudentClassRoomYear>($"Lỗi: {e.Message}", false);
        }

    }

}