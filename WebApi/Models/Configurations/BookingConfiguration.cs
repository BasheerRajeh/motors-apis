using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Entities;

namespace WebApi.Models.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> entity)
        {
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Contact).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Comments).HasMaxLength(300);
            entity.Property(e => e.UserIp).HasMaxLength(100);
        }
    }
}
