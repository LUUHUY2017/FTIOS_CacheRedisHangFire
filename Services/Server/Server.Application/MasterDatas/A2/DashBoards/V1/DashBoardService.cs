using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.Devices;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.Schools;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.SendEmail;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.StudentAttendances;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.StudentFaces;
using Server.Application.MasterDatas.A2.Devices.Models.Commons;
using Server.Core.Interfaces.A2.SendEmails;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A2.DashBoards.V1;

public class DashBoardService
{
    private readonly IMasterDataDbContext _dbContext;
    private readonly IMapper _mapper;
    //Send email
    private readonly ISendEmailRepository _sendEmailRepository;
    private readonly ISendEmailLogRepository _sendEmailLogRepository;
    public DashBoardService(
        IMasterDataDbContext dbContext,
        IMapper mapper,
        //Send email
        ISendEmailRepository sendEmailRepository,
        ISendEmailLogRepository sendEmailLogRepository
        )
    {
        _dbContext = dbContext;
        _mapper = mapper;
        //Send email
        _sendEmailRepository = sendEmailRepository;
        _sendEmailLogRepository = sendEmailLogRepository;
    }

    //Send Email
    public async Task<Result<TotalSendEmailModel>> GetToTalSendEmail(DashBoardFilter filter)
    {
        try
        {
            var dataSendEmail = await _dbContext.SendEmail
                                    .Where(x => (x.Actived == true)
                                            && (DateTime.Now.Date == ((DateTime)x.TimeSent).Date)
                                            && ((!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? x.OrganizationId == filter.OrganizationId : true)
                                    )
                                    .ToListAsync();
            var data = new TotalSendEmailModel()
            {
                TotalSendEmail = dataSendEmail.Count,
                TotalSendSuccess = dataSendEmail.Count(x => x.Sent == true),
                TotalSendFail = dataSendEmail.Count(x => x.Sent != true),
            };
            return new Result<TotalSendEmailModel>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<TotalSendEmailModel>(null, $"Có lỗi: {ex.Message}", false); 
        }
    }

    //Device
    public async Task<Result<DBDeviceModel1>> GetToTalDevice1(DashBoardFilter filter)
    {
        try
        {
            var dataDevice = await _dbContext.Device.Where(x => (x.Actived == true)
                                                        && ((!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? x.OrganizationId == filter.OrganizationId : true)
                                                    ).ToListAsync();
            var data = new DBDeviceModel1()
            {
                TotalDevice = dataDevice.Count,
                TotalHN = dataDevice.Count(x => x.DeviceModel == DeviceBrandConst.Hanet),
                TotalHNOn = dataDevice.Count(x => x.DeviceModel == DeviceBrandConst.Hanet && x.ConnectionStatus == true),
                TotalHNOff = dataDevice.Count(x => x.DeviceModel == DeviceBrandConst.Hanet && x.ConnectionStatus != true),
                TotalZK = dataDevice.Count(x => x.DeviceModel == DeviceBrandConst.ZKTeco),
                TotalZKOn = dataDevice.Count(x => x.DeviceModel == DeviceBrandConst.ZKTeco && x.ConnectionStatus == true),
                TotalZKOff = dataDevice.Count(x => x.DeviceModel == DeviceBrandConst.ZKTeco && x.ConnectionStatus != true),
            };
            return new Result<DBDeviceModel1>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<DBDeviceModel1>(null, $"Có lỗi: {ex.Message}", false);
        }
    }
    public async Task<Result<List<TotalDeviceModel>>> GetToTalDevice(DateTime? dateTimeFilter)
    {
        try
        {
            var dataDevice = await (from d in _dbContext.Device
                                    join o in _dbContext.Organization
                                    on d.OrganizationId equals o.Id
                                    where d.Actived == true
                                    group d by new { o.Id, o.OrganizationName } into g
                                    select new TotalDeviceModel
                                    {
                                        OrganizationId = g.Key.Id,
                                        OrganizationName = g.Key.OrganizationName,
                                        TotalSchool = 1, 
                                        TotalDevice = g.Count(), 
                                        TotalOnline = g.Count(x => x.ConnectionStatus == true), 
                                        TotalOffline = g.Count(x => x.ConnectionStatus != true) 
                                    })
                                    .ToListAsync();

            return new Result<List<TotalDeviceModel>> (dataDevice.OrderBy(x => x.OrganizationName).ToList(), $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<List<TotalDeviceModel>> (null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<TotalDeviceOrgModel>>> GetToTalDeviceOrg(string orgId)
    {
        try
        {
            var dataDevice = await (from d in _dbContext.Device
                                    join o in _dbContext.Organization
                                    on d.OrganizationId equals o.Id
                                    where (d.Actived == true && d.OrganizationId == orgId)
                                    group d by new { o.Id, o.OrganizationName } into g
                                    select new TotalDeviceOrgModel
                                    {
                                        OrganizationId = g.Key.Id,
                                        OrganizationName = g.Key.OrganizationName,
                                        TotalDevice = g.Count(),
                                        TotalHN = g.Count(x => x.DeviceModel == DeviceBrandConst.Hanet),
                                        TotalZK = g.Count(x => x.DeviceModel == DeviceBrandConst.ZKTeco),
                                    })
                                    .ToListAsync();

            return new Result<List<TotalDeviceOrgModel>>(dataDevice, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<List<TotalDeviceOrgModel>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<DashBoardDevice>>> GetDeviceForStatus(DBDeviceFilter filter)
    {
        try
        {
            var status = filter.Status == "1";
            var dataDevice = await (from d in _dbContext.Device
                                    join o in _dbContext.Organization
                                    on d.OrganizationId equals o.Id
                                    where (d.Actived == true
                                        && ((!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? d.OrganizationId == filter.OrganizationId : true)
                                        && ((!string.IsNullOrEmpty(filter.DeviceModel) && filter.DeviceModel != "0") ? d.DeviceModel == filter.DeviceModel : true)
                                        && ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable != "device_name") ? d.DeviceName.Contains(filter.Key) : true)
                                        && ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable != "device_serial") ? d.SerialNumber.Contains(filter.Key) : true)
                                        && ((!string.IsNullOrEmpty(filter.Status)) ? d.ConnectionStatus == status : true)
                                        )
                                    select new DashBoardDevice(d, o)
                                    )
                                    .ToListAsync();

            return new Result<List<DashBoardDevice>>(dataDevice.OrderBy(x => x.OrganizationName).ToList(), $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<List<DashBoardDevice>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    //StudentFace
    public async Task<Result<DBStudentFaceModel>> GetTotalStudentFace(DashBoardFilter filter)
    {
        try
        {
            var data = await (from s in _dbContext.Student
                              join pf in _dbContext.PersonFace
                              on s.Id equals pf.PersonId into personFaceGroup
                              from pfLeftJoin in personFaceGroup.DefaultIfEmpty() 
                              where s.Actived == true
                                    && ((!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? s.OrganizationId == filter.OrganizationId : true)
                              group new { s, pfLeftJoin } by 1 into g 
                              select new DBStudentFaceModel
                              {
                                  TotalStudent = g.Count(x => x.s != null), 
                                  TotalHasFace = g.Count(x => x.pfLeftJoin != null && !string.IsNullOrEmpty(x.pfLeftJoin.FaceUrl)), 
                                  TotalHasNoFace = g.Count(x => x.pfLeftJoin == null || string.IsNullOrEmpty(x.pfLeftJoin.FaceUrl)) 
                              })
                    .FirstOrDefaultAsync();

            return new Result<DBStudentFaceModel>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<DBStudentFaceModel>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    //DBStudentAttendace
    public async Task<Result<DBStudentAttendaceModel>> GetTotalStudentAttendance(DashBoardFilter filter)
    {
        try
        {
            var data = await _dbContext.TimeAttendenceEvent
                        .Where(t => t.Actived == true 
                                && ((DateTime)t.EventTime).Date == DateTime.Now.Date
                                && ((!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? t.OrganizationId == filter.OrganizationId : true)
                        )
                        .GroupBy(t => true) 
                        .Select(g => new DBStudentAttendaceModel
                        {
                            totalAmount = g.Count(),
                            totalFace = g.Count(t => t.EventType == true),
                            totalCurrent = g.Count() - g.Count(t => t.EventType == true)
                        })
                        .FirstOrDefaultAsync();

            return new Result<DBStudentAttendaceModel>(data ?? new DBStudentAttendaceModel()
            {
                totalAmount = 0,
                totalFace = 0,
                totalCurrent = 0,
            }, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<DBStudentAttendaceModel>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    //DBSchools
    public async Task<Result<TotalSchoolModel>> GetTotalSchool()
    {
        try
        {
            var data = new TotalSchoolModel()
            {
                TotalSchool = await _dbContext.Organization.CountAsync(),
                TotalClass = await _dbContext.ClassRoom.CountAsync(),
                TotalStudent = await _dbContext.Student.CountAsync(),
            };

            return new Result<TotalSchoolModel>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<TotalSchoolModel>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    //DashBoardReport
    public async Task<Result<TotalDashBoardModel>> DashBoardReport(DashBoardFilter filter)
    {
        try
        {
            var data = new TotalDashBoardModel()
            {
                TotalSchoolModel = (await GetTotalSchool()).Data,
                TotalSendEmailModel = (await GetToTalSendEmail(filter)).Data,
                DBDeviceModel1 = (await GetToTalDevice1(filter)).Data,    
                DBStudentFaceModel = (await GetTotalStudentFace(filter)).Data,
                StudentAttendaceModel = (await GetTotalStudentAttendance(filter)).Data,
            };

            return new Result<TotalDashBoardModel>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            //Logger.Error(ex);
            return new Result<TotalDashBoardModel>(null, $"Có lỗi: {ex.Message}", false);
        }
    }
}
