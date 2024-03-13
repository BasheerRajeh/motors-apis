using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models.Entities;

namespace WebApi.Models.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> entity)
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Battery).HasMaxLength(1500);
            entity.Property(e => e.BatteryKz).HasMaxLength(1500);
            entity.Property(e => e.BatteryRu).HasMaxLength(1500);
            entity.Property(e => e.Performance).HasMaxLength(1500);
            entity.Property(e => e.PerformanceKz).HasMaxLength(1500);
            entity.Property(e => e.PerformanceRu).HasMaxLength(1500);
            entity.Property(e => e.Range).HasMaxLength(1500);
            entity.Property(e => e.RangeKz).HasMaxLength(1500);
            entity.Property(e => e.RangeRu).HasMaxLength(1500);
            entity.Property(e => e.Efficiency).HasMaxLength(1500);
            entity.Property(e => e.EfficiencyKz).HasMaxLength(1500);
            entity.Property(e => e.EfficiencyRu).HasMaxLength(1500);
            entity.Property(e => e.PriceCurrency).HasMaxLength(1500);
            entity.Property(e => e.BrandName).HasMaxLength(100);
            entity.Property(e => e.ImagePath).HasMaxLength(150);
        }
    }

    public class CarBrandConfiguration : IEntityTypeConfiguration<CarBrand>
    {
        public void Configure(EntityTypeBuilder<CarBrand> entity)
        {
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.IconPath).HasMaxLength(150);
            entity.ToTable("Car_Brands");
        }
    }
    public class CarReviewConfiguration : IEntityTypeConfiguration<CarReview>
    {
        public void Configure(EntityTypeBuilder<CarReview> entity)
        {
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Contact).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.UserIp).HasMaxLength(150);
            entity.ToTable("Car_Reviews");
        }
    }
    public class CarImageConfiguration : IEntityTypeConfiguration<CarImage>
    {
        public void Configure(EntityTypeBuilder<CarImage> entity)
        {
            entity.Property(e => e.Path).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.MimeType).HasMaxLength(100);
            entity.ToTable("Car_Images");
        }
    }
    public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
    {
        public void Configure(EntityTypeBuilder<Testimonial> entity)
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.Comments).HasMaxLength(600);
            entity.Property(e => e.ProfilePhoto).HasMaxLength(200);
        }
    }
}