using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.Students.V1.Requests;
using Server.Application.MasterDatas.A2.Devices;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Entities.A2;
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
        private readonly DeviceService _deviceService;
        private readonly IMapper _mapper;
        private readonly SyncDataSmartService _service;


        public StudentController(DeviceService deviceService, IMapper mapper, SyncDataSmartService service)
        {
            _deviceService = deviceService;
            _mapper = mapper;
            _service = service;
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
        /// Post Student
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post(StudentSearchRequest request)
        {
            try
            {
                List<StudentResponse> students = new List<StudentResponse>();

                if (ModelState.IsValid)
                {
                    students = await _service.PostStudents(request.ClassId, request.SchoolyearId);
                }
                var data = new
                {
                    students = students,
                };
                return Ok(new Result<object>(data, "Thành công!", true));
            }
            catch (Exception ex)
            {
                return Ok(new Result<TotalDevice>(null, "Lỗi:" + ex.Message, false));
            }
        }

        /// <summary>
        /// Post School
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostSchool")]
        public async Task<IActionResult> PostSchool()
        {
            try
            {
                List<SchoolLevel> schools = new List<SchoolLevel>();
                var schoolsRaw = await _service.PostSchool();
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
                return Ok(new Result<TotalDevice>(null, "Lỗi:" + ex.Message, false));
            }
        }

        /// <summary>
        /// Post SchoolYears 
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostSchoolYears")]
        public async Task<IActionResult> PostSchoolYears()
        {
            try
            {
                var schoolYears = await _service.PostSchoolYears();
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
        [HttpPost("PostClass")]
        public async Task<IActionResult> PostClass(ClassSearchRequest request)
        {
            try
            {
                List<ClassResponse> classes = new List<ClassResponse>();
                if (ModelState.IsValid)
                {
                    var classesRaw = await _service.PostClass(request.SchoolLevelCode, request.SchoolYearId, request.Schoolyear);
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
