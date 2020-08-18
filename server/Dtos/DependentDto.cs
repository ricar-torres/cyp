using System;
using server.Entities;
using WebApi.Entities;

namespace server.Dtos {
	public class DependentDto : Dependents {
		public DependentDto() {

		}
		// public new DateTime EffectiveDate { get; set; }
		// public new DateTime BirthDate { get; set; }
		public string CoverName { get; set; }
		public TypeOfRelationship RelationshipType { get; set; }
		public HealthPlans Plan { get; set; }
	}
}