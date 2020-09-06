using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public class AlianzaAddOns
    {
        public int AlianzaId { get; set; }
        public int InsuranceAddOnId { get; set; }

        [Column(TypeName = "decimal(12,2)", Order = 3)]
        public float Cost { get; set; }
        public virtual Alianzas Alianza { get; set; }
        public virtual InsuranceAddOns InsuranceAddOn { get; set; }
    }
}
