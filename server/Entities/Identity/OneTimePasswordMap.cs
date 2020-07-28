using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Identity
{
    public class OneTimePasswordMap
    {
        public OneTimePasswordMap(EntityTypeBuilder<OneTimePassword> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => t.Id);

            entityBuilder.Property(t => t.ApplicationId).IsRequired();

            //Default Values
            entityBuilder.Property(t => t.FCreateUserId).HasDefaultValueSql("0");
            entityBuilder.Property(t => t.FUpdUserId).HasDefaultValueSql("0");
            entityBuilder.Property(t => t.UpdDt).HasDefaultValueSql("GETDATE()");
            entityBuilder.Property(t => t.CreateDt).HasDefaultValueSql("GETDATE()");
            entityBuilder.Property(t => t.UpdDt).HasDefaultValueSql("GETDATE()");
            entityBuilder.Property(t => t.UpdSysHostDt).HasDefaultValueSql("CONVERT(date, GETDATE())");
            entityBuilder.Property(t => t.UpdSysSqlUser).HasDefaultValueSql("SUSER_SNAME()");
            entityBuilder.Property(t => t.UpdSysHostNm).HasDefaultValueSql("HOST_NAME()");
            entityBuilder.Property(t => t.DelFlag).HasDefaultValueSql("0");

            //FK
            entityBuilder.HasOne(u => u.Application).WithMany().HasForeignKey(u => u.ApplicationId).OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasIndex(t => new { t.ApplicationId, t.UserName });
            entityBuilder.HasIndex(t => new { t.ApplicationId, t.UserName, t.DelFlag });

        }
    }
}
