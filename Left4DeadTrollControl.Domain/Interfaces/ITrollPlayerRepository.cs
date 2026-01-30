namespace Left4DeadTrollControl.Domain.Interfaces;

public interface ITrollPlayerRepository
{
    public Task<TrollPlayer> CreateAsync(TrollPlayer entity);
    public Task<TrollPlayer> DleteAsync(TrollPlayer entity);
    public Task<TrollPlayer> UpdateAsync(TrollPlayer entity);
    public Task<TrollPlayer> GetAsync(Guid id);
    public Task<TrollPlayer> GetAsync(Expression<Func<TrollPlayer, bool>> filter, IEnumerable<Expression<Func<TrollPlayer, object>>>? includes = null);
    public Task<IEnumerable<TrollPlayer>> GetAllAsync(Expression<Func<TrollPlayer, bool>> filter, IEnumerable<Expression<Func<TrollPlayer, object>>>? includes = null);
    public Task<bool> CheckIfExists(Expression<Func<TrollPlayer, bool>> filter);
}
