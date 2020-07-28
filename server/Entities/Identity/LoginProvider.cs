using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities.Identity
{
    public class LoginProvider
    {

        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(20)")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string ProviderKey { get; set; }

        public bool DelFlag { get; set; }

    }

    public enum LoginProviderEnum
    {
        Local = 1,
        LDAP = 2
    }
}
