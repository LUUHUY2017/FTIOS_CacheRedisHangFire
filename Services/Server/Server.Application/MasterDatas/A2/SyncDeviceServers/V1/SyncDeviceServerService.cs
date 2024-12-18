﻿using EventBus.Messages;
using MassTransit;
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

    public async Task<IQueryable<SyncDeviceServerReportRes>> GetAlls(SyncDeviceServerFilterReq request)
    {
        try
        {
            var _data = (from _do in _dbContext.PersonSynToDevice

                         join _la in _dbContext.Student on _do.PersonId equals _la.Id into K
                         from la in K.DefaultIfEmpty()

                         join _de in _dbContext.Device on _do.DeviceId equals _de.Id into KG
                         from de in KG.DefaultIfEmpty()

                             //join _or in _dbContext.Organization on la.OrganizationId equals _or.Id into OG
                             //from or in OG.DefaultIfEmpty()

                         where
                          (request.StartDate != null ? _do.LastModifiedDate >= request.StartDate : true)
                          && (request.EndDate != null ? _do.LastModifiedDate <= request.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1) : true)
                          && (!string.IsNullOrWhiteSpace(request.DeviceId) ? _do.DeviceId == request.DeviceId : true)
                          && ((!string.IsNullOrWhiteSpace(request.OrganizationId) && request.OrganizationId != "0") ? la.OrganizationId == request.OrganizationId : true)

                         orderby _do.SynStatus ascending, _do.LastModifiedDate descending
                         select new SyncDeviceServerReportRes()
                         {
                             Id = _do.Id,
                             Actived = _do.Actived,
                             CreatedDate = _do.CreatedDate,
                             LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,

                             PersonId = _do.PersonId,
                             StudentCode = la != null ? la.StudentCode : "",
                             StudentName = la != null ? la.FullName : "",
                             Name = la != null ? la.Name : "",
                             ClassName = la != null ? la.ClassName : "",


                             DeviceId = _do.DeviceId,
                             DeviceName = de != null ? de.DeviceName : "",
                             IPAddress = de != null ? de.IPAddress : "",

                             SynStatus = _do.SynStatus,
                             SynAction = _do.SynAction,
                             SynCardMessage = _do.SynCardMessage,
                             SynCardStatus = _do.SynCardStatus,
                             SynFaceMessage = _do.SynFaceMessage,
                             SynFaceStatus = _do.SynFaceStatus,
                             SynFingerMessage = _do.SynFingerMessage,
                             SynFingerStatus = _do.SynFingerStatus,
                             SynMessage = _do.SynMessage,

                         });

            return _data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<IQueryable<SyncDeviceServerReportRes>> ApplyFilter(IQueryable<SyncDeviceServerReportRes> query, List<FilterItems>? filters)
    {
        if (filters == null && filters.Count == 0)
            return query;

        foreach (var filter in filters)
        {
            switch (filter.PropertyName.ToLower())
            {
                case "studentname":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.StudentName.Contains(filter.Value.Trim()));
                    break;
                case "studentcode":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.StudentCode.Contains(filter.Value.Trim()));
                    break;

                case "devicename":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.DeviceName.Contains(filter.Value.Trim()));
                    break;

                case "synaction":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.SynAction.Contains(filter.Value.Trim()));
                    break;

                case "synstatus":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                        var statusValue = filter.Value.ToLower();
                        if (statusValue == "true")
                            query = query.Where(p => p.SynStatus == true);
                        else if (statusValue == "false")
                            query = query.Where(p => p.SynStatus == false);
                        else
                            query = query.Where(p => p.SynStatus == null);
                    }
                    break;

                case "organizationid":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.OrganizationId.Contains(filter.Value.Trim()));
                    break;
                case "classname":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.ClassName.Contains(filter.Value.Trim()));
                    break;

                case "synmessage":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.SynMessage.Contains(filter.Value.Trim()));
                    break;

                case "synfacemessage":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.SynFaceMessage.Contains(filter.Value.Trim()));
                    break;
                default:
                    break;
            }
        }
        return query;
    }
}

