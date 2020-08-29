using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class InsuranceRate
    {

        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int CoverId { get; set; }

        [Column(Order = 3)]
        public int Age { get; set; }

        [Column(TypeName = "date", Order = 4)]
        public DateTime RateEffectiveDate { get; set; }

        [Column(TypeName = "date", Order = 5)]
        public DateTime RateExpirationDate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 6)]
        public float IndividualRate { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 7)]
        public float IndividualTobaccoRate { get; set; }


        [Column(Order = 8)]
        public int PolicyYear { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [JsonIgnore]
        public Covers Covers { get; set; }

    }
}
