using System;
using System.Collections.Generic;

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
