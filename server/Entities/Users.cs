using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Users
    {
        public Users()
        {
            ClientUser = new HashSet<ClientUser>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RememberToken { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Roles Role { get; set; }
        public virtual ICollection<ClientUser> ClientUser { get; set; }
    }
}
