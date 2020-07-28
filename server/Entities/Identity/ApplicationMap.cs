using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class ApplicationMap
    {
        public ApplicationMap(EntityTypeBuilder<Application> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Id).ValueGeneratedNever(); //No identity

            // Default Values
            //entityBuilder.Property(t => t.Key).ValueGeneratedOnAdd().HasDefaultValue(new Guid().ToString());

            //Constraints
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.Key).IsRequired();
            entityBuilder.Property(t => t.DelFlag).IsRequired();


        }
    }
}
