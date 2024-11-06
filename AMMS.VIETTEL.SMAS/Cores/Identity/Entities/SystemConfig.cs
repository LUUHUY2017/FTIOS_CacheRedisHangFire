using IdentityServer4.Models;

namespace AMMS.VIETTEL.SMAS.Cores.Identity.Entities;

public class SystemConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
      new IdentityResource[]
      {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),

                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),

                //new IdentityResource("roles", new[] { "role", "admin", "contentManager" }) // Add the role claim
      };
    public static IEnumerable<ApiScope> ApiScopes =>
    new ApiScope[]
    {
        new ApiScope("masterapi", "Master API"),
        new ApiScope("amms.hanet", "Hanet Device API"),
        new ApiScope("amms.zkteco", "Zkteco Device API"),
    };

    // danh sách các Api ở đây ta chỉ có mỗi thằng knowledgespace
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
                //Master Data
                new ApiResource()
                {
                    Name = "Master",
                    DisplayName =  "Master.API",
                    Scopes = { "masterapi"}
                },
                 
                //Device
                new ApiResource("Device", "Device.API - API nhận dữ liệu từ thiết bị")
                {
                    Scopes = { "masterapi", "amms.hanet", "amms.zkteco" }
                },
        };

}

