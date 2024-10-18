using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Server.Core.Identity.Entities;
using Server.Infrastructure.Datas.MasterData;
using System.Security.Claims;

namespace Server.API.Services;


public sealed class ProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly RoleManager<IdentityRole> _roleMgr;
    private readonly MasterDataDbContext _masterDbContext;
    public string UserId { get; set; }

    public ProfileService(
        UserManager<ApplicationUser> userMgr,
        RoleManager<IdentityRole> roleMgr,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        MasterDataDbContext masterDbContext
        )
    {
        _userMgr = userMgr;
        _roleMgr = roleMgr;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _masterDbContext = masterDbContext;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string sub = context.Subject.GetSubjectId();
        UserId = sub;
        ApplicationUser user = await _userMgr.FindByIdAsync(sub);
        ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

        List<Claim> claims = userClaims.Claims.ToList();

        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

        if (_userMgr.SupportsUserRole)
        {
            IList<string> roles = await _userMgr.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                if (_roleMgr.SupportsRoleClaims)
                {
                    IdentityRole role = await _roleMgr.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        claims.AddRange(await _roleMgr.GetClaimsAsync(role));
                    }
                }
            }
        }


        //var person = await _masterDbContext.Person.FirstOrDefaultAsync(o => o.Id == user.Id);
        //string avata = "";
        //if (person != null)
        //{
        //    string fileName = FollderCommon.AvataFolder + person.PersonImage;
        //    //avata = FollderCommon.ImageFileToBase64(fileName);
        //    if (System.IO.File.Exists(fileName))
        //        avata = $"{AuthBaseController.AMMS_Master_HostAddress}/Datas/Avatas/{person.PersonImage}";
        //}
        //claims.Add(new Claim("avata", avata));

        claims.Add(new Claim("username", user.UserName));
        claims.Add(new Claim("firstname", string.IsNullOrEmpty(user.FirstName) ? "" : user.FirstName));
        claims.Add(new Claim("lastname", string.IsNullOrEmpty(user.LastName) ? "" : user.LastName));
        claims.Add(new Claim("fullname", $"{user.LastName} {user.FirstName}"));
        claims.Add(new Claim("phone", string.IsNullOrEmpty(user.PhoneNumber) ? "" : user.PhoneNumber));
        claims.Add(new Claim("email", string.IsNullOrEmpty(user.Email) ? "" : user.Email));

        //var pageIds = await GetMenuByUserAsync(UserId);
        //if (pageIds != null && pageIds.Count > 0)
        //    foreach (var item in pageIds)
        //        claims.Add(new Claim("pageIds", item));

        //string rolesJson = JsonSerializer.Serialize(pageIds);
        //claims.Add(new Claim("pageId", rolesJson, IdentityServerConstants.ClaimValueTypes.Json));

        //context.IssuedClaims = claims;
        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string sub = context.Subject.GetSubjectId();
        ApplicationUser user = await _userMgr.FindByIdAsync(sub);
        context.IsActive = user != null;
        UserId = sub;
    }

    //public async Task<List<string>> GetMenuByUserAsync(string userId)
    //{
    //    var pageId = new List<string>();
    //    var menu = new List<MenuShowModel>();
    //    try
    //    {
    //        var pages = PagesConst._Menu_General_Left.ToList();
    //        var categories = Category.ListCategory.ToList();
    //        var _RoleGroupUser = _masterDbContext.RoleGroupUser.Where(o => o.UserId == userId).ToList();

    //        var GroupIds = _RoleGroupUser.Select(o => o.RoleGroupId).ToList();
    //        var _RoleGroupPage = _masterDbContext.RoleGroupPage.Where(o => GroupIds.Contains(o.RoleGroupId)).ToList();

    //        var pageUsers = (from groupPage in _RoleGroupPage
    //                         join _page in pages on groupPage.PageId equals _page.Id into PA
    //                         from pa in PA.DefaultIfEmpty()
    //                         select new { Id = pa.Id }).ToList();
    //        pageId = pageUsers.Select(o => o.Id).ToList();
    //    }
    //    catch (Exception ex)
    //    { }
    //    return pageId;

    //}
}
