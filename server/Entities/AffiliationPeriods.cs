using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class AffiliationPeriods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StatDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
