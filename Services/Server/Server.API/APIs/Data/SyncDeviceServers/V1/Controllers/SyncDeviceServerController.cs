using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
using Server.Application.Services.VTSmart;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.Students;
using Server.Core.Interfaces.A2.SyncDeviceServers.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.SyncDeviceServers.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]

    [Authorize("Bearer")]
    //[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
    public class SyncDeviceServerController : AuthBaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly SmartService _smartService;
        private readonly IStudentRepository _studentRepository;

        private readonly StudentService _studentService;
        private readonly SyncDeviceServerService _syncDeviceService;
        private readonly IPersonRepository _personRepository;


        public SyncDeviceServerController(
            SyncDeviceServerService syncDeviceService,
            StudentService studentService, IMapper mapper,
            IPersonRepository personRepository,
            IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _personRepository = personRepository;
            _studentRepository = studentRepository;

            _syncDeviceService = syncDeviceService;
            _studentService = studentService;
        }


        /// <summary>
        /// Post Student
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Post")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(SyncDeviceServerFilterReq request)
        {
            try
            {
                var datas = await _syncDeviceService.GetAlls(request);
                return Ok(datas);

            }
            catch (Exception ex)
            {
                return Ok(new Result<object>(null, "Lỗi:" + ex.Message, false));
            }
        }


        [HttpPost("PostSyncItem")]
        [AllowAnonymous]
        public async Task<IActionResult> PostSyncItem(SyncStudentDeviceReq request)
        {
            try
            {
                string imgSrc = "";
                var retval = await _studentRepository.GetByIdAsync(request.PersonId);
                var resImg = await _personRepository.GetFacePersonById(request.PersonId);

                if (!retval.Succeeded)
                    return Ok(new Result<object>("Không tìm thấy học sinh", false));

                if (resImg.Succeeded)
                {
                    imgSrc = resImg.Data.FaceData;
                }

                Student student = retval.Data;
                student.ImageSrc = imgSrc;

                var datas = await _studentService.PushPersonByEventBusAsync(student);
                return Ok(datas);
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>("Lỗi:" + ex.Message, false));
            }
        }

    }
}
