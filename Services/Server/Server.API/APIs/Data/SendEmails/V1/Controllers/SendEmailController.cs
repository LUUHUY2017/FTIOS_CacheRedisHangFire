﻿using AMMS.Notification.Workers.Emails;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.ScheduleSendMails.V1.Requests;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Server.Core.Interfaces.A2.SendEmails;
using Share.WebApp.Controllers;
using Shared.Core.Commons;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;

namespace Server.API.APIs.Data.ScheduleSendMails.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SendEmailController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ISendEmailRepository _sendEmailRepository;
    private readonly SendEmailMessageService1 _sendEmailMessageService1;


    public SendEmailController(
        IMediator mediator,
        IMapper mapper,
        ISendEmailRepository sendEmailRepository,
        SendEmailMessageService1 sendEmailMessageService1
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _sendEmailRepository = sendEmailRepository;
        _sendEmailMessageService1 = sendEmailMessageService1;

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
        var data = await _sendEmailRepository.GetAlls(model);
        return Ok(data);
    }


    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(SendEmailRequest request)
    {
        try
        {
            var model = _mapper.Map<SendEmails>(request);
            var retVal = await _sendEmailRepository.UpdateAsync(model);

            try
            {
                var data = retVal.Data;
                var retvlCon = await _sendEmailRepository.GetEmailConfiguration(data.OrganizationId);
                if (retvlCon.Succeeded)
                {
                    EmailConfiguration emailConfig = retvlCon.Data;
                    string fullName = Common.GetCurentFolder() + data.AttachFile;
                    var attachFiles = new List<string>() { fullName };
                    var retVal1 = await _sendEmailMessageService1.SendByEventBusAsync(
                         new SendEmailMessageRequest1()
                         {
                             Id = retVal.Data.Id.ToString(),
                             Message = new MailRequest()
                             {
                                 ToEmail = retVal.Data.ToEmails,
                                 EmailSubject = $"{data.Subject}",
                                 EmailBody = $"{data.Body}",
                                 AttachFiles = attachFiles,
                             },
                             EmailSettings = new EmailSettings()
                             {
                                 EmailFrom = emailConfig.Email,
                                 SmtpHost = emailConfig.Server,
                                 SmtpPort = emailConfig.Port ?? 587,
                                 SmtpPass = emailConfig.PassWord,
                                 SmtpUser = emailConfig.Email,
                                 DisplayName = emailConfig.UserName,
                             }
                         });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return Ok(retVal);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Chi tiết lập lịch
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Detail")]
    public async Task<ActionResult> Detail(string id)
    {
        return Ok();
    }


    /// <summary>
    ///  Xóa vĩnh viễn
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<ActionResult> Delete([FromBody] SendEmailDeteleRequest request)
    {
        var result = await _sendEmailRepository.DeleteAsync(request.Id);
        return Ok(result);
    }
}
