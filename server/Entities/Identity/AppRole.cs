using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities.Identity
{
    public class AppRole : BaseEntity
    {
        public AppRole() { }
        public AppRole(string name) : this()
        {
            this.Name = name;
            MenuItems = new HashSet<RoleMenu>();
        }

        [Column(TypeName = "VARCHAR(255)", Order = 2)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(100)", Order = 3)]
        public string Description { get; set; }

        public ICollection<RoleMenu> MenuItems { get; set; }
    }
}