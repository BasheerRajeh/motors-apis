using WebApi.Models.Entities;
using WebApi.Models;
using WebApi.ViewModels.Filters;
using Microsoft.EntityFrameworkCore;
using WebApi.Common.Models;
using System.Linq.Expressions;

namespace WebApi.Persistence.Repository
{
    public class BookingRepository : GenericRepository<Booking>
    {

        public BookingRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }

        internal async Task<DataPageModel<Booking>> Filter(BookingFilter filter)
        {
            var page = new DataPageModel<Booking>();
            var query = dbSet
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

        private static IQueryable<Booking> BuildFilterQuery(BookingFilter filter, IQueryable<Booking> query)
        {

            var parameterExpr = Expression.Parameter(typeof(Booking), "p");

            var expressionList = new List<Expression>();


            if (filter.Closed.HasValue)
            {
                var closedProperty = Expression.Property(parameterExpr, nameof(Booking.Closed));
                var closedValue = Expression.Constant(filter.Closed.Value);
                var closedCondition = Expression.Equal(closedProperty, closedValue);
                expressionList.Add(closedCondition);
            }


            if (expressionList.Count > 0)
            {
                var andExpression = expressionList.Aggregate(Expression.AndAlso);
                var lambda = Expression.Lambda<Func<Booking, bool>>(andExpression, parameterExpr);
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
                query.OrderByDescending(x => EF.Property<Booking>(x, filter.OrderBy)) :
                query.OrderBy(x => EF.Property<Booking>(x, filter.OrderBy));
            }

            return query;
        }
    }
}