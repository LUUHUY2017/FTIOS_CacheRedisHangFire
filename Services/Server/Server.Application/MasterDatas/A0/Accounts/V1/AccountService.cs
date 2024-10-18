using Microsoft.AspNetCore.Identity;
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
        //private readonly IMailService _emailService;
        //private readonly JWTSettings _jwtSettings;
        //private readonly IDateTimeService _dateTimeService;
        //private readonly IFeatureManager _featureManager;

        private readonly IMasterDataDbContext _biDbContext;
        //private readonly IAmmsDataContext _ammsDataContext;
        //private readonly IMediator _mediator;

        //private readonly IVisitorRepository _visitorRepository;
        //private readonly ITruckDriverRepository _truckDriverRepository;

        //private readonly IPersonRepository _personRepository;


        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            //IMailService emailService,
            IMasterDataDbContext biDbContext
            //IOptions<JWTSettings> jwtSettings,
            //IDateTimeService dateTimeService,
            //IFeatureManager featureManager,
            //AmmsDataContext ammsDataContext,
            //IMediator mediator
            //, IVisitorRepository visitorRepository
            //, ITruckDriverRepository driverRepository
            //, IPersonRepository personRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            //_emailService = emailService;
            _biDbContext = biDbContext;
            //_jwtSettings = jwtSettings.Value;
            //_dateTimeService = dateTimeService;
            //_featureManager = featureManager;

            //_ammsDataContext = ammsDataContext;

            //_mediator = mediator;
            //_visitorRepository = visitorRepository;
            //_truckDriverRepository = driverRepository;
            //_personRepository = personRepository;
        }

        public async Task<List<UserAccountRes>> GetAccountSystems()
        {
            var all_accounts = _userManager.Users.ToList();
            //var all_persons = await _biDbContext.Users.ToListAsync();
            //var accounts = all_accounts.Where(o => all_persons.All(x => x.Id.ToString() != o.Id)).Select(u => new UserAccountRes()
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
            }).ToList();

            if (accounts == null)
                accounts = new List<UserAccountRes>();
            return accounts;
        }
    }
}
