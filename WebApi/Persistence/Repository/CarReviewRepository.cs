using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Entities;

namespace WebApi.Persistence.Repository
{
    public class CarReviewRepository : GenericRepository<CarReview>
    {
        public CarReviewRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }

        internal async Task<IEnumerable<CarReview>> ByCarId(int carId)
        {
            return await dbSet
                .Where(x => x.CarId == carId)
                .OrderByDescending(x => x.Id)
                .Take(500)
                .ToListAsync();
        }
    }
}
