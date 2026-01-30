namespace Left4DeadTrollControl.Infrastructure.Persistence.Repositories;

public class TrollPlayerRepository : ITrollPlayerRepository
{
    private readonly IRepositoryBase<TrollPlayer> _repositoryBase;

    public TrollPlayerRepository(IRepositoryBase<TrollPlayer> repositoryBase)
    {
        _repositoryBase = repositoryBase;
    }

    public async Task<bool> CheckIfExists(Expression<Func<TrollPlayer, bool>> filter)
    {
        var exists = await _repositoryBase.CheckIfExists(filter);
        return exists;
    }

    public async Task<TrollPlayer> CreateAsync(TrollPlayer entity)
    {
        var trollPlayer = await _repositoryBase.CreateAsync(entity);
        return trollPlayer;
    }

    public async Task<TrollPlayer> DleteAsync(TrollPlayer entity)
    {
        var trollPlayer = await _repositoryBase.DeleteAsync(entity);
        return trollPlayer;
    }

    public async Task<IEnumerable<TrollPlayer>> GetAllAsync(Expression<Func<TrollPlayer, bool>> filter, IEnumerable<Expression<Func<TrollPlayer, object>>>? includes = null)
    {
        var trollsPlayes = await _repositoryBase.GetAllAsync(filter, includes);
        return trollsPlayes;
    }

    public async Task<TrollPlayer> GetAsync(Guid id)
    {
        var trollPlayer = await _repositoryBase.GetAsync(id);
        return trollPlayer;
    }

    public async Task<TrollPlayer> GetAsync(Expression<Func<TrollPlayer, bool>> filter, IEnumerable<Expression<Func<TrollPlayer, object>>>? includes = null)
    {
        var trollPlayer = await _repositoryBase.GetAsync(filter, includes);
        return trollPlayer;
    }

    public async Task<TrollPlayer> UpdateAsync(TrollPlayer entity)
    {
        var trollPlayer = await _repositoryBase.UpdateAsync(entity);
        return trollPlayer;
    }
}
