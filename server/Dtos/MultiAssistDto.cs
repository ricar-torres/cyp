using WebApi.Entities;

namespace server.Dtos {
	public class MultiAssistDto {
		public MultiAssists MultiAssist { get; set; }
		public int ClientId { get; set; }
		public string Name { get; set; }
		public HealthPlans HealthPlan { get; set; }
	}
}