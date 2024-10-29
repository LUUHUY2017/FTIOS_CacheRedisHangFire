using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.API.Areas.Managers.Users.V1.Models;
using Server.Core.Entities.A0;
using Server.Core.Identity.Entities;
using Server.Core.Identity.Interfaces.Accounts.Requests;
using Server.Core.Identity.Interfaces.Accounts.Services;
using Server.Core.Interfaces.A2.Persons;
using Server.Infrastructure.Datas.MasterData;
using Server.Infrastructure.Identity;
using Shared.Core.Commons;

namespace Server.API.Areas.Managers.Users.V1.Controllers;

[Route("v1/[controller]")]
//[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class UserController : Controller
{
    const string pathUrl = "~/Areas/Managers/Users/V1/Views/";
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MasterDataDbContext _context;
    private readonly IdentityContext _identityContext;
    private readonly IPersonRepository _personRepository;

    private readonly IAccountService _accountService;
    public UserController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, MasterDataDbContext context, IdentityContext identityContext
        , IPersonRepository personRepository
        , IAccountService accountService
        )
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
        _identityContext = identityContext;
        _personRepository = personRepository;
        _accountService = accountService;
    }


    [HttpGet]
    public IActionResult Index(string tab = "system")
    {
        ViewBag.Tab = tab;
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }

    [HttpGet("Gets")]
    public async Task<IActionResult> Gets(string tab = "system")
    {
        var all_accounts = _userManager.Users.ToList();

        var accounts = new List<UserAccountRes>();
        if (tab == "system")
        {
            accounts = await _accountService.GetAccountSystems();
        }

        return Ok(new Result<List<UserAccountRes>>
        {
            Code = 0,
            Data = accounts,// _userManager.Users.ToList(),
            Message = $"Success!",
            Succeeded = true,
        });
    }
    [HttpGet("Delete")]
    public async Task<IActionResult> Delete(string id, string reasonDelete)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            var roleGroupUsers = _context.A0_RoleGroupUser.Where(o => o.UserId == id).ToList();
            if (roleGroupUsers != null && roleGroupUsers.Count() > 0)
            {
                _context.A0_RoleGroupUser.RemoveRange(roleGroupUsers);
                _context.SaveChanges();
            }
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
                PhoneNumber = model.PhoneNumber,
                Type = model.Type,
            };

            var xx = await _userManager.CreateAsync(user, model.Password);
            if (xx.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
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

            }

            curent_user.FirstName = model.FirstName;
            curent_user.UserName = model.UserName;
            curent_user.Email = model.Email;
            curent_user.PhoneNumber = model.PhoneNumber;
            curent_user.Type = model.Type;


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

    [HttpGet("Detail")]
    public async Task<IActionResult> Detail(string userId)
    {
        //Lấy danh sách roleGroups
        var roleGroups = _context.A0_RoleGroup.ToList();
        var roleGroups_selected = (from rg in _context.A0_RoleGroup join rgu in _context.A0_RoleGroupUser on rg.Id equals rgu.RoleGroupId where rgu.UserId == userId select rg).ToList();
        var roleGroups_unselected = roleGroups.Where(o => roleGroups_selected.All(x => x.Id != o.Id)).ToList();

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
                data_roleGroups_unselect = roleGroups_unselected,
                data_roleGroups_select = roleGroups_selected,
                user_roles_unselected = user_roles_unselected,
                user_roles_selected = user_roles_selected,
            },
            Message = $"Success!",
            Succeeded = true,
        });
    }


    #region Use
    [HttpGet("AddRoleGroup")]
    public async Task<IActionResult> AddRoleGroup(string userId, string roleGroupId)
    {
        var roleGroupUser = _context.A0_RoleGroupUser.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.UserId == userId);
        if (roleGroupUser == null)
        {
            roleGroupUser = new A0_RoleGroupUser()
            {
                Actived = true,
                UserId = userId,
                RoleGroupId = roleGroupId,
            };
            _context.A0_RoleGroupUser.Add(roleGroupUser);
            await _context.SaveChangesAsync();


            //Add Role To User
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = _roleManager.Roles.ToList();
                var roleGorups = _context.A0_RoleGroupDetail.ToList();
                var rolesSelected = (from r in roles join rgd in roleGorups on r.Id equals rgd.RoleId where rgd.RoleGroupId == roleGroupId select r).ToList();

                if (rolesSelected != null && rolesSelected.Count() > 0)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in rolesSelected)
                    {
                        if (!userRoles.Any(o => o == role.Name))
                            await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
            }



            return Ok(new Result<A0_RoleGroupUser>
            {
                Code = 0,
                Data = roleGroupUser,
                Message = $"Success!",
                Succeeded = true,
            });
        }
        return Ok(new Result<A0_RoleGroupUser>
        {
            Code = 0,

            Message = $"Dữ liệu đã tồn tại!",
            Succeeded = false,
        });
    }
    [HttpGet("DeleteRoleGroup")]
    public async Task<IActionResult> DeleteRoleGroup(string roleGroupId, string userId)
    {
        var roleGroup = _context.A0_RoleGroup.FirstOrDefault(o => o.Id == roleGroupId);
        if (roleGroup != null)
        {
            //Xóa  RoleGroupUser
            var roleGroupUsers = _context.A0_RoleGroupUser.Where(o => o.RoleGroupId == roleGroup.Id);
            if (roleGroupUsers != null && roleGroupUsers.Count() > 0)
            {
                _context.A0_RoleGroupUser.RemoveRange(roleGroupUsers);
                await _context.SaveChangesAsync();
            }

            //Lấy roles trong roleGroup
            var roles = Get_Roles_In_RoleGroups(roleGroupId);
            if (roles != null && roles.Count() > 0)
            {
                var user = await _userManager.FindByIdAsync(userId);
                //xóa role ứng với các user
                foreach (var role in roles)
                    await Delete_RoleForUser(roleGroupId, role, user);
            }

            return Ok(new Result<A0_RoleGroup>
            {
                Code = 0,
                Data = roleGroup,
                Message = $"Success!",
                Succeeded = true,
            });
        }
        return Ok(new Result<A0_RoleGroup>
        {
            Code = 0,

            Message = $"Không tìm thấy dữ liệu!",
            Succeeded = false,
        });
    }
    #endregion


    #region Comment
    [HttpGet("AddRole")]
    public async Task<IActionResult> AddRole(string roleId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, roleId);
            return Ok(new Result<object>
            {
                Code = 0,
                Data = user,
                Message = $"Success!",
                Succeeded = true,
            });
        }
        return Ok(new Result<object>
        {
            Code = 0,

            Message = $"Không tìm thấy dữ liệu!",
            Succeeded = false,
        });
    }
    [HttpGet("DeleteRole")]
    public async Task<IActionResult> DeleteRole(string roleId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, roleId);
            return Ok(new Result<object>
            {
                Code = 0,
                Data = user,
                Message = $"Success!",
                Succeeded = true,
            });
        }
        return Ok(new Result<object>
        {
            Code = 0,

            Message = $"Không tìm thấy dữ liệu!",
            Succeeded = false,
        });
    }
    #endregion


    private List<IdentityRole> Get_Roles_In_RoleGroups(string roleGroupId)
    {
        try
        {
            var _roles = _roleManager.Roles.ToList();
            var _roleGorups = _context.A0_RoleGroupDetail.ToList();
            var rolesSelected = (from r in _roles join rgd in _roleGorups on r.Id equals rgd.RoleId where rgd.RoleGroupId == roleGroupId select r).ToList();
            return rolesSelected;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    private async Task<bool> Delete_RoleForUser(string roleGroupId, IdentityRole role, ApplicationUser user)
    {
        //Kiểm tra quyền có ở nhóm khác ứng với user này không? nếu không có thì mới xóa, có thì không xóa
        var _roleGroupUsers = _context.A0_RoleGroupUser.Where(o => o.RoleGroupId != roleGroupId && o.UserId == user.Id).ToList();
        var roleGroupIds = _roleGroupUsers.Select(o => o.RoleGroupId).ToList();
        //Lấy danh sách Roles của roleGroupIds
        var _roles = _roleManager.Roles.ToList();
        var _roleGroupDetais = _context.A0_RoleGroupDetail.ToList();
        var roleGroupIds_roles = (from r in _roles join rgd in _roleGroupDetais on r.Id equals rgd.RoleId where roleGroupIds.Contains(rgd.RoleGroupId) select r).ToList();
        if (roleGroupIds_roles == null)
            roleGroupIds_roles = new List<IdentityRole>();
        if (!roleGroupIds_roles.Any(o => o.Id == role.Id))
            await _userManager.RemoveFromRoleAsync(user, role.Name);


        return true;
    }
    private List<ApplicationUser> Get_Users_In_RoleGroups(string roleGroupId)
    {
        try
        {
            var _users = _userManager.Users.ToList();
            var roleGroupUsers = _context.A0_RoleGroupUser.ToList();

            var users = (from u in _users join rgu in roleGroupUsers on u.Id equals rgu.UserId where rgu.RoleGroupId == roleGroupId select u).ToList();
            return users;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }



}
