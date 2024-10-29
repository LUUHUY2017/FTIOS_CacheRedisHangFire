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
            var all_accounts = await _userManager.Users.Where(x => x.Type != null).ToListAsync();
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
    }
}
