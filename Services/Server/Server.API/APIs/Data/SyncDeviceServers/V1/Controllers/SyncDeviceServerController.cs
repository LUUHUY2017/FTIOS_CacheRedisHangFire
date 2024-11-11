using AMMS.Share.WebApp.Helps;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [AuthorizeMaster]
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
        public async Task<IActionResult> Post(SyncDeviceServerFilterReq request)
        {
            try
            {
                request.OrganizationId = GetOrganizationId();
                var items = await _syncDeviceService.GetAlls(request);
                if (request.FilterItems != null && request.FilterItems.Count > 0)
                {
                    foreach (var filter in request.FilterItems)
                    {
                        items = await _syncDeviceService.ApplyFilter(items, filter);
                    }
                }


                int totalRow = await items.CountAsync();
                // phân trang
                int skip = (request.CurentPage.Value - 1) * (request.RowsPerPage.Value);
                int totalPage = 0;
                totalPage = totalRow / (request.RowsPerPage.Value);
                if (totalRow % (request.RowsPerPage.Value) > 0)
                    totalPage++;

                var datas = await items.Skip(skip).Take(request.RowsPerPage.Value).ToListAsync();
                int totalDataRow = datas.Count();

                var retVal = new
                {
                    items = datas,

                    totalPage = totalPage,
                    totalRow = totalRow,
                    totalDataRow = totalDataRow,

                    rowPerPage = request.RowsPerPage,
                    curentPage = request.CurentPage,
                };
                return Ok(new Result<object>(retVal, "Thành công!", true));

            }
            catch (Exception ex)
            {
                return Ok(new Result<object>(null, "Lỗi:" + ex.Message, false));
            }
        }


        [HttpPost("PostSyncItem")]
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
                    imgSrc = resImg.Data.FaceData;


                Student student = retval.Data;
                student.ImageSrc = imgSrc;
                var datas = await _studentService.PushStudentsByEventBusAsync(student);
                return Ok(datas);
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>("Lỗi:" + ex.Message, false));
            }
        }

    }
}
