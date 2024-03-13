using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using WebApi.Models;
using WebApi.ViewModels.Filters;

namespace WebApi.Persistence.Repository
{
    public class GenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        protected DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(
            AppDbContext context,
            ILogger logger)
        {
            _context = context;
            this.dbSet = context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }
        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.CountAsync(predicate);
        }
        public async Task<T?> Find(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public Task<T?> FindOne(Expression<Func<T, bool>> predicate)
        {
            return dbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindPaged(Expression<Func<T, bool>> predicate, int page, int size = 100, bool tracked = false)
        {
            return await (tracked ? dbSet : dbSet.AsNoTracking())
                .Where(predicate)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }

        public virtual async Task<T> Add(T entity)
        {
            var rslt = await dbSet.AddAsync(entity);
            return rslt.Entity;
        }
        public virtual Task<int> Delete(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).ExecuteDeleteAsync();
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }
        protected string GetPaginationAndOrderByClause(FilterBase filter, string defaulOrderBy = "UpdatedDate")
        {
            var orderByClause = $"ORDER BY {defaulOrderBy} DESC";
            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                if(filter.OrderBy.Contains(" ")|| filter.OrderBy.Contains("--"))
                {
                    throw new Exception($"'{filter.OrderBy}' is not a valid value for 'OrderBy' property.");
                }
                orderByClause = $"ORDER BY {filter.OrderBy} {(filter.Desc ? "DESC" : "")}";
            }

            var offSet = filter.PageSize * filter.Page;
            orderByClause += $@"
OFFSET {offSet} ROWS
FETCH  NEXT {filter.PageSize} ROWS ONLY";

            return orderByClause;
        }
        protected async Task<IEnumerable<T>> ExecuteRawSql(List<SqlParameter> paramsList, string query)
        {
            _logger.LogDebug(query);
            var data = await dbSet.FromSqlRaw(query, paramsList.ToArray()).ToListAsync();
            return data;
        }
    }
}
