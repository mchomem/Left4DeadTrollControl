namespace Left4DeadTrollControl.AppClient;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        // Registra apenas ViewModels (camada de Presentation)
        services.AddTransient<TrollRegistrationViewModel>();
        services.AddTransient<TrollListPageViewModel>();
        services.AddTransient<ScriptGenerationPageViewModel>();
        services.AddTransient<SettingsPageViewModel>();

        return services;
    }
}
