using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class InsurancePlanAddOnsMap
    {
        public InsurancePlanAddOnsMap(EntityTypeBuilder<InsurancePlanAddOns> entityBuilder)
        {

            //Pk
            entityBuilder.HasKey(t => new { t.CoverId , t.InsuranceAddOnsId });

            //
            entityBuilder.Property(t => t.CoverId).IsRequired();
            entityBuilder.Property(t => t.InsuranceAddOnsId).IsRequired();

            ////FK
            entityBuilder.HasOne(p => p.Covers)
                         .WithMany(b => b.AddOns)
                         .HasForeignKey(p => p.CoverId);

        }
    }
}
