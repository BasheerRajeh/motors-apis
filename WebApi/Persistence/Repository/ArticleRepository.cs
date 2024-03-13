using WebApi.Models.Entities;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Common.Models;
using WebApi.ViewModels.Filters;

namespace WebApi.Persistence.Repository
{
    public class ArticleRepository : GenericRepository<Article>
    {

        public ArticleRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }
        internal async Task<DataPageModel<Article>> Filter(FilterBase filter)
        {
            var page = new DataPageModel<Article>();
            var query = dbSet
                .Include(x => x.Images)
                .Where(x=>x.Active)
                .AsQueryable();
            query = BuildFilterQuery(filter, query);
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
        private static IQueryable<Article> BuildFilterQuery(FilterBase filter, IQueryable<Article> query)
        {
            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                query = filter.Desc ?
                query.OrderByDescending(x => EF.Property<Article>(x, filter.OrderBy)) :
                query.OrderBy(x => EF.Property<Article>(x, filter.OrderBy));
            }

            return query;
        }

        internal Task<Article?> GetDetails(int id)
        {
            return dbSet.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}