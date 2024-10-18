using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.API.APIs.Data.Users.V1.Models;
using Server.Core.Identity.Entities;
using Server.Core.Identity.Interfaces.Accounts.Requests;
using Server.Core.Identity.Interfaces.Accounts.Services;
using Server.Infrastructure.Datas.MasterData;
using Server.Infrastructure.Identity;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.Users.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    //[Authorize("Bearer")]
    //[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]

    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MasterDataDbContext _context;
        private readonly IdentityContext _identityContext;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private readonly IServiceProvider _serviceProvider;
        public UserController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            MasterDataDbContext context,
            IdentityContext identityContext,
            IConfiguration configuration,
            IMapper mapper,
            IMediator mediator,
            IServiceProvider serviceProvider
            )
        {
            _accountService = accountService;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _identityContext = identityContext;
            _mediator = mediator;
            _mapper = mapper;

            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Danh sách các tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            //var user = await _userManager.GetUserAsync(User);
            //if (user != null)
            //{
            //    var userRoles = await _userManager.GetRolesAsync(user);
            //}
            //var claims = User.Claims.ToList();

            var users = _userManager.Users;
            var uservms = await users.Select(o => o).ToListAsync();
            return Ok(uservms);
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> Detail(string userId)
        {
            //Lấy danh sách roleGroups
            //var roleGroups = _context.RoleGroup.ToList();
            //var roleGroups_selected = (from rg in _context.RoleGroup join rgu in _context.RoleGroupUser on rg.Id equals rgu.RoleGroupId where rgu.UserId == userId select rg).ToList();
            //var roleGroups_unselected = roleGroups.Where(o => roleGroups_selected.All(x => x.Id != o.Id)).ToList();

            var user = await _userManager.FindByIdAsync(userId);
            //Lấy danh sách roles
            var user_roles = await _userManager.GetRolesAsync(user);
            var user_roles_selected = new List<IdentityRole>();
            user_roles_selected = user_roles?.Select(o => new IdentityRole() { Name = o }).ToList();
            var roles = _identityContext.Roles.ToList();
            var user_roles_unselected = roles.Where(o => user_roles_selected.All(x => x.Name != o.Name)).ToList();


            return Ok(new Result<object>
            {
                Code = 0,
                Data = new
                {
                    //data_roleGroups_unselect = roleGroups_unselected,
                    //data_roleGroups_select = roleGroups_selected,
                    user_roles_unselected = user_roles_unselected,
                    user_roles_selected = user_roles_selected,
                },
                Message = $"Success!",
                Succeeded = true,
            });
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {

                //Kiểm tra tài khoản đăng nhập
                var user_userName = await _userManager.FindByNameAsync(model.UserName);
                if (user_userName != null)
                    return Ok(new Result<ApplicationUser>
                    {
                        Code = 1,

                        Message = $"Tên đăng nhập đã tồn tại!",
                        Succeeded = false,
                    });

                //Kiểm tra email
                var user_email = await _userManager.FindByEmailAsync(model.Email);
                if (user_email != null)
                    return Ok(new Result<ApplicationUser>
                    {
                        Code = 1,

                        Message = $"Email đã tồn tại!",
                        Succeeded = false,
                    });

                //Kiểm tra phone
                //var user_phone = await _userManager.FindByLoginAsync(model.Email, "");
                //if (user_email != null)
                //    return Ok(new Result<ApplicationUser>
                //    {
                //        Code = 1,
                //        
                //        Message = $"Email đã tồn tại!",
                //        Succeeded = false,
                //    });

                var user = new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var xx = await _userManager.CreateAsync(user, model.Password);
                if (xx.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _userManager.AddToRoleAsync(user, "SuperAdmin");
                    return Ok(new Result<ApplicationUser>
                    {
                        Code = 0,
                        Data = user,
                        Message = $"Success!",
                        Succeeded = true,
                    });
                }
                return Ok(new Result<ApplicationUser>
                {
                    Code = 0,
                    Message = xx.Errors.FirstOrDefault().Description,
                    Succeeded = false,
                });

            }
            else
            {
                var curent_user = await _userManager.FindByIdAsync(model.Id);
                if (curent_user == null)
                    return Ok(new Result<ApplicationUser>
                    {
                        Code = 1,

                        Message = $"Không tìm thấy dữ liệu!",
                        Succeeded = false,
                    });
                //Kiểm tra tài khoản đăng nhập
                var user_userName = await _userManager.FindByNameAsync(model.UserName);
                if (user_userName != null && user_userName.Id != model.Id)
                    return Ok(new Result<ApplicationUser>
                    {
                        Code = 1,

                        Message = $"Tên đăng nhập đã tồn tại!",
                        Succeeded = false,
                    });
                //Kiểm tra email
                try
                {
                    var user_email = await _userManager.FindByEmailAsync(model.Email);
                    if (user_email != null && user_email.Id != model.Id)
                        return Ok(new Result<ApplicationUser>
                        {
                            Code = 1,

                            Message = $"Email đã tồn tại!",
                            Succeeded = false,
                        });
                }
                catch (Exception e)
                {
                    return Ok(new Result<ApplicationUser>
                    {
                        Code = 1,
                        Message = $"Có lỗi: {e.Message}",
                        Succeeded = false,
                    });
                }

                curent_user.FirstName = model.FirstName;
                curent_user.UserName = model.UserName;
                curent_user.Email = model.Email;
                curent_user.PhoneNumber = model.PhoneNumber;


                var user = await _userManager.UpdateAsync(curent_user);

                if (user != null)
                {
                    return Ok(new Result<List<ApplicationUser>>
                    {
                        Code = 0,
                        Data = _userManager.Users.ToList(),
                        Message = $"Success!",
                        Succeeded = true,
                    });
                }
                else
                {
                    return Ok(new Result<List<ApplicationUser>>
                    {
                        Code = 1,

                        Message = $"Không tìm thấy dữ liệu!",
                        Succeeded = false,
                    });
                }

            }
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(string id, string reasonDelete)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                //var roleGroupUsers = _context.RoleGroupUser.Where(o => o.UserId == id).ToList();
                //if (roleGroupUsers != null && roleGroupUsers.Count() > 0)
                //{
                //    _context.RoleGroupUser.RemoveRange(roleGroupUsers);
                //    _context.SaveChanges();
                //}
                await _userManager.DeleteAsync(user);

                return Ok(new Result<List<ApplicationUser>>
                {
                    Code = 0,
                    Data = _userManager.Users.ToList(),
                    Message = $"Success!",
                    Succeeded = true,
                });
            }
            return Ok(new Result<ApplicationUser>
            {
                Code = 0,

                Message = $"Không tìm thấy dữ liệu!",
                Succeeded = false,
            });
        }

        [AllowAnonymous]
        [HttpGet("GetAccountSystems")]
        public async Task<IActionResult> GetAccountSystems()
        {
            var accounts = await _accountService.GetAccountSystems();

            return Ok(new Result<List<UserAccountRes>>
            {
                Code = 0,
                Data = accounts,
                Message = $"Success!",
                Succeeded = true,
            });
        }
    }
}
