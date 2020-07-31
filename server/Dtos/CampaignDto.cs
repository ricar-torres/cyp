using System;

namespace server.Dtos {
	public class CampaignDto {
		public int Id { get; set; }
		public string Name { get; set; }
		public int Origin { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}