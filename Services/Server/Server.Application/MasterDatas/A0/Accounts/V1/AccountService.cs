using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A0.Accounts.V1.Commons;
using Server.Core.Identity.Entities;
using Server.Core.Identity.Interfaces.Accounts.Requests;
using Server.Core.Identity.Interfaces.Accounts.Services;
using Server.Infrastructure.Datas.MasterData;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Hangfire.Dashboard;

namespace Server.Application.MasterDatas.A0.Accounts.V1
{
    public class AccountService : IAccountService
    {
        public string? UserId { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMasterDataDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly IUrlHelperFactory _urlHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IMasterDataDbContext dbContext,
            IEmailSender emailSender,
            IUrlHelperFactory urlHelper,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _emailSender = emailSender;
            _urlHelper = urlHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserAccountRes>> GetAccountSystems()
        {
            var userSuperAdmin = await _userManager.FindByIdAsync(UserId);
            var groupManager = (await _dbContext.RoleGroup.Where(x => x.Name == "Manager" || x.Name == "Operator" || x.Name == "SupperAdmin").ToListAsync()).Select(x => x.Id);
            var groupUserIds = (await _dbContext.RoleGroupUser.Where(x => !groupManager.Contains(x.RoleGroupId)).ToListAsync()).Select(x => x.UserId);
            var all_accounts = await _userManager.Users.Where(x => groupUserIds.Contains(x.Id)).ToListAsync();
            if (userSuperAdmin != null && string.IsNullOrEmpty(userSuperAdmin.Type))
            {
                all_accounts = await _userManager.Users.Where(x => groupUserIds.Contains(x.Id) || string.IsNullOrEmpty(x.Type)).ToListAsync();
            }
            var accounts = all_accounts.Select(u => new UserAccountRes()
            {
                Id = u.Id,
                Actived = u.LockoutEnabled,
                Descriptions = u.LockoutEnabled ? "Đang hoạt động" : "Đã khóa",
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                UserType = UserType.User,
                Type = u.Type,
                EmailConfirmed = u.EmailConfirmed,
            }).ToList();

            if (accounts == null)
                accounts = new List<UserAccountRes>();
            return accounts;
        }

        public async Task<List<UserAccountRes>> GetAccountSchools()
        {
            try
            {
                var groupManager = (await _dbContext.RoleGroup.Where(x => x.Name == "Manager" || x.Name == "Operator").ToListAsync()).Select(x => x.Id);
                var groupUserIds = (await _dbContext.RoleGroupUser.Where(x => groupManager.Contains(x.RoleGroupId)).ToListAsync()).Select(x => x.UserId);
                var all_accounts = await _userManager.Users.Where(x => groupUserIds.Contains(x.Id)).ToListAsync();
                var accounts = all_accounts.Select(u => new UserAccountRes()
                {
                    Id = u.Id,
                    Actived = u.LockoutEnabled,
                    Descriptions = u.LockoutEnabled ? "Đang hoạt động" : "Đã khóa",
                    PhoneNumber = u.PhoneNumber,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserType = UserType.User,
                    Type = u.Type,
                }).ToList();

                if (accounts == null)
                    accounts = new List<UserAccountRes>();
                return accounts;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task SendEmailConfirm(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var urlHelperModel = _urlHelper.GetUrlHelper(new ActionContext(
            _httpContextAccessor.HttpContext,
            _httpContextAccessor.HttpContext.GetRouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));

            var callbackUrl = urlHelperModel.Action(
                "ConfirmEmail",  // Action name
                "Account",       // Controller name
                new { userId = userId, code = code },
                protocol: _httpContextAccessor.HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
