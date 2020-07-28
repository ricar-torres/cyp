using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities.Identity
{
    public class MenuItem
    {

        public MenuItem() { }
        public MenuItem(string name): this()
        {
            this.Title = Title;
            Roles = new HashSet<RoleMenu>();
        }

        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string Title { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string Description { get; set; }

        public int ParentId { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string Icon { get; set; }

        [Column(TypeName = "VARCHAR(2083)")]
        public string Url { get; set; }

        [Column(TypeName = "VARCHAR(2)")]
        public string Type { get; set; }

        public ICollection<RoleMenu> Roles { get; set; }
        
    }

}
