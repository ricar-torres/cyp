using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class MenuPermissionMap
    {
        public MenuPermissionMap(EntityTypeBuilder<MenuPermission> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => new {t.RoleMenuId,t.PermissionId});

            //
            entityBuilder.Property(t => t.RoleMenuId).IsRequired();
            entityBuilder.Property(t => t.PermissionId).IsRequired();

            //FK
            entityBuilder.HasOne(p => p.RoleMenu)
                         .WithMany(b => b.Permissions)
                         .HasForeignKey(p => p.RoleMenuId);

        }
    }
}
