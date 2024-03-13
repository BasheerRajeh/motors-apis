using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models.Entities;

namespace WebApi.Models.Configurations
{
    public class ContactSubmissionConfiguration : IEntityTypeConfiguration<ContactSubmission>
    {
        public void Configure(EntityTypeBuilder<ContactSubmission> entity)
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Contact).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.UserIp).HasMaxLength(100);
            entity.Property(e => e.Message).HasMaxLength(600);
        }
    }
}
