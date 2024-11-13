using AMMS.Share.WebApp.Helps;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A2.Devices;
using Server.Application.MasterDatas.A2.Students.V1;
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

    [Authorize("Bearer")]
    [AuthorizeMaster]
    public class SyncDeviceServerController : AuthBaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly SmartService _smartService;
        private readonly IStudentRepository _studentRepository;

        private readonly StudentService _studentService;
        private readonly SyncDeviceServerService _syncDeviceService;
        private readonly DeviceAdminService _deviceAdminService;
        private readonly IPersonRepository _personRepository;


        public SyncDeviceServerController(
            SyncDeviceServerService syncDeviceService,
            DeviceAdminService deviceAdminService,
            StudentService studentService, IMapper mapper,
            IPersonRepository personRepository,
            IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _personRepository = personRepository;
            _studentRepository = studentRepository;

            _syncDeviceService = syncDeviceService;
            _deviceAdminService = deviceAdminService;
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


        [HttpPost("PostGeneral")]
        public async Task<IActionResult> PostGeneral(SyncDeviceServerFilterReq request)
        {
            try
            {
                request.OrganizationId = GetOrganizationId();
                request.StartDate = null;
                request.EndDate = null;
                var items = await _syncDeviceService.GetAlls(request);

                if (request.FilterItems != null && request.FilterItems.Count > 0)
                {
                    foreach (var filter in request.FilterItems)
                    {
                        items = await _syncDeviceService.ApplyFilter(items, filter);
                    }
                }

                int totalAmount = await items.CountAsync();
                int totalFace = await items.CountAsync(o => o.SynStatus == true);
                int totalWait = await items.CountAsync(o => o.SynStatus == null);
                int totalCurrent = totalAmount - totalWait - totalFace;



                var retVal = new
                {
                    totalAmount = totalAmount,
                    totalFace = totalFace,
                    totalWait = totalWait,
                    totalCurrent = totalCurrent,
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
                var retval1 = await _studentRepository.GetByIdAsync(request.PersonId);
                if (!retval1.Succeeded)
                    return Ok(new Result<object>("Không tìm thấy học sinh", false));

                var retval2 = await _deviceAdminService.GetByIdAsync(request.DeviceId);
                if (!retval2.Succeeded)
                    return Ok(new Result<object>("Không tìm thấy thiết bị", false));

                var datas = await _studentService.PushStudentByEventBusAsync(retval2.Data, retval1.Data);

                return Ok(datas);
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>("Lỗi:" + ex.Message, false));
            }
        }

        [HttpGet("GetFaceByPersonId")]
        public async Task<IActionResult> GetFaceByPersonId(string personId)
        {
            try
            {
                var datas = await _studentService.GetFaceByPersonId(personId);
                return Ok(datas);
            }
            catch (Exception ex)
            {
                return Ok(new Result<object>("Lỗi:" + ex.Message, false));
            }
        }
    }
}
