using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class UserRoleMap
    {
        public UserRoleMap(EntityTypeBuilder<UserRole> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => new { t.UserId, t.RoleId });

            //
            entityBuilder.Property(t => t.UserId).IsRequired();
            entityBuilder.Property(t => t.RoleId).IsRequired();

            //FK
            entityBuilder.HasOne(p => p.User)
                         .WithMany(b => b.Roles)
                         .HasForeignKey(p => p.UserId);

        }
    }
}
