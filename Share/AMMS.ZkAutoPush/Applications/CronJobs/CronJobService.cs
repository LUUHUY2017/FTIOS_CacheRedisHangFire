﻿using AMMS.ZkAutoPush.Applications.V1;
using AMMS.ZkAutoPush.Datas.Databases;
using Hangfire;
using Share.WebApp.Controllers;
using Shared.Core.SignalRs;

namespace AMMS.ZkAutoPush.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{
    private readonly DeviceAutoPushDbContext _dbContext;
    private readonly DeviceCacheService _deviceCacheService;
    private readonly IConfiguration _configuration;
    private readonly ISignalRClientService _signalRService;
    private readonly StartupDataService _startupDataService;
    public CronJobService(DeviceAutoPushDbContext deviceAutoPushDbContext, DeviceCacheService deviceCacheService, IConfiguration configuration, ISignalRClientService signalRClientService, StartupDataService startupDataService )
    {
        _dbContext = deviceAutoPushDbContext;
        _deviceCacheService = deviceCacheService;
        _configuration = configuration;
        _signalRService = signalRClientService;
        _startupDataService = startupDataService;
    }
    public async Task Test()
    {
        var newCronExpression = "*/1 * * * *";
        RecurringJob.AddOrUpdate("test", () => Write(), newCronExpression, TimeZoneInfo.Local);
    }
    public void Write()
    {
        Console.WriteLine("Test");
        RecurringJob.RemoveIfExists("test");
    }
}
