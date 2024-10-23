using AMMS.Share.WebApp.Helps;
using AMMS.WebAPI.Areas.Manage.RoleGroups.V1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Entities.A0;
using Server.Core.Identity.Entities;
using Server.Infrastructure.Datas.MasterData;
using Server.Infrastructure.Identity;
using Shared.Core.Commons;
using Shared.Core.Identity;

namespace Server.API.Areas.Managers.RoleGroups.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster(Roles = $"{RoleConst.MasterDataPage}")]

public class RoleGroup1Controller : Controller
{
    const string pathUrl = "~/Areas/Managers/RoleGroups/V1/Views/";
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MasterDataDbContext _context;
    private readonly IdentityContext _identityContext;


    public RoleGroup1Controller(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, MasterDataDbContext context, IdentityContext identityContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
        _identityContext = identityContext;
    }
    [HttpGet]
    public IActionResult Index(string tab = "role_groups")
    {
        ViewBag.Tab = tab;
        return View(pathUrl + "Index1.cshtml");
    }
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        try
        {
            var roles = _context.A0_RoleGroup.ToList();
            return Ok(new Result<List<A0_RoleGroup>>
            {
                Code = 0,
                Data = roles,
                Message = $"Thành công!",
                Succeeded = true,
            });
        }
        catch (Exception ex)
        {
            return Ok(new Result<List<A0_RoleGroup>>
            {
                Code = 0,
                Data = null,
                Message = $"Lỗi: {ex.Message}",
                Succeeded = false,
            });
        }
    }
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] RoleGroupModel model)
    {
        if (string.IsNullOrEmpty(model.Id))
        {
            var role = new A0_RoleGroup()
            {
                Id = Guid.NewGuid().ToString(),
                Actived = model.Actived,
                Name = model.Name,
                Descriptions = model.Descriptions,
            };
            _context.A0_RoleGroup.Add(role);
            await _context.SaveChangesAsync();

            return Ok(new Result<A0_RoleGroup>
            {
                Code = 0,
                Data = role,
                Message = $"Success!",
                Succeeded = true,
            });
        }
        else
        {
            var role = _context.A0_RoleGroup.FirstOrDefault(o => o.Id == model.Id);
            if (role != null)
            {
                role.Name = model.Name;
                role.Descriptions = model.Descriptions;
                role.Actived = model.Actived;

                _context.A0_RoleGroup.Update(role);
                await _context.SaveChangesAsync();
                var roles = _context.A0_RoleGroup.ToList();
                return Ok(new Result<A0_RoleGroup>
                {
                    Code = 0,
                    Data = role,
                    Message = $"Success!",
                    Succeeded = true,
                });
            }
            else
            {
                return Ok(new Result<A0_RoleGroup>
                {
                    Code = 1,
                    Data = null,
                    Message = $"Không tìm thấy dữ liệu!",
                    Succeeded = false,
                });
            }

        }
    }
    [HttpGet("Delete")]
    public async Task<IActionResult> Delete(string id, string reasonDelete)
    {
        var roleGroup = _context.A0_RoleGroup.FirstOrDefault(o => o.Id == id);
        if (roleGroup != null)
        {
            _context.A0_RoleGroup.Remove(roleGroup);
            var roleGroups = _context.A0_RoleGroupDetail.Where(o => o.RoleGroupId == roleGroup.Id);
            if (roleGroups != null && roleGroups.Count() > 0)
                _context.A0_RoleGroupDetail.RemoveRange(roleGroups);

            await _context.SaveChangesAsync();

            //Lấy roles trong roleGroup
            var roles = Get_Roles_In_RoleGroups(id);
            if (roles != null && roles.Count() > 0)
            {
                //xóa role ứng với các user
                foreach (var role in roles)
                    await Delete_RoleForUser(id, role);
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
            Data = null,
            Message = $"Không tìm thấy dữ liệu!",
            Succeeded = false,
        });
    }

    #region RoleDetail

    [HttpGet("GetRoles")]
    public async Task<IActionResult> GetRoles(string roleGroupId)
    {
        try
        {
            //var roles = _roleManager.Roles.ToList();
            var roles = RoleConst.IdentityRoleList();
            var roleGorups = _context.A0_RoleGroupDetail.ToList();
            var rolesSelected = (from r in roles
                                 join rgd in roleGorups on r.Id equals rgd.RoleId
                                 where rgd.RoleGroupId == roleGroupId
                                 select r).ToList();

            var rolesUnSelected = roles.Where(o => rolesSelected.All(x => x.Id != o.Id)).ToList();

            if (roles != null)
            {
                return Ok(new Result<ViewRoleDetails>
                {
                    Code = 0,
                    Data = new ViewRoleDetails()
                    {
                        DatasSelected = rolesSelected,
                        DatasUnSelected = rolesUnSelected,
                    },
                    Message = $"Success!",
                    Succeeded = true,
                });
            }
            return Ok(new Result<ViewRoleDetails>
            {
                Code = 0,
                Data = null,
                Message = $"Dữ liệu đã tồn tại!",
                Succeeded = false,
            });
        }
        catch (Exception ex)
        {
            return Ok(new Result<ViewRoleDetails>
            {
                Code = 0,
                Data = null,
                Message = $"Lỗi: {ex.Message}",
                Succeeded = false,
            });
        }

    }


    [HttpGet("AddRole")]
    public async Task<IActionResult> AddRole(string roleGroupId, string roleId)
    {
        var role = _context.A0_RoleGroupDetail.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.RoleId == roleId);
        if (role == null)
        {
            role = new A0_RoleGroupDetail()
            {
                Actived = true,
                RoleId = roleId,
                RoleGroupId = roleGroupId,
            };
            _context.A0_RoleGroupDetail.Add(role);
            await _context.SaveChangesAsync();

            //Lấy tất cả các userId trong RoleGroup
            var roles = Get_Roles_In_RoleGroups(roleGroupId);
            var users = Get_Users_In_RoleGroups(roleGroupId);
            if (roles != null && roles.Count() > 0 && users != null && users.Count() > 0)
            {
                foreach (var user in users)
                {
                    try
                    {
                        await Add_Roles2User(user, roles);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            return Ok(new Result<A0_RoleGroupDetail>
            {
                Code = 0,
                Data = role,
                Message = $"Success!",
                Succeeded = true,
            });
        }
        return Ok(new Result<A0_RoleGroupDetail>
        {
            Code = 0,
            Data = null,
            Message = $"Dữ liệu đã tồn tại!",
            Succeeded = false,
        });
    }
    [HttpGet("DeleteRole")]
    public async Task<IActionResult> DeleteRole(string roleGroupId, string roleId)
    {
        try
        {
            var roleGroupDetail = _context.A0_RoleGroupDetail.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.RoleId == roleId);
            if (roleGroupDetail != null)
            {
                _context.A0_RoleGroupDetail.Remove(roleGroupDetail);
                await _context.SaveChangesAsync();

                var role = await _roleManager.FindByIdAsync(roleId);
                if (role != null)
                    await Delete_RoleForUser(roleGroupId, role);

                return Ok(new Result<A0_RoleGroupDetail>
                {
                    Code = 0,
                    Data = roleGroupDetail,
                    Message = $"Success!",
                    Succeeded = true,
                });
            }
            return Ok(new Result<A0_RoleGroupDetail>
            {
                Code = 0,
                Data = null,
                Message = $"Không tìm thấy dữ liệu!",
                Succeeded = false,
            });
        }
        catch (Exception e)
        {
            return Ok(new Result<A0_RoleGroupDetail>
            {
                Code = 0,
                Data = null,
                Message = e.Message,
                Succeeded = false,
            });
        }

    }
    #endregion


    #region UserDetail
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers(string roleGroupId)
    {
        try
        {
            var roles = _userManager.Users.ToList();
            var roleGorups = _context.A0_RoleGroupUser.ToList();

            var rolesSelected = (from r in roles join rgd in roleGorups on r.Id equals rgd.UserId where rgd.RoleGroupId == roleGroupId select r).ToList();
            var rolesUnSelected = roles.Where(o => rolesSelected.All(x => x.Id != o.Id)).ToList();

            if (roles != null)
            {
                return Ok(new Result<ViewRoleGroupUser>
                {
                    Code = 0,
                    Data = new ViewRoleGroupUser()
                    {
                        DatasSelected = rolesSelected,
                        DatasUnSelected = rolesUnSelected,
                    },
                    Message = $"Success!",
                    Succeeded = true,
                });
            }
            return Ok(new Result<ViewRoleGroupUser>
            {
                Code = 0,
                Data = null,
                Message = $"Dữ liệu đã tồn tại!",
                Succeeded = false,
            });
        }
        catch (Exception ex)
        {
            return Ok(new Result<ViewRoleDetails>
            {
                Code = 0,
                Data = null,
                Message = $"Lỗi: {ex.Message}",
                Succeeded = false,
            });
        }

    }

    [HttpGet("AddUser")]
    public async Task<IActionResult> AddUser(string roleGroupId, string userId)
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
            Data = null,
            Message = $"Dữ liệu đã tồn tại!",
            Succeeded = false,
        });
    }
    [HttpGet("DeleteUser")]
    public async Task<IActionResult> DeleteUser(string roleGroupId, string userId)
    {
        var roleGroupUser = _context.A0_RoleGroupUser.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.UserId == userId);
        if (roleGroupUser != null)
        {
            _context.A0_RoleGroupUser.Remove(roleGroupUser);
            await _context.SaveChangesAsync();

            //Delete Role To User
            ApplicationUser userToMakeAdmin = await _userManager.FindByIdAsync(userId);
            if (userToMakeAdmin != null)
            {
                //Lấy danh sách RoleGroups theo User
                var roleGroups = (from rg in _context.A0_RoleGroup join rgu in _context.A0_RoleGroupUser on rg.Id equals rgu.RoleGroupId where rgu.UserId == userId select rg).ToList();
                if (roleGroups == null)
                    roleGroups = new List<A0_RoleGroup>();
                //Lấy danh sách Roles theo RoleGroup
                var roles = _roleManager.Roles.ToList();
                var roleGroupDetais = _context.A0_RoleGroupDetail.ToList();
                var roles_roleGroups = (from r in roles join rgd in roleGroupDetais on r.Id equals rgd.RoleId where rgd.RoleGroupId == roleGroupId select r).ToList();
                if (roles_roleGroups == null)
                    roles_roleGroups = new List<IdentityRole>();

                //Lấy danh sách quyền của user
                var user_roles = await _userManager.GetRolesAsync(userToMakeAdmin);

                ApplicationUser user = await _userManager.FindByIdAsync(userId);
                if (user_roles != null && user_roles.Count() > 0)
                {
                    _userManager.RemoveFromRolesAsync(user, user_roles);
                }
                if (roles_roleGroups != null && roles_roleGroups.Count() > 0)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles_roleGroups)
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
            Data = null,
            Message = $"Không tìm thấy dữ liệu!",
            Succeeded = false,
        });
    }

    [HttpGet("GetUserInfo")]
    public async Task<IActionResult> GetUserInfo(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoleGroups = (from rg in _context.A0_RoleGroup join rgu in _context.A0_RoleGroupUser on rg.Id equals rgu.RoleGroupId where rgu.UserId == userId select rg).ToList();

            var user_roles = await _userManager.GetRolesAsync(user);
            var xx = new List<IdentityRole>();
            xx = user_roles?.Select(o => new IdentityRole() { Name = o }).ToList();
            return Ok(new Result<object>
            {
                Code = 0,
                Data = new
                {
                    userRoles = xx,
                    userRoleGroups,
                },
                Message = $"Success!",
                Succeeded = true,
            });
        }

        return Ok(new Result<object>
        {
            Code = 0,
            Data = null,
            Message = $"Không tìm thấy thông tin user!",
            Succeeded = false,
        });
    }

    #endregion

    /// <summary>
    /// Lấy danh sách Roles của RoleGroup
    /// </summary>
    /// <param name="roleGroupId"></param>
    /// <returns></returns>
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
    private async Task<bool> Add_Role2User(ApplicationUser user, string role)
    {
        try
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any(o => o == role))
                await _userManager.AddToRoleAsync(user, role);

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    private async Task<bool> Add_Roles2User(ApplicationUser user, List<string> roles)
    {
        try
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                if (!userRoles.Any(o => o == role))
                    await _userManager.AddToRoleAsync(user, role);
            }

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    private async Task<bool> Add_Roles2User(ApplicationUser user, List<IdentityRole> roles)
    {
        try
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                if (!userRoles.Any(o => o == role.Name))
                    await _userManager.AddToRoleAsync(user, role.Name);
            }

            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    private async Task<bool> Delete_RoleForUser(string roleGroupId, IdentityRole role)
    {
        //Lấy tất cả các users của nhóm
        var users = Get_Users_In_RoleGroups(roleGroupId);
        if (users != null && users.Count() > 0)
        {
            foreach (var user in users)
            {
                //Kiểm tra quyền có ở nhóm khác ứng với user này không? nếu không có thì mới xóa, có thì không xóa
                //Lấy danh sách RoleGroups của user
                var _users = _userManager.Users.ToList();
                var _roleGroupUsers = _context.A0_RoleGroupUser.ToList();

                var roleGroupIds = (from u in _users join rgu in _roleGroupUsers on u.Id equals rgu.UserId where rgu.RoleGroupId != roleGroupId select rgu.RoleGroupId).ToList();
                //Lấy danh sách Roles của roleGroupIds
                var _roles = _roleManager.Roles.ToList();
                var _roleGroupDetais = _context.A0_RoleGroupDetail.ToList();
                var roleGroupIds_roles = (from r in _roles join rgd in _roleGroupDetais on r.Id equals rgd.RoleId where roleGroupIds.Contains(rgd.RoleGroupId) select r).ToList();

                if (roleGroupIds_roles != null && roleGroupIds_roles.Count() > 0)
                {
                    if (role != null)
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

            }
        }
        return true;
    }

    #region Quyền
    [HttpGet("Roles")]
    public async Task<IActionResult> Roles()
    {
        //var roles = _roleManager.Roles.ToList();
        var roles = RoleConst.IdentityRoleList();
        return Ok(new Result<List<IdentityRole>>
        {
            Code = 0,
            Data = roles,
            Message = $"Success!",
            Succeeded = true,
        });
    }

    [HttpGet("RoleInfo")]
    public async Task<IActionResult> RoleInfo(string roleId)
    {
        //Lấy roleGroups theo role
        var roleGroups = (from rg in _context.A0_RoleGroup join rgd in _context.A0_RoleGroupDetail on rg.Id equals rgd.RoleGroupId where rgd.RoleId == roleId select rg).ToList();
        var users = (from u in _identityContext.Users join ur in _identityContext.UserRoles on u.Id equals ur.UserId where ur.RoleId == roleId select u).ToList();

        return Ok(new Result<object>
        {
            Code = 0,
            Data = new
            {
                roleGroups,
                users,
            },
            Message = $"Success!",
            Succeeded = true,
        });
    }

    /// <summary>
    /// Đồng bộ quyền vào CSDL từ danh sách quyền
    /// </summary>
    /// <returns></returns>
    [HttpGet("SynRoles")]
    public async Task<IActionResult> SynRoles()
    {
        var roles = _roleManager.Roles.ToList();
        return Ok(new Result<List<IdentityRole>>
        {
            Code = 0,
            Data = roles,
            Message = $"Success!",
            Succeeded = true,
        });
    }

    #endregion
}

