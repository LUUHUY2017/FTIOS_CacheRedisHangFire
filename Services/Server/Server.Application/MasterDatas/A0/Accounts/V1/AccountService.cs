using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A0.Accounts.V1.Commons;
using Server.Core.Identity.Entities;
using Server.Core.Identity.Interfaces.Accounts.Requests;
using Server.Core.Identity.Interfaces.Accounts.Services;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Application.MasterDatas.A0.Accounts.V1
{
    public class AccountService : IAccountService
    {
        public string? UserId { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMasterDataDbContext _dbContext;


        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IMasterDataDbContext dbContext
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
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
    }
}
