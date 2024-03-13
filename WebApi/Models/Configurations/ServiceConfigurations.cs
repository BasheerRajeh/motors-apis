using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Entities;

namespace WebApi.Models.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> entity)
        {
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.TitleKz).HasMaxLength(100);
            entity.Property(e => e.TitleRu).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.DescriptionKz).HasMaxLength(4000);
            entity.Property(e => e.DescriptionRu).HasMaxLength(4000);
            entity.Property(e => e.ImagePath).HasMaxLength(150);
        }
    }
}
