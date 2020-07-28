using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Entities.Identity
{
    public class OneTimePassword : BaseEntity
    {
        public OneTimePassword()
        {
        }

        public int ApplicationId { get; set; }

        [Column(TypeName = "VARCHAR(254)", Order = 2)]
        public string UserName { get; set; }

        [Column(TypeName = "VARCHAR(255)", Order = 3)]
        public string OTP { get; set; }

        public int ValidDays { get; set; }

        public string Type { get; set; }

        public Application Application { get; set; }

    }
}
