using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WebApi.Entities
{
    public class InsuranceAddOnsRateAge
    {

        [Column(Order = 1)]
        public int Id { get; set; }


        [Column(Order = 2)]
        public int InsuranceAddOnsId { get; set; }

        [Column(Order = 3)]
        public int Age { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 4)]
        public float Rate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public InsuranceAddOns InsuranceAddOns { get; set; }
    }
}
