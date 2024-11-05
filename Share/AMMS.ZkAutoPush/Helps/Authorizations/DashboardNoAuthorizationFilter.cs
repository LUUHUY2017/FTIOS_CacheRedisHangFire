using Hangfire.Dashboard;

namespace AMMS.ZkAutoPush.Helps.Authorizations
{
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            return true;
        }
    }
}
