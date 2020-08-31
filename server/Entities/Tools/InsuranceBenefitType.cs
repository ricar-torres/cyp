using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class InsuranceBenefitType 
    {

        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int ParentBenefitTypeID { get; set; }

        [Column(TypeName = "NVARCHAR(255)", Order = 3)]
        public string BenefitType { get; set; }

        [Column(Order = 4)]
        public int RowOrder { get; set; }


        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


    }
}
