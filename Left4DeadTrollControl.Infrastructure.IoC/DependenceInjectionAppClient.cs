namespace Left4DeadTrollControl.Infrastructure.IoC;

public static class DependenceInjectionAppClient
{
    public static IServiceCollection AddInfrastructureAppClient(this IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext

        var baseDirectory = AppContext.BaseDirectory;
        var dbPath = Path.Combine(baseDirectory, "l4dtc.db");

        services.AddDbContext<Left4DeadTrollControlContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
        });

        #endregion

        #region Repositories

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped<ITrollPlayerRepository, TrollPlayerRepository>();

        #endregion

        #region Services

        services.AddScoped<ITrollPlayerService, TrollPlayerService>();

        #endregion

        #region Mapster

        var config = TypeAdapterConfig.GlobalSettings;
        ProfileMapping.RegisterMappings(config);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddMapster();

        #endregion

        return services;
    }
}
