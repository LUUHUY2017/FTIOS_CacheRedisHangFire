using Hangfire;
using Microsoft.Extensions.Configuration;
using Server.Application.Services.VTSmart;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.SignalRs;

namespace Server.Application.CronJobs;

public class CronJobService : ICronJobService
{
    private readonly IMasterDataDbContext _dbContext;
    private readonly SmartService _smartService;
    private readonly IConfiguration _configuration;
    private readonly ISignalRClientService _signalRService;

    public CronJobService(IMasterDataDbContext dbContext, SmartService smartService, IConfiguration configuration, ISignalRClientService signalRClientService)
    {
        _dbContext = dbContext;
        _smartService = smartService;
        _configuration = configuration;
        _signalRService = signalRClientService;
    }
    public async Task SyncStudentFromSmas()
    {

    }
    public void Write()
    {
        Console.WriteLine("Test");
        RecurringJob.RemoveIfExists("test");
    }
}
