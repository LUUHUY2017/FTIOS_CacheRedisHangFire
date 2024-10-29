using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
using Server.Application.Services.VTSmart;
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

    //[Authorize("Bearer")]
    //[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
    public class SyncDeviceServerController : AuthBaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly SmartService _smartService;
        private readonly IPersonRepository _personRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly SyncDeviceServerService _syncDeviceService;


        public SyncDeviceServerController(SyncDeviceServerService syncDeviceService, IMapper mapper, IPersonRepository personRepository,
   IStudentRepository studentRepository)
        {
            _mapper = mapper;

            _personRepository = personRepository;
            _studentRepository = studentRepository;
            _syncDeviceService = syncDeviceService;
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



    }
}
