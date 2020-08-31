using System;
using System.Collections.Generic;

namespace WebApi.Entities {
	public class MultiAssists {

        public MultiAssists(){
            Beneficiaries = new HashSet<Beneficiaries>();
            MultiAssistsVehicle = new HashSet<MultiAssistsVehicle>();
        }

		public int Id { get; set; }
		public int ClientProductId { get; set; }
		public int CoverId { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public DateTime? EligibleWaitingPeriodDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? SentDate { get; set; }
        public double? Cost { get; set; }
        public string? Ref1 { get; set; }
        public string? Ref2 { get; set; }
        public string? Ref3 { get; set; }
		public string StatusId { get; set; }
        public string? AccountType { get; set; }
        public string? BankName { get; set; }
        public string? AccountHolderName { get; set; }
        public string? RoutingNum { get; set; }
        public string? AccountNum { get; set; }
        public string? ExpDate { get; set; }
        public int? DebDay { get; set; }

        public string? DebRecurringType { get; set; }

		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
        public virtual ClientProduct ClientProduct { get; set; }
        public virtual Covers Cover { get; set; }

        public virtual ICollection<Beneficiaries> Beneficiaries { get; set; }

        public virtual ICollection<MultiAssistsVehicle> MultiAssistsVehicle { get; set; }


	}
}
