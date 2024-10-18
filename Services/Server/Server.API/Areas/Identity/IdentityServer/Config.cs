using IdentityServer4;
using IdentityServer4.Models;

namespace AMMS.WebAPI.Areas.Identity.IdentityServer;
public class Config
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
        new ApiScope("api.WebApp", "WebApp API"),

        new ApiScope("masterapi", "Master API"),
        new ApiScope("deliveryapi", "Delivery API"),
        new ApiScope("gateioapi", "GateIO API"),
        new ApiScope("wmsapi", "WMS API"),

        new ApiScope("ammsiotgateway", "AMMS IOT Gateway API"),
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
                 
                //Delivery
                new ApiResource("Delivery", "Delivery.API - API và ứng dụng vận hành, quản lý xuất nhập hàng")
                {
                    Scopes = { "masterapi", "deliveryapi" }
                },
                new ApiResource("DeliveryMobileAppDriver", "Ứng dụng trên điện thoại cho lái xe vận tải xuất nhập hàng")
                {
                    Scopes = { "masterapi", "deliveryapi" }
                },

                //GateIO
                new ApiResource("GateIO", "GateIO.API - API và ứng dụng vận hành, quản lý vào ra cổng")
                {
                    Scopes = { "masterapi", "gateioapi" }
                },
              
                //WMS
                new ApiResource("WMS", "WMS.API - API và ứng dụng vận hành, quản lý kho")
                {
                    Scopes = { "wmsapi", "wmsapi" }
                },


                //API GateWay
                new ApiResource("AMMSIoTGateway", "AMMS IoT Gateway")
                {
                    Scopes = { "ammsiotgateway", "masterapi", "deliveryapi", "gateioapi", "wmsapi" }
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
                                ClientId = "master.api.swagger",
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
                                ClientId = "master.api.swagger.prod",
                                ClientName = "Master Swagger",

                                AllowedGrantTypes = GrantTypes.Implicit,
                                AllowAccessTokensViaBrowser = true,
                                RequireConsent = false,

                                RedirectUris =           {  "https://amms.acs.vn:8001/swagger/oauth2-redirect.html" }, // chuyển hướng
                                PostLogoutRedirectUris = {  "https://amms.acs.vn:8001/swagger/oauth2-redirect.html" },// chuyển hướng đăng xuất
                                AllowedCorsOrigins =     {  "https://amms.acs.vn:8001" }, // cho phép nguồn gốc cores

                                AllowedScopes = new List<string>
                                {
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    "masterapi"
                                }
                            },
                        new Client
                            {
                                ClientId = "amms.master.webserver",
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
            
            #region Delivery
            // Delivery Webserver
            new Client
            {
                ClientId = "amms.delivery.webserver",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Webserver",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "ammsiotgateway", "masterapi", "deliveryapi"
                },
                RedirectUris = new List<string> { "https://localhost:5003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5003/signout-callback-oidc" },
                AllowAccessTokensViaBrowser = true,
            },
            new Client
            {
                ClientId = "amms.delivery.webserver.prod",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Webserver Prod",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "ammsiotgateway", "masterapi", "deliveryapi"
                },
                RedirectUris = new List<string> { "https://amms.acs.vn:8003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://amms.acs.vn:8003/signout-callback-oidc" },
                AllowAccessTokensViaBrowser = true,
            },
            new Client
            {
                ClientId = "amms.delivery.webserver.dev",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Webserver Dev",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "ammsiotgateway", "masterapi", "deliveryapi"
                },
                RedirectUris = new List<string> { "https://localhost:5003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5003/signout-callback-oidc" },
                AllowAccessTokensViaBrowser = true,
            },

            //Delivery Swagger
            new Client
            {
                ClientId = "delivery.api.swagger",
                ClientName = "Delivery Webserver Swagger",

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
                    "masterapi", "deliveryapi"
                }
            },
            new Client
            {
                ClientId = "delivery.api.swagger.prod",
                ClientName = "Delivery API Swagger Prod",

                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,

                RedirectUris =           { "https://amms.acs.vn:8003/swagger/oauth2-redirect.html" }, // chuyển hướng
                PostLogoutRedirectUris = { "https://amms.acs.vn:8003/swagger/oauth2-redirect.html" },// chuyển hướng đăng xuất
                AllowedCorsOrigins =     { "https://amms.acs.vn:8003" }, // cho phép nguồn gốc cores

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "masterapi", "deliveryapi"
                }
            },
            new Client
            {
                ClientId = "delivery.api.swagger.dev",
                ClientName = "Delivery API Swagger Dev",

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
                    "masterapi", "deliveryapi"
                }
            },


#region M2M
            new Client
            {
                ClientId = "amms.delivery.api",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Web API",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string> { "openid", "profile", "email", "ammsiotgateway", "masterapi", },
            },
            new Client
            {
                ClientId = "amms.delivery.api.prod",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Web API",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string> { "openid", "profile", "email", "ammsiotgateway", "masterapi", },
            },
#endregion
            //Web App Extent login
            new Client
            {
                ClientId = "amms.delivery.webapp",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Web Application",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string> { "openid", "profile", "email", "ammsiotgateway", "masterapi", "deliveryapi" },
                RedirectUris = new List<string> { "https://localhost:5003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5003/signout-callback-oidc" }
            },
            //Web App đăng nhập với username và password
            new Client
            {
                ClientId = "amms.delivery.webapp.password",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Web Application",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = new List<string> { "openid", "profile", "email", "ammsiotgateway", "masterapi", "deliveryapi" },
                RedirectUris = new List<string> { "https://localhost:5003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5003/signout-callback-oidc" }
            },
            //Win App
            new Client
            {
                ClientId = "amms.delivery.winapp",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Web Application",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string> { "openid", "profile", "email", "masterapi" },
                RedirectUris = new List<string> { "https://localhost:5003/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5003/signout-callback-oidc" }
            },
            //Mobile Driver
            new Client
            {
                ClientId = "amms.delivery.driver.mobile",
                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                ClientName = "AMMS Delivery Mobile App For Driver",

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                //RequireConsent = false,
                //RequirePkce = true,
                AllowOfflineAccess = true,
                //AllowedCorsOrigins={"http://localhost/"                     },
                     
                // ở client này cho phép chuy cập đến những cái này
                AllowedScopes = new List<string>
                {
                    // ở đây chúng ta cho chuy cập cả thông tin user lần api
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Phone,
                    "ammsiotgateway", "masterapi", "deliveryapi", "gateioapi", "wmsapi"
                },
                UpdateAccessTokenClaimsOnRefresh = true,

                //IdentityTokenLifetime = 43200, // thời gian hết hạn token 12 tiếng
                //AccessTokenLifetime = 43200,// thời gian hết hạn token 12 tiếng

                IdentityTokenLifetime = 120, // thời gian hết hạn token 12 tiếng
                AccessTokenLifetime = 120,// thời gian hết hạn token 12 tiếng
            },
            #endregion

            #region GateIO
                        // AMMS WebApp GateIO
                        new Client
                            {
                                ClientId = "amms.gateio.webapp",
                                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                                ClientName = "AMMS GateIO Web Application",
                                AllowedGrantTypes = GrantTypes.Code,
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
                                RedirectUris = new List<string> { "https://localhost:5005/signin-oidc" },
                                PostLogoutRedirectUris = new List<string> { "https://localhost:5005/signout-callback-oidc" }
                            },

            #endregion

            #region WMS
                        //WMS WebApp ext login
                        new Client
                            {
                                ClientId = "amms.wms.webapp.ext",
                                ClientSecrets = new List<Secret> { new("secret".Sha256()) },
                                ClientName = "AMMS WMS Web Application",
                                AllowedGrantTypes = GrantTypes.Code,
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
                                RedirectUris = new List<string> { "https://localhost:5007/signin-oidc" },
                                PostLogoutRedirectUris = new List<string> { "https://localhost:5007/signout-callback-oidc" }
                            },


            #endregion 
        };
}
