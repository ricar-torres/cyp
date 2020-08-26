using System;
using System.Collections.Generic;

namespace WebApi.Entities {
	public partial class Dependents {
		public int Id { get; set; }
		public int ClientId { get; set; }
		public string Name { get; set; }
		public string Initial { get; set; }
		public string LastName1 { get; set; }
		public string LastName2 { get; set; }
		public string Ssn { get; set; }
		public byte? Gender { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Email { get; set; }
		public string Phone1 { get; set; }
		public string Phone2 { get; set; }
		public byte? Relationship { get; set; }
		public int CoverId { get; set; }
		public string ContractNumber { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public int? CityId { get; set; }
		public int? AgencyId { get; set; }

		public virtual Agencies Agency { get; set; }
		public virtual Cities City { get; set; }
		public virtual Clients Client { get; set; }
		public virtual Covers Cover { get; set; }
	}
}