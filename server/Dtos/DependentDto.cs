using WebApi.Entities;

namespace server.Dtos {
	public class DependentDto : Dependents {
		public DependentDto() {

		}
		public string RelationName { get; set; }
	}
}