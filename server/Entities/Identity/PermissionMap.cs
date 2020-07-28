using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class PermissionMap
    {
        public PermissionMap(EntityTypeBuilder<Permission> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Id).ValueGeneratedNever(); //No identity

            //
            entityBuilder.Property(t => t.Name).IsRequired();


        }
    }
}
