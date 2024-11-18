using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.SendEmail;
using Server.Core.Interfaces.A2.SendEmails;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Loggers;

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

    public async Task<Result<TotalSendEmailModel>> GetToTalSendEmail(DateTime? dateTimeFilter)
    {
        try
        {
            var dataSendEmail = await _dbContext.SendEmail
                                    .Where(x => (x.Actived == true)
                                            && (dateTimeFilter != null ? ((DateTime)dateTimeFilter).Date == ((DateTime)x.TimeSent).Date : true)
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
            Logger.Error(ex);
            return new Result<TotalSendEmailModel>(null, $"Có lỗi: {ex.Message}", false); 
        }
    }
}
