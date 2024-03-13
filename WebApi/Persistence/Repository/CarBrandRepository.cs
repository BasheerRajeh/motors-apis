using WebApi.Models;
using WebApi.Models.Entities;

namespace WebApi.Persistence.Repository
{
    public class CarBrandRepository : GenericRepository<CarBrand>
    {
        public CarBrandRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }
    }
}