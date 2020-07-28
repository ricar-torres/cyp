using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebApi.Entities.Identity
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [JsonIgnore]
        public AppUser User { get; set; }

        //[JsonIgnore]
        public AppRole Role { get; set; }
    }
}
