using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class RoleMenuMap
    {
        public RoleMenuMap(EntityTypeBuilder<RoleMenu> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => t.Id);

            //
            entityBuilder.Property(t => t.RoleId).IsRequired();
            entityBuilder.Property(t => t.MenuItemId).IsRequired();

            // Unique
            entityBuilder.HasAlternateKey(t => new { t.RoleId, t.MenuItemId });

            //FK
            //entityBuilder.
            entityBuilder.HasOne(p => p.MenuItem)
                         .WithMany(b => b.Roles)
                         .HasForeignKey(p => p.MenuItemId);

            entityBuilder.HasOne(p => p.Role)
                         .WithMany(b => b.MenuItems)
                         .HasForeignKey(p => p.RoleId);

        }
    }
}
