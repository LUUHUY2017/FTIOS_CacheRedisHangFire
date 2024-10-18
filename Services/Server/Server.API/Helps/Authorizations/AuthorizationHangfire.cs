using Hangfire.Dashboard;

namespace Server.API.Helps.Authorizations
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            // Add your authorization logic here
            // For example, allow only local requests:
            return httpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
