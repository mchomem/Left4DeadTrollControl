namespace Left4DeadTrollControl.Infrastructure.Persistence.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly Left4DeadTrollControlContext _appDbContext;

    public RepositoryBase(Left4DeadTrollControlContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = _appDbContext.Set<TEntity>();
    }

    public async Task<bool> CheckIfExists(Expression<Func<TEntity, bool>> filter)
    {
        IQueryable<TEntity> query = _dbSet
            .AsQueryable()
            .Where(filter)
            .AsNoTracking();

        var result = await query.AnyAsync();

        return result;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
        return _appDbContext.Entry(entity).Entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _appDbContext.SaveChangesAsync();
        return _appDbContext.Entry(entity).Entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter
        , IEnumerable<Expression<Func<TEntity, object>>>? includes = null
        , IEnumerable<(Expression<Func<TEntity, object>> keySelector, bool asceding)>? orderBy = null)
    {
        IQueryable<TEntity> query = _dbSet
            .AsQueryable()
            .AsNoTracking()
            .Where(filter);

        if (includes != null)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        if (orderBy != null && orderBy.Any())
        {
            // Aplica a primeira ordenação com OrderBy/ OrderByDescending
            var firstOrder = orderBy.First();
            IOrderedQueryable<TEntity> orderedQuery = firstOrder.asceding
                ? query.OrderBy(firstOrder.keySelector)
                : query.OrderByDescending(firstOrder.keySelector);

            // Aplica as ordenações seguintes com ThenBy/ThenByDescending
            foreach (var order in orderBy.Skip(1))
            {
                orderedQuery = order.asceding
                    ? orderedQuery.ThenBy(order.keySelector)
                    : orderedQuery.ThenByDescending(order.keySelector);
            }

            return await orderedQuery.ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity> GetAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity!;
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, IEnumerable<Expression<Func<TEntity, object>>>? includes = null)
    {
        IQueryable<TEntity> query = _dbSet
            .AsQueryable()
            .Where(filter)
            .AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        var entity = await query.FirstOrDefaultAsync();
        return entity!;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return _appDbContext.Entry(entity).Entity;
    }
}
