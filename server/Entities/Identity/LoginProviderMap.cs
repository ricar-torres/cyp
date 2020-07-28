using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class LoginProviderMap
    {
        public LoginProviderMap(EntityTypeBuilder<LoginProvider> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Id).ValueGeneratedNever(); //No identity

            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.ProviderKey);

        }
    }
}
