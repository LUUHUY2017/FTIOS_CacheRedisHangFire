using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Application.MasterDatas.A2.Lanes.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Server.Core.Interfaces.A2.Lanes.Requests;
using Share.Core.Pagination;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.Lanes.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    //[Authorize("Bearer")]
    //[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
    public class LaneController : AuthBaseAPIController
    {
        private readonly ILaneRepository _laneRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public LaneController(ILaneRepository laneRepository, IMapper mapper, IUriService uriService)
        {
            _laneRepository = laneRepository;
            _mapper = mapper;
            _uriService = uriService;
        }


        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Gets")]
        public async Task<IActionResult> Gets()
        {
            try
            {
                var data = await _laneRepository.GetAll();
                return Ok(new Result<List<A2_Lane>>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<List<A2_Lane>>(null, "Lỗi:" + ex.Message, false));
            }
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post(LaneFilterRequest request)
        {
            try
            {
                var data = await _laneRepository.Gets(request);
                return Ok(new Result<List<A2_Lane>>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<List<A2_Lane>>(null, "Lỗi:" + ex.Message, false));
            }
        }


        /// <summary>
        /// Cập nhật dữ liệu 
        /// </summary>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(LaneRequest model)
        {
            var request = _mapper.Map<A2_Lane>(model);
            var relval = await _laneRepository.UpdateAsync(request);
            return Ok(relval);
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
