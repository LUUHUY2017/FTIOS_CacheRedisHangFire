using IdentityServer4;
using IdentityServer4.Models;

namespace Server.Core.Identity.Entities;

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
        new ApiScope("deviceapi", "Device API"),
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
                    Scopes = { "masterapi", "deviceapi" }
                },
        };

    /*  định nghĩa ra các Client chín là các ứng dụng ta định làm , chính là webportal , server (chính là swagger) và .. */
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            #region Masster
                        //Master API Swagger
                        new Client
                            {
                                ClientId = "bi.master.api.swagger.dev",
                                ClientName = "Master Swagger",

                                AllowedGrantTypes = GrantTypes.Implicit,
                                AllowAccessTokensViaBrowser = true,
                                RequireConsent = false,

                                RedirectUris =           { "https://localhost:5001/swagger/oauth2-redirect.html"  }, // chuyển hướng
                                PostLogoutRedirectUris = { "https://localhost:5001/swagger/oauth2-redirect.html"  },// chuyển hướng đăng xuất
                                AllowedCorsOrigins =     { "https://localhost:5001"  }, // cho phép nguồn gốc cores

                                AllowedScopes = new List<string>
                                {
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    "masterapi"
                                }
                            },
                        new Client
                            {
                                ClientId = "bi.master.api.swagger.prod",
                                ClientName = "Master Swagger",

                                AllowedGrantTypes = GrantTypes.Implicit,
                                AllowAccessTokensViaBrowser = true,
                                RequireConsent = false,

                                RedirectUris =           {  "https://retail.acs.vn:8001/swagger/oauth2-redirect.html" }, // chuyển hướng
                                PostLogoutRedirectUris = {  "https://retail.acs.vn:8001/swagger/oauth2-redirect.html" },// chuyển hướng đăng xuất
                                AllowedCorsOrigins =     {  "https://retail.acs.vn:8001" }, // cho phép nguồn gốc cores

                                AllowedScopes = new List<string>
                                {
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    "masterapi"
                                }
                            },

                        new Client
                            {
                                ClientId = "bi.master.webserver.dev",
                                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                                ClientName = "AMMS Masster Webserver",
                                AllowedGrantTypes = GrantTypes.Code,
                                AllowedScopes = new List<string>
                                {
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    IdentityServerConstants.StandardScopes.OfflineAccess,
                                    IdentityServerConstants.StandardScopes.Address,
                                    IdentityServerConstants.StandardScopes.Email,
                                    IdentityServerConstants.StandardScopes.Phone,
                                    "masterapi"
                                },
                                RedirectUris = new List<string> { "https://localhost:5001/signin-oidc" },
                                PostLogoutRedirectUris = new List<string> { "https://localhost:5001/signout-callback-oidc" },
                                AllowAccessTokensViaBrowser = true,
                                UpdateAccessTokenClaimsOnRefresh = true,
                            },
                        new Client
                            {
                                ClientId = "bi.master.webserver.prod",
                                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                                ClientName = "AMMS Masster Webserver",
                                AllowedGrantTypes = GrantTypes.Code,
                                AllowedScopes = new List<string>
                                {
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    IdentityServerConstants.StandardScopes.OfflineAccess,
                                    IdentityServerConstants.StandardScopes.Address,
                                    IdentityServerConstants.StandardScopes.Email,
                                    IdentityServerConstants.StandardScopes.Phone,
                                    "masterapi"
                                },
                                RedirectUris = new List<string> { "https://retail.acs.vn:8001/signin-oidc" },
                                PostLogoutRedirectUris = new List<string> { "https://retail.acs.vn:8001/signout-callback-oidc" },
                                AllowAccessTokensViaBrowser = true,
                                UpdateAccessTokenClaimsOnRefresh = true,
                            },

                        new Client
                        {
                            ClientId = "amms.master.webapp",
                            ClientName = "AMMS Master Web Aapp",

                            ClientSecrets = { new Secret("secret".Sha256()) },//  mã hóa theo Sha256
                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            AllowOfflineAccess = true,
                            AllowedScopes = new List<string>
                            {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                IdentityServerConstants.StandardScopes.OfflineAccess,
                                IdentityServerConstants.StandardScopes.Address,
                                IdentityServerConstants.StandardScopes.Email,
                                IdentityServerConstants.StandardScopes.Phone,
                                "ammsiotgateway", "masterapi"
                            },
                            UpdateAccessTokenClaimsOnRefresh = true,
                        },
          
            #endregion            
            
            #region Device
            // Device API
            new Client
            {
                ClientId = "bi.device.webapi.dev",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "BI Device WebAPI DEV",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "masterapi", "deviceapi"
                },
                RedirectUris = new List<string> { "https://localhost:5003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5003/signout-callback-oidc" },
                AllowAccessTokensViaBrowser = true,
            },
            new Client
            {
                ClientId = "bi.device.webapi.prod",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "BI Device WebAPI",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "masterapi", "deviceapi"
                },
                RedirectUris = new List<string> { "https://retail.acs.vn:8003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://retail.acs.vn:8003/signout-callback-oidc" },
                AllowAccessTokensViaBrowser = true,
            },
            

            //Devices Swagger
            new Client
            {
                ClientId = "bi.device.webapi.swagger.dev",
                ClientName = "Device Webserver Swagger",

                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,

                RedirectUris =           { "https://localhost:5003/swagger/oauth2-redirect.html" }, // chuyển hướng
                PostLogoutRedirectUris = { "https://localhost:5003/swagger/oauth2-redirect.html" },// chuyển hướng đăng xuất
                AllowedCorsOrigins =     { "https://localhost:5003" }, // cho phép nguồn gốc cores

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "masterapi", "deviceapi"
                }
            },
            new Client
            {
                ClientId = "bi.device.webapi.swagger.prod",
                ClientName = "Delivery API Swagger Prod",

                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,

                RedirectUris =           { "https://retail.acs.vn:8003/swagger/oauth2-redirect.html" }, // chuyển hướng
                PostLogoutRedirectUris = { "https://retail.acs.vn:8003/swagger/oauth2-redirect.html" },// chuyển hướng đăng xuất
                AllowedCorsOrigins =     { "https://retail.acs.vn:8003" }, // cho phép nguồn gốc cores

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "masterapi", "deviceapi"
                }
            },
             
            #endregion
 
            
        };
}

