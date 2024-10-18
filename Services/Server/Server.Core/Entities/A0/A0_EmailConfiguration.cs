using Shared.Core.Entities;

namespace Server.Core.Entities.A0
{
    public class A0_EmailConfiguration : EntityBase
    {
        public string Server { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public string? Email { get; set; }
        public int? Port { get; set; }
        public bool? EnableSSL { get; set; }
    }
}
