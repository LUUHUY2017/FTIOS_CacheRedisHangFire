using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.Models;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations.Requests;
using AutoMapper;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Application.Services.VTSmart;
using Server.Core.Entities.A2;
using Server.Infrastructure.Datas.MasterData;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.APIControllers.Students.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class StudentController : AuthBaseAPIController
{
    private readonly IMapper _mapper;
    private readonly MasterDataDbContext _masterDb;
    public StudentController(IMapper mapper, MasterDataDbContext masterDb)
    {
        _mapper = mapper;
        _masterDb = masterDb;
    }

    /// <summary>
    /// Lấy danh sách học sinh từ SMAS
    /// </summary>
    /// <returns></returns> 
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets(string schoolCode = "20186511")
    {
        var data = await SmartService.GetStudents(schoolCode);
        return Ok(data);
    }
    /// <summary>
    /// Tải danh sách học sinh từ SMAS xuống AMMS
    /// </summary>
    /// <param name="schoolCode"></param>
    /// <returns></returns>
    [HttpGet("Download")]
    public async Task<IActionResult> Download(string schoolCode = "20186511")
    {
        var datas = await SmartService.GetStudents(schoolCode);

        if (datas.isSuccess)
        {
            var students = await _masterDb.Student.Where(o => o.SchoolCode == schoolCode).ToListAsync();
            if (students == null)
                students = new List<Server.Core.Entities.A2.Student>();

            foreach (var s in datas.responses)
            {
                if (!students.Any(o => o.StudentCode == s.studentCode))
                {
                    var st = new Student()
                    {
                        StudentCode = s.studentCode,
                        Name = s.studentName,
                        Actived = true,
                        ClassName = s.className,
                        DateOfBirth = s.birthDay,
                        SchoolCode = schoolCode,
                        ClassId = s.classId,

                    };
                    _masterDb.Student.Add(st);
                }
            }
            await _masterDb.SaveChangesAsync();
        }
        return Ok(datas);
    }

}

