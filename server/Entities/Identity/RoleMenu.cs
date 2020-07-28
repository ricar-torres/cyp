using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebApi.Entities.Identity
{
    public class RoleMenu
    {
        public RoleMenu()
        {
            Permissions = new HashSet<MenuPermission>();
        }

        //[Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int MenuItemId { get; set; }

        [JsonIgnore]
        public AppRole Role { get; set; }

        [JsonIgnore]
        public MenuItem MenuItem { get; set; }

        public ICollection<MenuPermission> Permissions { get; set; }
    }
}
