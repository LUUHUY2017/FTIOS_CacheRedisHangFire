using Hangfire.Dashboard;

namespace AMMS.VIETTEL.SMAS.Helps.Authorizations;

public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext dashboardContext)
    {
        return true;
    }
}
