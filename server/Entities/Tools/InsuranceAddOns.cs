using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class InsuranceAddOns 
    {
        public InsuranceAddOns()
        {
            RatesByAge = new HashSet<InsuranceAddOnsRateAge>();
        }

        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int HealthPlanId { get; set; }

        [Column(TypeName = "VARCHAR(80)", Order = 3)]
        public string Name { get; set; }

        [Column(Order = 4)]
        public bool? Beneficiary { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 5)]
        public float IndividualRate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 6)]
        public float CoverageSingleRate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 7)]
        public float CoverageCoupleRate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 8)]
        public float CoverageFamilyRate { get; set; }

        [Column(Order = 9)]
        public int MinimumEE { get; set; }

        [Column(Order = 10)]
        public int TypeCalculate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public HealthPlans HealthPlans { get; set; }

        public ICollection<InsuranceAddOnsRateAge> RatesByAge { get; set; }


    }
}
