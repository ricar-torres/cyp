using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class MenuItemMap
    {
        public MenuItemMap(EntityTypeBuilder<MenuItem> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Id).ValueGeneratedNever(); //No identity

            entityBuilder.Property(t => t.Title).IsRequired();
            entityBuilder.Property(t => t.Type).IsRequired();

        }
    }
}
