using Microsoft.EntityFrameworkCore;
using WebApi.Common.Models;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.ViewModels.Filters;

namespace WebApi.Persistence.Repository
{
    public class TestimonialRepository : GenericRepository<Testimonial>
    {
        public TestimonialRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }

        internal Task<Testimonial?> GetDetails(int id)
        {
            var query = dbSet.AsQueryable();

            return query.FirstOrDefaultAsync(x => x.Id == id);
        }

        internal async Task<DataPageModel<Testimonial>> Filter(FilterBase filter)
        {
            var page = new DataPageModel<Testimonial>();
            var query = dbSet
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
        private static IQueryable<Testimonial> BuildFilterQuery(FilterBase filter, IQueryable<Testimonial> query)
        {
            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                query = filter.Desc ?
                query.OrderByDescending(x => EF.Property<Testimonial>(x, filter.OrderBy)) :
                query.OrderBy(x => EF.Property<Testimonial>(x, filter.OrderBy));
            }

            return query;
        }
    }
}