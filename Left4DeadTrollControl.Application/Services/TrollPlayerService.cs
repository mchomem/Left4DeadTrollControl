namespace Left4DeadTrollControl.Application.Services;

public class TrollPlayerService : ITrollPlayerService
{
    private readonly ITrollPlayerRepository _trollPlayerRepository;
    private readonly IMapper _mapper;

    public TrollPlayerService(ITrollPlayerRepository trollPlayerRepository, IMapper mapper)
    {
        _trollPlayerRepository = trollPlayerRepository;
        _mapper = mapper;
    }

    public async Task<TrollPlayerDto> CreateAsync(TrollPlayerInsertDto entity)
    {
        var exists = await _trollPlayerRepository.CheckIfExists(x => x.SteamId.Equals(entity.SteamId));

        if (exists)
            throw new Exception("Troll player with the same SteamId already exists.");

        var trollPlayer = new TrollPlayer(entity.SteamId, entity.ProfileUrl, entity.Nickname, entity.Notes);
        trollPlayer.ExecuteValidations();
        var createdTrollPlayer = await _trollPlayerRepository.CreateAsync(trollPlayer);
        return _mapper.Map<TrollPlayerDto>(createdTrollPlayer);
    }

    public async Task<IEnumerable<TrollPlayerDto>> CreateRangeAsync(IEnumerable<TrollPlayerInsertDto> entities)
    {
        var trollPlayers = entities.Select(entity =>
        {
            var trollPlayer = new TrollPlayer(entity.SteamId, entity.ProfileUrl, entity.Nickname, entity.Notes);
            trollPlayer.ExecuteValidations();
            return trollPlayer;
        }).ToList();

        var createdTrollPlayers = await _trollPlayerRepository.CreateRangeAsync(trollPlayers);
        return _mapper.Map<IEnumerable<TrollPlayerDto>>(createdTrollPlayers);
    }

    public async Task<TrollPlayerDto> DeleteAsync(Guid id)
    {
        var trollPlayer = await _trollPlayerRepository.GetAsync(id);

        if (trollPlayer is null)
            throw new Exception("Troll player not found.");

        var deletedCustomer = await _trollPlayerRepository.DeleteAsync(trollPlayer);

        return _mapper.Map<TrollPlayerDto>(deletedCustomer);
    }

    public async Task<IEnumerable<TrollPlayerDto>> GetAllAsync(TrollPlayerFilter filter)
    {
        Expression<Func<TrollPlayer, bool>> expressionFilter =
            x => ((string.IsNullOrEmpty(filter.SteamId) || x.SteamId == filter.SteamId)
                 && (string.IsNullOrEmpty(filter.Nickname) || x.Nickname.Contains(filter.Nickname))
            );

        IEnumerable<TrollPlayer> customers = await _trollPlayerRepository.GetAllAsync(expressionFilter);
        return _mapper.Map<IEnumerable<TrollPlayerDto>>(customers);
    }

    public async Task<TrollPlayerDto> GetAsync(Guid id)
    {
        var user = await _trollPlayerRepository.GetAsync(id);

        if (user is null)
            throw new Exception("Troll player not found.");

        return _mapper.Map<TrollPlayerDto>(user);
    }

    public async Task<TrollPlayerDto> UpdateAsync(Guid id, TrollPlayerUpdateDto entity)
    {
        var trollPlayer = await _trollPlayerRepository.GetAsync(id);

        if (trollPlayer is null)
            throw new Exception("Troll player not found.");

        trollPlayer.Update(entity.SteamId, entity.ProfileUrl, entity.Nickname, entity.Notes);
        trollPlayer.ExecuteValidations();
        var updatedCustomer = await _trollPlayerRepository.UpdateAsync(trollPlayer);

        return _mapper.Map<TrollPlayerDto>(updatedCustomer);
    }
}
