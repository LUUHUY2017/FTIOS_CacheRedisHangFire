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
using Shared.Core.Identity.Menu;

namespace Server.API.Areas.Managers.RoleGroups.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster(Roles = RoleConst.AdminPage)]

public class RoleGroupController : Controller
{
    const string pathUrl = "~/Areas/Managers/RoleGroups/V1/Views/";
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MasterDataDbContext _context;
    private readonly IdentityContext _identityContext;


    public RoleGroupController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, MasterDataDbContext context, IdentityContext identityContext)
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
        return View(pathUrl + "Index.cshtml");
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
            var roles = Get_Pages_In_RoleGroups(id);
            if (roles != null && roles.Count() > 0)
            {
                //xóa role ứng với các user
                //foreach (var role in roles)
                //await Delete_RoleForUser(id, role);
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

    #region Role From Page
    [HttpGet("GetPages")]
    public async Task<IActionResult> GetPages(string roleGroupId)
    {
        try
        {
            var pages = PagesConst._Menu_General_Left.ToList();
            var groupPages = _context.A0_RoleGroupPage.ToList();
            var pagesSelected = (from r in pages
                                 join rgd in groupPages on r.Id equals rgd.PageId
                                 where rgd.RoleGroupId == roleGroupId
                                 select r).OrderBy(o => o.Module).ToList();

            var pagesUnSelected = pages.Where(o => pagesSelected.All(x => x.Id != o.Id)).ToList();

            if (pages != null)
            {
                return Ok(new
                {
                    Code = 0,
                    Data = new
                    {
                        DatasSelected = pagesSelected,
                        DatasUnSelected = pagesUnSelected,
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

    [HttpGet("GetRoles1")]
    public async Task<IActionResult> GetRoles1(string roleGroupId)
    {
        try
        {
            var roles = RoleConst.IdentityRoleList();
            var roleGorups = _context.A0_RoleGroupDetail.ToList();
            var rolesSelected = (from r in roles
                                 join rgd in roleGorups on r.Id equals rgd.RoleId
                                 where rgd.RoleGroupId == roleGroupId
                                 select r).ToList();

            if (roles != null)
            {
                return Ok(new Result<ViewRoleDetails>
                {
                    Code = 0,
                    Data = new ViewRoleDetails()
                    {
                        DatasSelected = rolesSelected,
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


    [HttpGet("AddPage")]
    public async Task<IActionResult> AddPage(string roleGroupId, string pageId)
    {
        var roleGrPage = _context.A0_RoleGroupPage.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.PageId == pageId);
        if (roleGrPage == null)
        {
            #region add RoleGroupPage
            roleGrPage = new A0_RoleGroupPage
            {
                Actived = true,
                PageId = pageId,
                RoleGroupId = roleGroupId,
            };
            _context.A0_RoleGroupPage.Add(roleGrPage);
            await _context.SaveChangesAsync();
            #endregion

            //#region Add Role User: from RolePage to User
            //var pages = Get_Pages_In_RoleGroups(roleGroupId);
            //var users = Get_Users_In_RoleGroups(roleGroupId);
            //if (pages != null && pages.Count() > 0 && users != null && users.Count() > 0)
            //{
            //    foreach (var user in users)
            //    {
            //        try
            //        {
            //            await Add_Roles2User(user, pages);
            //        }
            //        catch (Exception e) { }
            //    }
            //}
            //#endregion

            //#region gán Role cho Group
            //var currentRoles = PagesConst._Menu_MD_Left.Where(o => o.Id == pageId).Select(o => o.RolePermission.Trim()).ToList();
            //List<string> roleNames = Convert2ListString(currentRoles);
            //var groupRoles = _context.A0_RoleGroupDetail.Where(o => o.RoleGroupId == roleGroupId).ToList();

            //var roles = _roleManager.Roles.ToList();
            //foreach (var roleName in roleNames)
            //{
            //    var role = roles.FirstOrDefault(o => o.Name == roleName);
            //    if (!groupRoles.Any(o => o.RoleId == role.Id) && role != null)
            //    {
            //        var item = new A0_RoleGroupDetail()
            //        {
            //            Actived = true,
            //            RoleId = role.Id,
            //            RoleGroupId = roleGroupId,
            //        };
            //        _context.A0_RoleGroupDetail.Add(item);
            //        await _context.SaveChangesAsync();
            //    }
            //}
            //#endregion

            return Ok(new Result<A0_RoleGroupPage>
            {
                Code = 0,
                Data = roleGrPage,
                Message = $"Success!",
                Succeeded = true,
            });
        }


        return Ok(new Result<A0_RoleGroupPage>
        {
            Code = 0,
            Data = null,
            Message = $"Dữ liệu đã tồn tại!",
            Succeeded = false,
        });
    }
    [HttpGet("DeletePage")]
    public async Task<IActionResult> DeletePage(string roleGroupId, string pageId)
    {
        try
        {
            var roleGroupPage = _context.A0_RoleGroupPage.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.PageId == pageId);
            if (roleGroupPage != null)
            {
                // Xóa RoleGroupPage
                _context.A0_RoleGroupPage.Remove(roleGroupPage);
                await _context.SaveChangesAsync();


                // Xóa Role gắn với User khi xóa Page
                //var currentPageRoles = PagesConst._Menu_MD_Left.Where(o => o.Id == pageId).Select(o => o.RolePermission.Trim()).ToList();
                //List<string> roleNamePages = Convert2ListString(currentPageRoles);
                //foreach (var item in roleNamePages)
                //{
                //    var role = await _roleManager.FindByNameAsync(item);
                //    if (role != null)
                //        await Delete_RoleForUser(roleGroupId, role);
                //}


                // Xóa Role khỏi Group
                //var pageGroupss = Get_Pages_In_RoleGroups(roleGroupId);
                //var currentGroupRoles = pageGroupss.Select(o => o.RolePermission.Trim()).ToList();
                //List<string> roleNameGroups = Convert2ListString(currentGroupRoles);

                //var roleDeleteds = roleNamePages.Where(o => roleNameGroups.All(x => x != o)).ToList();
                //foreach (var roleName in roleDeleteds)
                //{
                //    var role = await _roleManager.FindByNameAsync(roleName);
                //    if (role == null)
                //        continue;
                //    var roleGroupDetails = _context.A0_RoleGroupDetail.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.RoleId == role.Id);
                //    if (roleGroupDetails == null)
                //        continue;
                //    _context.A0_RoleGroupDetail.RemoveRange(roleGroupDetails);
                //    await _context.SaveChangesAsync();
                //}

                return Ok(new Result<A0_RoleGroupPage>
                {
                    Code = 0,
                    Data = roleGroupPage,
                    Message = $"Success!",
                    Succeeded = true,
                });
            }
            return Ok(new Result<A0_RoleGroupPage>
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

    [HttpGet("AddUser1")]
    public async Task<IActionResult> AddUser1(string roleGroupId, string userId)
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
                var userRoles = await _userManager.GetRolesAsync(user);

                // lấy Danh sách pageRole của RoleGroup
                //#region gắn role  cho user 
                //var pages = PagesConst._Menu_MD_Left.ToList();
                //var roleGroups = _context.A0_RoleGroupPage.ToList();
                //var pageGroups = (from r in pages
                //                  join rgd in roleGroups on r.Id equals rgd.PageId
                //                  where rgd.RoleGroupId == roleGroupId
                //                  select r).ToList();

                //var currentRoles = pageGroups.Select(o => o.RolePermission.Trim()).ToList();
                //List<string> roleNames = Convert2ListString(currentRoles);

                //foreach (var roleName in roleNames)
                //{
                //    if (!userRoles.Any(o => o == roleName))
                //        await _userManager.AddToRoleAsync(user, roleName);
                //}
                //#endregion



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
    [HttpGet("DeleteUser1")]
    public async Task<IActionResult> DeleteUser1(string roleGroupId, string userId)
    {
        var roleGroupUser = _context.A0_RoleGroupUser.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.UserId == userId);
        if (roleGroupUser != null)
        {
            _context.A0_RoleGroupUser.Remove(roleGroupUser);
            await _context.SaveChangesAsync();

            //Delete Role To User

            //Lấy danh sách quyền của user


            //ApplicationUser userToMakeAdmin = await _userManager.FindByIdAsync(userId);
            //if (userToMakeAdmin != null)
            //{
            //    var user_roles = await _userManager.GetRolesAsync(userToMakeAdmin);

            //    var roles = PagesConst._Menu_MD_Left.ToList();
            //    var roleGroups = _context.A0_RoleGroupPage.ToList();
            //    var pages = (from r in roles
            //                 join rgd in roleGroups on r.Id equals rgd.PageId
            //                 where rgd.RoleGroupId == roleGroupId
            //                 select r).ToList();

            //    var currentRoles = pages.Select(o => o.RolePermission.Trim()).ToList();
            //    List<string> roleNames = Convert2ListString(currentRoles);
            //    foreach (var role in roleNames)
            //    {
            //        if (user_roles.Any(o => o == role))
            //            await _userManager.RemoveFromRoleAsync(userToMakeAdmin, role);
            //    }
            //}

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
    private List<A0_Page> Get_Pages_In_RoleGroups(string roleGroupId)
    {
        try
        {
            var _pages = PagesConst._Menu_MD_Left.ToList();
            var _roleGorups = _context.A0_RoleGroupPage.ToList();
            var pagessSelected = (from r in _pages
                                  join rgd in _roleGorups on r.Id equals rgd.PageId
                                  where rgd.RoleGroupId == roleGroupId
                                  select new A0_Page()
                                  {
                                      Id = r.Id,
                                      Module = r.Module,
                                      Name = r.Name,
                                      Description = r.Description,
                                      RolePermission = r.RolePermission,
                                      Url = r.Url,
                                  }).ToList();
            return pagessSelected;
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

            var users = (from u in _users
                         join rgu in roleGroupUsers on u.Id equals rgu.UserId
                         where rgu.RoleGroupId == roleGroupId
                         select u).ToList();
            return users;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
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
    private List<string> Convert2ListString(List<string> roles)
    {
        try
        {
            string combinedString = string.Join(",", roles);
            roles = combinedString.Split(',').Distinct().ToList();

        }
        catch (Exception e) { }
        return roles;
    }

    private async Task<bool> Add_Roles2User1(ApplicationUser user, List<A0_Page> pages)
    {
        try
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRoles = pages.Select(o => o.RolePermission.Trim()).ToList();
            List<string> roleNames = Convert2ListString(currentRoles);
            foreach (var role in roleNames)
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


        var pages = PagesConst._Menu_MD_Left.ToList();
        var roleGorups = _context.A0_RoleGroupPage.ToList();

        var currentRoles = (from r in pages
                            join rgd in roleGorups on r.Id equals rgd.PageId
                            where rgd.RoleGroupId == roleGroupId
                            select r.RolePermission).ToList();
        List<string> roleNames = Convert2ListString(currentRoles);
        if (users != null && users.Count() > 0)
        {
            foreach (var user in users)
            {
                if (role != null && !roleNames.Contains(role.Name))
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
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

