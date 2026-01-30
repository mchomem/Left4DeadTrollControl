namespace Left4DeadTrollControl.Application.Interfaces;

public interface ITrollPlayerService
{
    public Task<TrollPlayerDto> CreateAsync(TrollPlayerInsertDto entity);
    public Task<TrollPlayerDto> DeleteAsync(Guid id);
    public Task<TrollPlayerDto> GetAsync(Guid id);
    public Task<IEnumerable<TrollPlayerDto>> GetAllAsync(TrollPlayerFilter filter);
    public Task<TrollPlayerDto> UpdateAsync(Guid id, TrollPlayerUpdateDto entity);
}
