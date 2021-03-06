using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities {
	public class TypeOfRelationshipMap {
		public TypeOfRelationshipMap(EntityTypeBuilder<TypeOfRelationship> entityBuilder) {
			entityBuilder.HasKey(_ => _.Id);
			entityBuilder.Property(_ => _.Id).ValueGeneratedNever();
		}
	}
}