using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.Persistence.Repository;

namespace WebApi.Persistence
{
    public class UnitOfWork : IDisposable
    {
        private readonly AppDbContext _context;

        public UserRepository Users { get; private set; }
        public ArticleRepository Articles { get; private set; }
        public BookingRepository Bookings { get; private set; }
        public CarBrandRepository Brands { get; private set; }
        public CarRepository Cars { get; private set; }
        public CarReviewRepository Reviews { get; private set; }
        public ContactSubmissionRepository ContactSubmissions { get; private set; }
        public ServiceRepository Services { get; private set; }
        public TestimonialRepository Testimonials { get; private set; }
        public MessageRepository Message{ get; private set; }

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            var logger = loggerFactory.CreateLogger("uow logs");
            Users = new UserRepository(context, logger);
            Articles = new ArticleRepository(context, logger);
            Bookings = new BookingRepository(context, logger);
            Brands = new CarBrandRepository(context, logger);
            Cars = new CarRepository(context, logger);
            Reviews = new CarReviewRepository(context, logger);
            ContactSubmissions = new ContactSubmissionRepository(context, logger);
            Services = new ServiceRepository(context, logger);
            Testimonials = new TestimonialRepository(context, logger);
            Message = new MessageRepository(context, logger);
        }

        public Task CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
