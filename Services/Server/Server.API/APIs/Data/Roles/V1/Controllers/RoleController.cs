using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Commons;
using Shared.Core.Identity;

namespace Server.API.APIs.Data.Roles.V1.Controllers;

/// <summary>
/// Quản lý nhóm quyền
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[AuthorizeMaster(Roles = RoleConst.AdminPage)]
public class RoleController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    public RoleController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Danh sách quyền
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Gets()
    {
        //var _RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //var roles = _RoleManager.Roles.ToList();

        var roles = RoleConst.IdentityRoleList();
        return Ok(new Result<List<IdentityRole>>
        {
            Code = 0,
            Data = roles,
            Message = $"Success!",
            Succeeded = true,
        });
    }
    /// <summary>
    /// Chi tiết nhóm quyền
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    [HttpGet("{roleName}")]
    public async Task<IActionResult> Detail(string roleName)
    {
        var _RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var role = _RoleManager.Roles.FirstOrDefault();
        return Ok(new Result<IdentityRole>
        {
            Code = 0,
            Data = role,
            Message = $"Success!",
            Succeeded = true,
        });
    }
    /// <summary>
    /// Thêm mới nhóm quyền
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    [HttpPost("Add{roleName}")]
    public async Task<IActionResult> Add(string roleName)
    {
        var _RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        IdentityResult adminRoleResult;
        IdentityRole role = _RoleManager.Roles.FirstOrDefault(o => o.Name == roleName);
        if (role == null)
        {
            adminRoleResult = await _RoleManager.CreateAsync(new IdentityRole(roleName));
            return Ok(new Result<IdentityRole>
            {
                Code = 0,
                Data = role,
                Message = adminRoleResult.Succeeded ? $"Success!" : "False!",
                Succeeded = adminRoleResult.Succeeded,
            });
        }
        else
        {
            return Ok(new Result<IdentityRole>
            {
                Code = 1,
                Data = role,
                Message = $"Quyền đã tồn tại!",
                Succeeded = false,
            });
        }
    }

    //[HttpGet("InitRoles")]
    //public async Task<IActionResult> InitRoles()
    //{
    //    var _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //    var roles = _roleManager.Roles.ToList();
    //    if (roles == null)
    //        roles = new List<IdentityRole>();
    //    var rolesDefault = RoleConst.IdentityRoleList();
    //    foreach (var role in rolesDefault)
    //        if (!roles.Any(o => o.Name == role.Name))
    //            await _roleManager.CreateAsync(role);


    //    return Ok(new Result<List<IdentityRole>>
    //    {
    //        Code = 0,
    //        Data = roles,
    //        Message = $"Success!",
    //        Succeeded = true,
    //    });
    //}

}
