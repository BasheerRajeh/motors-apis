using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using WebApi.Common.Models;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.ViewModels.Filters;

namespace WebApi.Persistence.Repository
{
    public class CarRepository : GenericRepository<Car>
    {
        public CarRepository(AppDbContext _context, ILogger logger) : base(_context, logger)
        {
        }


        internal async Task<DataPageModel<Car>> Filter(CarFilter filter)
        {
            var page = new DataPageModel<Car>();
            var query = dbSet
                .Include(x => x.Images)
                .Where(x => x.Active);
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

        internal Task<Car?> GetDetails(int id)
        {
            var query = dbSet
                .Include(x => x.Images)
                .Include(x => x.Brand);

            return query.FirstOrDefaultAsync(x => x.Id == id);
        }

        private static IQueryable<Car> BuildFilterQuery(CarFilter filter, IQueryable<Car> query)
        {

            var parameterExpr = Expression.Parameter(typeof(Car), "p");

            var expressionList = new List<Expression>();

            
            if (filter.MaxPrice.HasValue)
            {
                var maxPriceProperty = Expression.Property(parameterExpr, nameof(Car.Price));
                var maxPriceValue = Expression.Constant(filter.MaxPrice.Value);
                var maxPriceCondition = Expression.LessThanOrEqual(maxPriceProperty, maxPriceValue);
                expressionList.Add(maxPriceCondition);
            }

            if (filter.MinPrice.HasValue)
            {
                var minPriceProperty = Expression.Property(parameterExpr, nameof(Car.Price));
                var minPriceValue = Expression.Constant(filter.MinPrice.Value);
                var minPriceCondition = Expression.GreaterThanOrEqual(minPriceProperty, minPriceValue);
                expressionList.Add(minPriceCondition);
            }

            if (filter.BrandId.HasValue)
            {
                var brandIdProperty = Expression.Property(parameterExpr, nameof(Car.BrandId));
                var brandIdValue = Expression.Constant(filter.BrandId.Value);
                var brandIdCondition = Expression.Equal(brandIdProperty, brandIdValue);
                expressionList.Add(brandIdCondition);
            }


            if (filter.BestSeller.HasValue)
            {
                var bestSellerProperty = Expression.Property(parameterExpr, nameof(Car.BestSeller));
                var bestSellerValue = Expression.Constant(filter.BestSeller.Value);
                var bestSellerCondition = Expression.Equal(bestSellerProperty, bestSellerValue);
                expressionList.Add(bestSellerCondition);
            }

 
            if (expressionList.Count > 0)
            {
                var andExpression = expressionList.Aggregate(Expression.AndAlso);
                var lambda = Expression.Lambda<Func<Car, bool>>(andExpression, parameterExpr);
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
                query.OrderByDescending(x => EF.Property<Car>(x, filter.OrderBy)) :
                query.OrderBy(x => EF.Property<Car>(x, filter.OrderBy));
            }

            return query;
        }
    }
}


