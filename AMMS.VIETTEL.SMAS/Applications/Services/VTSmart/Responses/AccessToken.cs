namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; } // Expiration time in seconds
        public string token_type { get; set; }
        public string scope { get; set; }

        public DateTime expires_at { get; private set; } // Expiration DateTime

        // Constructor to initialize the expiration date
        public AccessToken(string token, int expiresInSeconds, string tokenType, string scope)
        {
            access_token = token;
            expires_in = expiresInSeconds;
            token_type = tokenType;
            this.scope = scope;
            expires_at = DateTime.UtcNow.AddSeconds(expires_in);
        }

        public bool IsTokenValid()
        {
            if (access_token != null)
                return DateTime.UtcNow < expires_at;
            else
                return false;
        }
    }

}
