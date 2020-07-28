using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebApi.Entities.Identity
{
    public class MenuPermission
    {
        public int RoleMenuId { get; set; }

        public int PermissionId { get; set; }

        public Permission Permission { get; set; }

        [JsonIgnore]
        public RoleMenu RoleMenu { get; set; }
    }
}
