using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models.Entities;

namespace WebApi.Models.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> entity)
        {
            entity.Property(p => p.Body).IsRequired().HasMaxLength(1000);
        }
    }
}
