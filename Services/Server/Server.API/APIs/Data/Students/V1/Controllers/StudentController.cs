using AMMS.Share.WebApp.Helps;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Authorize("Bearer")]
    [AuthorizeMaster]
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
            var data = await _studentService.SaveFromWeb(request);
            return Ok(data);
        }

        /// <summary>
        /// Danh sách học sinh đã đồng bộ
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Post")]
        public async Task<IActionResult> Post(StudentSearchRequest request)
        {
            try
            {
                request.OrganizationId = GetOrganizationId();
                var items = await _studentService.GetAlls(request);
                if (request.FilterItems != null && request.FilterItems.Count > 0)
                {
                    foreach (var filter in request.FilterItems)
                    {
                        items = await _studentService.ApplyFilter(items, filter);
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


        /// <summary>
        /// Post
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("PostGeneral")]
        public async Task<ActionResult> PostGeneral(StudentSearchRequest request)
        {
            try
            {
                request.OrganizationId = GetOrganizationId();
                var items = await _studentService.GetAlls(request);
                int totalAmount = await items.CountAsync();
                int totalFace = await items.CountAsync(o => o.IsFace == true);
                int totalCurrent = totalAmount - totalFace;

                //#region Type
                //var infos = items.GroupBy(o => o.OrganizationId).Select(g => new ObjectDataDashboard()
                //{
                //    Name = g.Key ?? "N/A",
                //    Value = g.Count(o => o.IsFace == true),
                //    Percent = g.Count(o => o.Actived == true),
                //}).OrderBy(o => o.Name).ToList();
                //#endregion

                var retVal = new
                {
                    totalAmount = totalAmount,
                    totalFace = totalFace,
                    totalCurrent = totalCurrent,
                };
                return Ok(new Result<object>(retVal, "Thành công", true));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
