using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.Students.V1.Requests;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.Students;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.Students.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    //[Authorize("Bearer")]
    //[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
    public class StudentController : AuthBaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly SmartService _smartService;
        private readonly StudentService _studentService;
        private readonly IPersonRepository _personRepository;
        private readonly IStudentRepository _studentRepository;


        public StudentController(StudentService studentService, IMapper mapper, SmartService smartService, IPersonRepository personRepository,
   IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _smartService = smartService;
            _studentService = studentService;

            _personRepository = personRepository;
            _studentRepository = studentRepository;
        }



        /// <summary>
        /// Cập nhật thông tin Học sinh
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(DtoStudentRequest request)
        {
            var data = await _studentService.Save(request);
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
                List<StudentResponse> students = new List<StudentResponse>();

                if (ModelState.IsValid)
                {
                    students = await _smartService.PostStudents(request.ClassId, request.SchoolyearId);
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
                var schoolsRaw = await _smartService.PostSchool();
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
                var schoolYears = await _smartService.PostSchoolYears();
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
                    var classesRaw = await _smartService.PostClass(request.SchoolLevelCode, request.SchoolYearId, request.Schoolyear);
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
