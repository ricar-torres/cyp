using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace server.Entities {
	public class TypeOfRelationshipMap {
		public TypeOfRelationshipMap(EntityTypeBuilder<TypeOfRelationship> entityBuilder) {
			entityBuilder.HasKey(_ => _.Id);
			entityBuilder.Property(_ => _.Id).ValueGeneratedNever();
		}
	}
}