namespace Left4DeadTrollControl.AppClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private ServiceProvider _serviceProvider;
    public IConfiguration Configuration { get; private set; }

    public App()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        services.AddSingleton(Configuration);

        // 1. Register internal layers (Application, Infrastructure, Domain)
        services.AddInfrastructureAppClient(Configuration);

        // 2. Register Presentation layer (ViewModels)
        services.AddPresentationLayer();

        // 3. Register Windows/Pages if needed
        services.AddSingleton<MainWindow>();
        services.AddTransient<TrollRegistrationPage>();
        services.AddTransient<TrollListPage>();
        services.AddTransient<ScriptGenerationPage>();

        _serviceProvider = services.BuildServiceProvider();

        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Left4DeadTrollControlContext>();
                dbContext.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error initializing database: {ex.Message}",
            "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }

    public static T GetService<T>() where T : class
    {
        return ((App)Current)._serviceProvider.GetRequiredService<T>();
    }
}
