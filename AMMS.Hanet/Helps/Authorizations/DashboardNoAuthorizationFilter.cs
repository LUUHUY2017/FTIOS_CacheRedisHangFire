﻿using Hangfire.Dashboard;

namespace AMMS.Hanet.Helps.Authorizations
{
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            return true;
        }
    }
}
