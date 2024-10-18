using AMMS.Share.WebApp.Helps;
using AutoMapper;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.Users.V1.Models;
using Server.Core.Entities.A0;
using Server.Core.Identity.Entities;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Identity;

namespace Server.API.APIs.Data.Users.V1.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize("Bearer")]
    [AuthorizeMaster(Roles = RoleConst.AdminPage)]

    public class UserProfileController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MasterDataDbContext _masterDbContext;

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailSender _emailSender;
        private readonly IPersonRepository _personRepository;


        public UserProfileController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            MasterDataDbContext masterDbContext,
            IConfiguration configuration,
            IMapper mapper,
            IMediator mediator,
            IServiceProvider serviceProvider,
            IEmailSender emailSender,
            IPersonRepository personRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _masterDbContext = masterDbContext;
            _mediator = mediator;
            _mapper = mapper;

            _serviceProvider = serviceProvider;
            _personRepository = personRepository;
            _emailSender = emailSender;
        }


        /// <summary>
        /// Thông tin đăng nhập tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new Result<ApplicationUser>
                {
                    Code = 1,
                    Message = $"False!",
                    Succeeded = false,
                });
            }
            var userVm = new ApplicationUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return Ok(new Result<ApplicationUser>
            {
                Code = 0,
                Data = userVm,
                Message = $"Success!",
                Succeeded = true,
            });
        }
        /// <summary>
        /// Thông tin đăng nhập tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(new Result<ApplicationUser>
                {
                    Code = 1,
                    Message = $"False!",
                    Succeeded = false,
                });
            }
            return Ok(new Result<ApplicationUser>
            {
                Code = 0,
                Data = user,
                Message = $"Success!",
                Succeeded = true,
            });
        }

        /// <summary>
        /// Danh sách quyền của tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var _RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _UserManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            //IdentityResult adminRoleResult;
            //bool adminRoleExists = await _RoleManager.RoleExistsAsync("Admin");
            //if (!adminRoleExists)
            //{
            //    adminRoleResult = await _RoleManager.CreateAsync(new IdentityRole("Admin"));
            //}

            //bool superAdminExists = await _RoleManager.RoleExistsAsync("SuperAdmin");
            //if (!superAdminExists)
            //{
            //    await _RoleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            //} 
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);


                //ApplicationUser userToMakeAdmin = await _UserManager.FindByNameAsync(user.Email);


                //if (user.Email == "quyet@acs.vn")
                //{
                //    if (userRoles.FirstOrDefault(o => o == "Admin") == null)
                //        await _UserManager.AddToRoleAsync(userToMakeAdmin, "Admin");
                //}
                //if (user.Email == "nguyencongquyet@gmail.com")
                //{
                //    if (userRoles.FirstOrDefault(o => o == "SuperAdmin") == null)
                //        await _UserManager.AddToRoleAsync(userToMakeAdmin, "SuperAdmin");

                //    if (userRoles.FirstOrDefault(o => o == "Admin") == null)
                //        await _UserManager.AddToRoleAsync(userToMakeAdmin, "Admin");
                //}


                return Ok(new Result<List<string>>
                {
                    Data = userRoles.ToList(),
                });
            }
            return BadRequest();
        }

        /// <summary>
        /// Cập nhật thông tin Person của tài khoản đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfoAsync(UserInfoModel model)
        {
            try
            {
                A0_Person request = new A0_Person();
                request.Note = model.Description;
                request.FirstName = model.FirstName;
                request.LastName = model.LastName;
                request.CitizenId = model.CitizenId;
                request.Id = User.GetSubjectId();

                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    await _userManager.UpdateAsync(user);
                }
                _personRepository.UpdateAsync(request);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        ///// <summary>
        ///// Lấy Avata của user đã đăng nhập
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("GetAvata")]
        //public async Task<IActionResult> GetAvata()
        //{
        //    var person = await _masterDbContext.Person.FirstOrDefaultAsync(o => o.Id == User.GetSubjectId());
        //    string avata = "";
        //    if (person != null)
        //    {
        //        string fileName = FollderCommon.AvataFolder + person.PersonImage;
        //        //avata = FollderCommon.ImageFileToBase64(fileName);
        //        if (System.IO.File.Exists(fileName))
        //            avata = $"{Request.Scheme}://{Request.Host.Value}/Datas/Avatas/{person.PersonImage}";
        //    }

        //    if (avata != "")
        //        return Ok(new Result<string>
        //        {
        //            Code = 0,
        //            Data = avata,
        //            Message = $"Success!",
        //            Succeeded = true,
        //        });

        //    return Ok(new Result<string>
        //    {
        //        Code = 0,
        //        Data = avata,
        //        Message = $"Không tìm thấy avata!",
        //        Succeeded = false,
        //    });
        //}

        ///// <summary>
        ///// Thiết lập Avata của user đã đăng nhập
        ///// </summary>
        ///// <param name="avata"></param>
        ///// <returns></returns>
        //[HttpPost("SetAvata")]
        //public async Task<IActionResult> SetAvata([FromBody] AvataModel model)
        //{
        //    try
        //    {
        //        //Save Image
        //        Image img = ImageCommon.Base64ToImage(model.ImageBase64);


        //        var imgNameArr = model.ImageName.Split(".");
        //        var imageFileName = $"{User.GetSubjectId()}.jpg";

        //        string fileName = FollderCommon.AvataFolder + imageFileName;

        //        if (System.IO.File.Exists(fileName))
        //            System.IO.File.Delete(fileName);
        //        img.Save(fileName);

        //        var user = await _userManager.GetUserAsync(User);


        //        var person = await _masterDbContext.Person.FirstOrDefaultAsync(o => o.Id == User.GetSubjectId());
        //        if (person == null)
        //        {
        //            person = new Core.MasterData.Persons.Entities.Person()
        //            {
        //                PersonImage = imageFileName,
        //                Id = User.GetSubjectId(),
        //                Actived = true,
        //                CreatedDate = DateTime.Now,
        //                FirstName = user.FirstName,
        //                LastName = user.LastName,
        //                PhoneNUmber = user.PhoneNumber,
        //                Email = user.Email,
        //            };
        //            await _masterDbContext.AddAsync(person);
        //        }
        //        else
        //        {
        //            string fileName1 = FollderCommon.AvataFolder + person.PersonImage;
        //            if (System.IO.File.Exists(fileName1))
        //                System.IO.File.Delete(fileName1);

        //            person.PersonImage = imageFileName;
        //        }
        //        await _masterDbContext.SaveChangesAsync();

        //        return Ok(new Result<string>
        //        {
        //            Code = 0,
        //            Data = $"{Request.Scheme}://{Request.Host.Value}/Datas/Avatas/{person.PersonImage}",
        //            Message = $"Thành công!",
        //            Succeeded = true,
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new Result<string>
        //        {
        //            Code = 0,
        //            Message = $"Lỗi: {e.Message} ",
        //            Succeeded = false,
        //        });
        //    }

        //}

        /// <summary>
        /// Đổi mật khẩu của tài khoản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel input)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(User.GetSubjectId());
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return Ok(new Result<object>("Có lỗi khi đặt mật khẩu.", false));
                }
                bool checkPassword = await _userManager.CheckPasswordAsync(user, input.CurentPassword);
                if (!checkPassword)
                    return Ok(new Result<object>("Mật khẩu hiện tại không đúng.", false));

                var result = await _userManager.ChangePasswordAsync(user, input.CurentPassword, input.Password);
                if (result.Succeeded)
                {
                    return Ok(new Result<object>("Đặt lại mật khẩu thành công", true));
                }

                List<string> errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }

                return Ok(new Result<string>("Có lỗi khi đặt lại mật khẩu. Xem chi tiết lỗi", false)
                {
                    Errors = errors
                });
            }
            catch (Exception ex)
            {
                return Ok(new Result<string>($"Có lỗi khi khôi phục mật khẩu {ex.Message}", false));
            }
        }

    }
}
