using AMMS.VIETTEL.SMAS.APIControllers.ScheduleSendMails.V1.Requests;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleSendEmails;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.SendEmails;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;

namespace AMMS.VIETTEL.SMAS.APIControllers.SendEmails.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SendEmailLogController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ISendEmailLogRepository _sendEmailLogRepository;


    public SendEmailLogController(
        IMediator mediator,
        IMapper mapper,
        ISendEmailLogRepository sendEmailLogRepository
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _sendEmailLogRepository = sendEmailLogRepository;

    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(ScheduleSendEmailLogFilter request)
    {

        var model = _mapper.Map<ScheduleSendEmailLogModel>(request);
        var data = await _sendEmailLogRepository.GetAlls(model);
        return Ok(data);
    }


    /// <summary>
    /// Danh sách lịch sử 1 record
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Detail")]
    public async Task<ActionResult> Detail(string id)
    {
        var data = await _sendEmailLogRepository.Get(id);
        return Ok(data);
    }



    /// <summary>
    ///  Xóa vĩnh viễn
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<ActionResult> Delete([FromBody] SendEmailDeteleRequest request)
    {
        //request.UserId = UserId;
        var result = await _sendEmailLogRepository.DeleteAsync(request.Id);
        return Ok(result);
    }
}
