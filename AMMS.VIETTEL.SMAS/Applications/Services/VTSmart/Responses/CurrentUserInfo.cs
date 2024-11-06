namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses
{

    public class CurrentUserInfo
    {
        public CurrentUser currentUser { get; set; }
        public CurrentTenant currentTenant { get; set; }
    }
    public class CurrentTenant
    {
        public bool isAvailable { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class CurrentUser
    {
        public bool isAuthenticated { get; set; }
        public string id { get; set; }
        public string userName { get; set; }
        public object name { get; set; }
        public object surName { get; set; }
        public string phoneNumber { get; set; }
        public bool phoneNumberVerified { get; set; }
        public string email { get; set; }
        public bool emailVerified { get; set; }
        public string tenantId { get; set; }
        public List<string> roles { get; set; }
    }




}
