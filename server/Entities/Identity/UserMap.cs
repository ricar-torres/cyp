using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<AppUser> entityBuilder)
        {
            
            //Pk
            entityBuilder.HasKey(t => t.Id);

            entityBuilder.Property(t => t.UserName).IsRequired();
            entityBuilder.Property(t => t.FirstName).IsRequired();
            entityBuilder.Property(t => t.LastName).IsRequired();
            //entityBuilder.Property(t => t.Email).IsRequired();
            entityBuilder.Property(t => t.LoginProviderId).IsRequired();
            entityBuilder.Property(t => t.ApplicationId).IsRequired();
            entityBuilder.Property(t => t.UserType).IsRequired();
            
            //entityBuilder.Property(t => t.Reference1).IsRequired();

            //Default Values
            entityBuilder.Property(t => t.CreateDt).HasDefaultValueSql("GETDATE()");
            entityBuilder.Property(t => t.UpdDt).HasDefaultValueSql("GETDATE()");
           
            //entityBuilder.Property(t => t.UpdHostNm).ValueGeneratedOnAddOrUpdate().HasDefaultValue(GetHostName());
            entityBuilder.Property(t => t.UpdSysHostDt).HasDefaultValueSql("CONVERT(date, GETDATE())");
            entityBuilder.Property(t => t.UpdSysSqlUser).HasDefaultValueSql("SUSER_SNAME()");
            entityBuilder.Property(t => t.UpdSysHostNm).HasDefaultValueSql("HOST_NAME()");

            //Constraint
            entityBuilder.HasIndex(t => new { t.ApplicationId, t.UserName}).IsUnique();
            //entityBuilder.HasIndex(t => new { t.ApplicationId, t.Reference1 }).IsUnique();
            entityBuilder.HasCheckConstraint("CK_AppUser_UserType", "UserType IN('ADMIN','USER') ");

            //FK
            entityBuilder.HasOne(u => u.LoginProvider).WithMany().HasForeignKey(u => u.LoginProviderId);
            entityBuilder.HasOne(u => u.Application).WithMany().HasForeignKey(u => u.ApplicationId);

            //Exclude
            entityBuilder.Ignore(t => t.FullName);
            entityBuilder.Ignore(t => t.RolesAlt);
            entityBuilder.Ignore(t => t.Token);
            entityBuilder.Ignore(t => t.Claims);

        }
    }
}
