namespace Share.WebApp.Settings;

public class AppSettings
{
    public ApplicationDetail? ApplicationDetail { get; set; }
    public Authentication? Authentication { get; set; }

    public string RedisURL { get; set; }
    public string DataArea { get; set; }
}

public class ApplicationDetail
{
    public string ApplicationName { get; set; }
    public string Description { get; set; }
    public string ContactWebsite { get; set; }
    public string LicenseDetail { get; set; }
}


public class Authentication
{
    public string HostAddress { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}