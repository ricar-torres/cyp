using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApi.Entities {
	public partial class Tutors {
		public int? Id { get; set; }
		public int ClientId { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string PhiFileUrl { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }

		[JsonIgnore]
		[IgnoreDataMember]
		public virtual Clients Client { get; set; }
	}
}