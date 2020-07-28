using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Entities.Identity
{
    public class AppUser : BaseEntity
    {

        public AppUser()
        {
            Roles = new HashSet<UserRole>();
        }


        [Column(TypeName = "VARCHAR(254)", Order = 2)]
        public string UserName { get; set; }

        [Column(TypeName = "VARCHAR(100)", Order = 3)]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR(100)", Order = 4)]
        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        //[StringLength(100)]
        //public string CompleteNm { get; set; }

        [Column(TypeName = "VARCHAR(254)", Order = 5)]
        public string Email { get; set; }
        public Boolean EmailConfirmed { get; set; }

        [Column(TypeName = "VARCHAR(15)", Order = 6)]
        public string PhoneNumber { get; set; }

        [Column(Order = 7)]
        public Boolean PhoneNumberConfirmed { get; set; }

        [Column(Order = 8)]
        public byte[] PasswordHash { get; set; }

        [Column(Order = 9)]
        public byte[] PasswordSalt { get; set; }

        [Column(Order = 10)]
        public int FailRetryQty { get; set; }

        [Column(Order = 11)]
        public int LoginProviderId { get; set; }

        [Column(TypeName = "VARCHAR(30)", Order = 12)]
        public string Reference1 { get; set; }

        [Column(TypeName = "VARCHAR(30)", Order = 13)]
        public string Reference2 { get; set; }

        [Column(TypeName = "VARCHAR(30)", Order = 14)]
        public string Reference3 { get; set; }

        [Column(TypeName = "VARCHAR(30)", Order = 15)]
        public int Reference4 { get; set; }

        [Column(Order = 16)]
        public Boolean IsLock { get; set; }

        [Column(Order = 17)]
        public Boolean IsChgPwd { get; set; }

        [Column(Order = 18)]
        public DateTime ExpirationDt { get; set; }

        [Column(Order = 19)]
        public DateTime LastLoginDt { get; set; }

        [Column(Order = 20)]
        public int ApplicationId { get; set; }

        [Column(Order = 21)]
        public string UserType { get; set; }

        public LoginProvider LoginProvider { get; set; }
        public Application Application { get; set; }

        public string Token { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public int[] RolesAlt { get; set; }
        public string[] Claims { get; set; }


    }

}