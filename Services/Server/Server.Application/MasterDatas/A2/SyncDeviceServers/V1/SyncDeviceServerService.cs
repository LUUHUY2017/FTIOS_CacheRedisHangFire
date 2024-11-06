using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.Students;
using Server.Core.Interfaces.A2.SyncDeviceServers.Requests;
using Server.Core.Interfaces.A2.SyncDeviceServers.Responses;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.SignalRs;

namespace Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
public partial class SyncDeviceServerService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;

    private readonly IPersonRepository _personRepository;
    private readonly IStudentRepository _studentRepository;

    private readonly IMasterDataDbContext _dbContext;


    public SyncDeviceServerService(
        IBus bus,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        IPersonRepository personRepository,
        IStudentRepository studentRepository,
        IMasterDataDbContext dbContext
        )
    {
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;

        _personRepository = personRepository;
        _studentRepository = studentRepository;
        _dbContext = dbContext;
    }


    public async Task<Result<List<SyncDeviceServerReportRes>>> GetAlls(SyncDeviceServerFilterReq request)
    {
        try
        {
            var _data = await (from _do in _dbContext.A2_PersonSynToDevice

                               join _la in _dbContext.A2_Student on _do.PersonId equals _la.Id into K
                               from la in K.DefaultIfEmpty()

                               join _de in _dbContext.A2_Device on _do.DeviceId equals _de.Id into KG
                               from de in KG.DefaultIfEmpty()


                               where
                                (request.StartDate != null ? _do.LastModifiedDate.Date >= request.StartDate.Value.Date : true)
                                && (request.EndDate != null ? _do.LastModifiedDate.Date <= request.EndDate.Value.Date : true)

                                && (!string.IsNullOrWhiteSpace(request.ClassId) ? la.ClassId == request.ClassId : true)

                               orderby _do.LastModifiedDate descending
                               select new SyncDeviceServerReportRes()
                               {
                                   Id = _do.Id,
                                   Actived = _do.Actived,
                                   CreatedDate = _do.CreatedDate,
                                   LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,

                                   PersonId = _do.PersonId,
                                   StudentCode = la != null ? la.StudentCode : "",
                                   StudentName = la != null ? la.FullName : "",
                                   ClassName = la != null ? la.ClassName : "",

                                   DeviceId = _do.DeviceId,
                                   DeviceName = de != null ? de.DeviceName : "",
                                   IPAddress = de != null ? de.IPAddress : "",

                                   SynAction = _do.SynAction,
                                   SynCardMessage = _do.SynCardMessage,
                                   SynCardStatus = _do.SynCardStatus,
                                   SynFaceMessage = _do.SynFaceMessage,
                                   SynFaceStatus = _do.SynFaceStatus,
                                   SynFingerMessage = _do.SynFingerMessage,
                                   SynFingerStatus = _do.SynFingerStatus,
                                   SynMessage = _do.SynMessage,
                                   SynStatus = _do.SynStatus,

                               }).ToListAsync();

            return new Result<List<SyncDeviceServerReportRes>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<SyncDeviceServerReportRes>>("Lỗi: " + ex.ToString(), false);
        }
    }


}

