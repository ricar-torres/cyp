using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class InsurancePlanAddOns 
    {

        //[Column(Order = 1)]
        //public int Id { get; set; }

        [Column(Order = 2)]
        public int CoverId { get; set; }

        [Column(Order = 3)]
        public int InsuranceAddOnsId { get; set; }

        //[Column(TypeName = "VARCHAR(max)", Order = 3)]
        //public string Value { get; set; }

        [JsonIgnore]
        public Covers Covers { get; set; }

        //[JsonIgnore]
        public InsuranceAddOns InsuranceAddOns { get; set; }

    }
}
