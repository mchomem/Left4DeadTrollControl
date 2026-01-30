namespace Left4DeadTrollControl.Application.ProfileMappings;

public static class ProfileMapping
{
    public static void RegisterMappings(TypeAdapterConfig config)
    {
        config.NewConfig<TrollPlayer, TrollPlayerDto>().TwoWays();
        config.NewConfig<TrollPlayer, TrollPlayerInsertDto>().TwoWays();
        config.NewConfig<TrollPlayer, TrollPlayerUpdateDto>().TwoWays();
    }
}
