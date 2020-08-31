using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities {
	public class AffTypeMap {
		public AffTypeMap(EntityTypeBuilder<AffType> entityBuilder) {
			entityBuilder.HasKey(_ => _.Id);
			entityBuilder.Property(_ => _.Id).ValueGeneratedNever();
            entityBuilder.Property(_ => _.Name).IsRequired();
		}
	}
}