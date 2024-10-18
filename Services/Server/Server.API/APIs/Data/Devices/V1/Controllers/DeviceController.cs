using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Devices;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Share.Core.Pagination;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.Devices.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    //[Authorize("Bearer")]
    //[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
    public class DeviceController : AuthBaseAPIController
    {
        private readonly DeviceService _deviceService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public DeviceController(DeviceService deviceService, IMapper mapper, IUriService uriService)
        {
            _deviceService = deviceService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Lấy danh sách đang hoạt động
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Gets")]
        public async Task<IActionResult> Gets()
        {
            try
            {
                var data = await _deviceService.GetAll();
                return Ok(new Result<List<A2_Device>>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<TotalDevice>(null, "Lỗi:" + ex.Message, false));
            }
        }

        /// <summary>
        /// Lấy tổng quan thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post(DeviceFilterRequest request)
        {
            try
            {
                var data = await _deviceService.GetDevices(request);
                return Ok(new Result<List<DeviceResponse>>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<TotalDevice>(null, "Lỗi:" + ex.Message, false));
            }
        }


        /// <summary>
        /// Cập nhật dữ liệu thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(DeviceRequest model)
        {
            return Ok(await _deviceService.Update(model));
        }

        /// <summary>
        /// Xóa thiết bị
        /// </summary>
        /// <returns></returns>
        //[HttpPost("Delete")]
        //public async Task<IActionResult> Delete(DeleteRequest request)
        //{
        //    //return Ok(await _deviceService.Delete(request.Id));
        //}
    }
}
