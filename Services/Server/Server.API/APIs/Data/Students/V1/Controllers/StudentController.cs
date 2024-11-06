using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Application.Services.VTSmart;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.Students;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.StudentSmas.V1.Controllers
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
            request.OrganizationId = GetOrganizationId();
            var data = await _studentService.SaveFromWeb(request);
            return Ok(data);
        }

        /// <summary>
        /// Danh sách học sinh đã đồng bộ
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Post")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(StudentSearchRequest request)
        {
            try
            {
                var students = await _studentService.GetAlls(request);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>(null, "Lỗi:" + ex.Message, false));
            }
        }

    }
}
