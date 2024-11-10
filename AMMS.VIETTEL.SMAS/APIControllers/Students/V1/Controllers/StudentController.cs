using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Share.WebApp.Controllers;

namespace AMMS.VIETTEL.SMAS.APIControllers.Students.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class StudentController : AuthBaseAPIController
{
    private readonly IMapper _mapper;
    private readonly ViettelDbContext _masterDb;
    public StudentController(IMapper mapper, ViettelDbContext masterDb)
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
                students = new List<Student>();

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

