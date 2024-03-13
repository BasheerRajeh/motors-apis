using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Common.Models;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.ViewModels.Filters;

namespace WebApi.Persistence.Repository
{
    public class ServiceRepository : GenericRepository<Service>
    {
        public ServiceRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }

        internal async Task<DataPageModel<Service>> Filter(ServiceFilter filter)
        {
            var page = new DataPageModel<Service>();
            var query = dbSet
                .AsQueryable()
                .Where(x=>x.Active);
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

        private static IQueryable<Service> BuildFilterQuery(ServiceFilter filter, IQueryable<Service> query)
        {

            var parameterExpr = Expression.Parameter(typeof(Service), "p");

            var expressionList = new List<Expression>();


            if (expressionList.Count > 0)
            {
                var andExpression = expressionList.Aggregate(Expression.AndAlso);
                var lambda = Expression.Lambda<Func<Service, bool>>(andExpression, parameterExpr);
                query = query.Where(lambda);
            }


            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                //var selector = Expression.PropertyOrField(parameterExpr, filter.OrderBy);
                //var method = filter.Desc ? "OrderByDescending" : "OrderBy";
                //expression = Expression.Call(typeof(Queryable), method,
                //    new Type[] { source.ElementType, selector.Type },
                //    expression, Expression.Quote(Expression.Lambda(selector, parameterExpr)));

                query = filter.Desc ?
                query.OrderByDescending(x => EF.Property<Service>(x, filter.OrderBy)) :
                query.OrderBy(x => EF.Property<Service>(x, filter.OrderBy));
            }

            return query;
        }

        internal Task<Service?> GetDetails(int id)
        {
            var query = dbSet.AsQueryable();
                
            return query.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}