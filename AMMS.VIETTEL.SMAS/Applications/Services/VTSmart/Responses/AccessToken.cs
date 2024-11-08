namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses
{
    public class AccessTokenLocal
    {
        public string access_token { get; set; }
        public int expires_in { get; set; } // Expiration time in seconds
        public string token_type { get; set; }
        public string scope { get; set; }

        public DateTime expires_at { get; private set; } // Expiration DateTime

        // Constructor to initialize the expiration date
        public AccessTokenLocal(string token, int expiresInSeconds, string tokenType, string scope)
        {
            access_token = token;
            expires_in = expiresInSeconds;
            token_type = tokenType;
            this.scope = scope;
            expires_at = DateTime.Now.AddSeconds(expires_in);
        }

        public bool IsTokenValid()
        {
            if (access_token != null)
                return DateTime.Now < expires_at;
            else
                return false;
        }
    }

    public class AccessToken
    {
        public string endpoint_gateway { get; set; }
        public string endpoint_identity { get; set; }

        public string access_token { get; set; }
        public int expires_in { get; set; } // Expiration time in seconds
        public string token_type { get; set; }
        public string scope { get; set; }
        public DateTime time_expires_in { get; set; } // Expiration DateTime
    }
}
