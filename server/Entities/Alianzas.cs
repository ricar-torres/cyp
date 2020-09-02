using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public partial class Alianzas
    {
        public Alianzas()
        {
            Beneficiaries = new HashSet<Beneficiaries>();
            ClientUser = new HashSet<ClientUser>();
            AlianzaAddOns = new HashSet<AlianzaAddOns>();
        }

        public int Id { get; set; }
        public int ClientProductId { get; set; }
        public int QualifyingEventId { get; set; }
        public int CoverId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime ElegibleDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndReason { get; set; }
        public byte? AffType { get; set; }
        public byte AffStatus { get; set; }
        public string AffFlag { get; set; }
        public bool? Coordination { get; set; }
        public bool? LifeInsurance { get; set; }
        public bool? MajorMedical { get; set; }
        public double? Prima { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public double? Joint { get; set; }
        public double? CoverAmount { get; set; }
        public double? LifeInsuranceAmount { get; set; }
        public double? MajorMedicalAmount { get; set; }
        public double? SubTotal { get; set; }

        public virtual ClientProduct ClientProduct { get; set; }
        public virtual Covers Cover { get; set; }
        public virtual QualifyingEvents QualifyingEvent { get; set; }
        public virtual ICollection<Beneficiaries> Beneficiaries { get; set; }
        public virtual ICollection<ClientUser> ClientUser { get; set; }
        public virtual ICollection<AlianzaAddOns> AlianzaAddOns { get; set; }
    }
}
