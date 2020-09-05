using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public partial class Covers
    {
        public Covers()
        {
            Alianzas = new HashSet<Alianzas>();
            Clients = new HashSet<Clients>();
            Dependents = new HashSet<Dependents>();
            BenefitTypes = new HashSet<InsurancePlanBenefit>();
            AddOns = new HashSet<InsurancePlanAddOns>();
            Rate = new HashSet<InsuranceRate>();
            MultiAssists = new HashSet<MultiAssists>();
        }

        public int Id { get; set; }
        public int HealthPlanId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Sob { get; set; }
        public bool? Alianza { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string SobImg { get; set; }
        public string Type { get; set; } // Colocar si es alianza +65,-65, ect...
        public bool? Beneficiary { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 6)]
        public float IndividualRate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 7)]
        public float CoverageSingleRate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 8)]
        public float CoverageCoupleRate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 9)]
        public float CoverageFamilyRate { get; set; }

        [Column(Order = 10)]
        public int MinimumEE { get; set; }

        [Column(Order = 11)]
        public int TypeCalculate { get; set; }

        public virtual HealthPlans HealthPlan { get; set; }
        public virtual ICollection<Alianzas> Alianzas { get; set; }
        public virtual ICollection<Clients> Clients { get; set; }
        public virtual ICollection<Dependents> Dependents { get; set; }



        public virtual ICollection<InsurancePlanBenefit> BenefitTypes { get; set; }
        public virtual ICollection<InsurancePlanAddOns> AddOns { get; set; }
        public virtual ICollection<InsuranceRate> Rate { get; set; }
        public int[] AddOnsAlt { get; set; }

        public virtual ICollection<MultiAssists> MultiAssists { get; set; }


    }
}
