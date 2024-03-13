using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Entities;

namespace WebApi.Persistence.Repository
{
    public class MessageRepository : GenericRepository<Message>
    {
        public MessageRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        internal Task<Message?> GetDetails(int id)
        {
            return dbSet.Include(x => x.ContactSubmission).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
