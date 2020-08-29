using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class InsurancePlanBenefit 
    {

        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int CoverId { get; set; }

        [Column(Order = 3)]
        public int InsuranceBenefitTypeId { get; set; }

        [Column(TypeName = "VARCHAR(max)", Order = 4)]
        public string Value { get; set; }


        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public Covers Covers { get; set; }

        [JsonIgnore]
        public InsuranceBenefitType InsuranceBenefitType { get; set; }

    }
}
