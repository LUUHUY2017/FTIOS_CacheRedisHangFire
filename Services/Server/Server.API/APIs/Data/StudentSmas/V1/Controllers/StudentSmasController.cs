using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.StudentSmas.V1.Requests;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.StudentSmas.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    //[Authorize("Bearer")]
    //[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
    public class StudentSmasController : AuthBaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly SmartService _smartService;
        private readonly StudentService _studentService;


        public StudentSmasController(StudentService studentService, IMapper mapper, SmartService smartService)
        {
            _mapper = mapper;
            _smartService = smartService;
            _studentService = studentService;
        }



        /// <summary>
        /// Cập nhật thông tin Học sinh
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(DtoStudentRequest request)
        {
            request.OrganizationId = GetOrganizationId();
            var data = await _studentService.SaveFromWeb(request);
            return Ok(data);
        }


        /// <summary>
        /// Post Student
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Post")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(StudentSearchRequest request)
        {
            try
            {
                List<StudentSmasResponse> students = new List<StudentSmasResponse>();

                if (ModelState.IsValid)
                {
                    //string key = "r0QQKLBa3x9KN/8el8Q/HQ==";
                    //string keyIV = "8bCNmt1+RHBNkXRx8MlKDA==";
                    //string secretKey = "Smas!@#2023";
                    //string _secretKey = _smartService.GetSecretKeyVMSAS(secretKey, key, keyIV, "20186511");

                    students = await _smartService.PostStudents(request.ClassId, request.SchoolyearId, GetOrganizationId());
                }
                var data = new
                {
                    students = students,
                };
                return Ok(new Result<object>(data, "Thành công!", true));

            }
            catch (Exception ex)
            {
                return Ok(new Result<object>(null, "Lỗi:" + ex.Message, false));
            }
        }

        /// <summary>
        /// Post School
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost("PostSchool")]
        public async Task<IActionResult> PostSchool()
        {
            try
            {
                List<SchoolLevel> schools = new List<SchoolLevel>();
                var schoolsRaw = await _smartService.PostSchool(GetOrganizationId());
                if (schoolsRaw != null)
                {
                    foreach (var item in schoolsRaw.SchoolLevels)
                    {
                        schools.Add(item);
                    }
                }
                var data = new
                {
                    schools = schools,
                };

                return Ok(new Result<object>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>(null, "Lỗi:" + ex.Message, false));
            }
        }

        /// <summary>
        /// Post SchoolYears 
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost("PostSchoolYears")]
        public async Task<IActionResult> PostSchoolYears()
        {
            try
            {
                var schoolYears = await _smartService.PostSchoolYears(GetOrganizationId());
                var data = new
                {
                    schoolYears = schoolYears,
                };
                return Ok(new Result<object>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>("Lỗi:" + ex.Message, false));
            }
        }


        /// <summary>
        /// Post Class
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost("PostClass")]
        public async Task<IActionResult> PostClass(ClassSearchRequest request)
        {
            try
            {
                List<ClassResponse> classes = new List<ClassResponse>();
                if (ModelState.IsValid)
                {
                    var classesRaw = await _smartService.PostClass(request.SchoolLevelCode, request.SchoolYearId, request.Schoolyear, GetOrganizationId());
                    if (classesRaw.Any())
                    {
                        foreach (var group in classesRaw)
                        {
                            foreach (var item in group.Classes)
                            {
                                classes.Add(item);
                            }
                        }
                    }
                }


                var data = new
                {
                    classes = classes,
                };
                return Ok(new Result<object>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>("Lỗi:" + ex.Message, false));
            }
        }



    }
}
