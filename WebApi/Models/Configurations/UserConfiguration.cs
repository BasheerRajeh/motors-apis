using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Entities;

namespace WebApi.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Contact).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            //entity.Property(e => e.RefreshToken).HasMaxLength(500);
            //entity.Property(e => e.ContactOtp).HasMaxLength(100);
            entity.Property(e => e.EmailOtp).HasMaxLength(100);
            entity.Property(e => e.ProfilePhoto).HasMaxLength(200);

            entity.HasIndex(x => x.UpdatedDate);
        }
    }
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.HasKey(x => new { x.UserId, x.Role });
            entity.Property(e => e.Role).HasMaxLength(20);
        }
    }
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> entity)
        {
            entity.Property(e => e.RefreshToken).HasMaxLength(500);
        }
    }
   
}
