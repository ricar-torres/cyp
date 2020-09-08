using System;
using System.Collections.Generic;

namespace WebApi.Entities {

	public class MultiAssistsVehicle {

		public int Id { get; set; }
		public int MultiAssistId { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int Year { get; set; }
		public string Vin { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public virtual MultiAssists MultiAssists { get; set; }
		public MultiAssistsVehicle() {

		}

	}
}