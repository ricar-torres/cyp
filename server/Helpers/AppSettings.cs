using System;
namespace WebApi.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int TokenValidDays { get; set; }
        public string DefaultConnection { get; set; }
        public LdapConfig LDAP { get; set; }
        public string DocumentsPath { get; set; }
        public int OtpValidDays { get; set; }
        public string PublicSiteUrl { get; set; }
    }
}
