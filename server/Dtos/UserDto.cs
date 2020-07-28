using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebApi.Entities;
using WebApi.Entities.Identity;

namespace WebApi.Dtos
{
    public class UserDto : BaseEntity
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public int LoginProviderId { get; set; }
        public int ApplicationId { get; set; }

        public string ApplicationKey { get; set; }

        public string Token { get; set; }

        public LoginProvider LoginProvider { get; set; }

        public ICollection<UserRole> Roles { get; set; }

        public int[] RolesAlt { get; set; }

        public string SSNO { get; set; }
        public string FileType { get; set; }
        public string UserType { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public int Reference4 { get; set; }

        public static implicit operator UserDto(AppUser v)
        {
            throw new NotImplementedException();
        }
    }
}
