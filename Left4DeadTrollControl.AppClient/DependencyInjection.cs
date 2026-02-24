namespace Left4DeadTrollControl.AppClient;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddScoped<TrollRegistrationViewModel>();
        services.AddScoped<TrollListPageViewModel>();
        services.AddScoped<ScriptGenerationPageViewModel>();
        services.AddScoped<SettingsPageViewModel>();

        return services;
    }
}
