using Microsoft.EntityFrameworkCore;
using WebApi.Common.Models;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.ViewModels.Filters;

namespace WebApi.Persistence.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }
        internal async Task<DataPageModel<User>> Filter(FilterBase filter)
        {
            var page = new DataPageModel<User>();
            var query = dbSet.AsQueryable();
            page.Count = await query.CountAsync();
            if (page.Count > 0)
            {
                var pageCount = (float)page.Count / filter.PageSize;
                page.PageCount = Math.Ceiling(pageCount);
                var offSet = filter.PageSize * filter.Page;
                page.Data = await query
                    .Skip(offSet)
                    .Take(filter.PageSize)
                    .ToListAsync();
            }

            return page;
        }
    }
}
