using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class RoleMap
    {
        public RoleMap(EntityTypeBuilder<AppRole> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => t.Id);

            entityBuilder.Property(t => t.Name).IsRequired();

            //Constraint
            entityBuilder.HasIndex(t => new { t.Name }).IsUnique();

        }
    }
}
