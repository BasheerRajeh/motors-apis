using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using WebApi.Models.Entities;

namespace WebApi.Models
{
    //DBCC CHECKIDENT ('Users', RESEED, 1000000);
    //script-migration init u1
    public partial class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarImage> Images { get; set; }
        public virtual DbSet<CarBrand> Brands { get; set; }
        public virtual DbSet<CarReview> Reviews { get; set; }
        public virtual DbSet<Testimonial> Testimonials { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleImage> ArticleImages { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<ContactSubmission> ContactSubmissions { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MainDatabase"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
