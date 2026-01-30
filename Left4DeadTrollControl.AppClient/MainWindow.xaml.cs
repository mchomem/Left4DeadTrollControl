namespace Left4DeadTrollControl.AppClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NavigateToHome(null, null);
    }

    private void NavigateToHome(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new HomePage();
    }

    private void NavigateToRegistration(object sender, RoutedEventArgs e)
    {
        // Resolve a página com suas dependências injetadas
        ContentArea.Content = App.GetService<TrollRegistrationPage>();
    }

    private void NavigateTolisting(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = App.GetService<TrollListPage>();
    }

    private void NavigateToProgress(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = App.GetService<ScriptGenerationPage>();
    }

    private void NavigateToSettings(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new SettingsPage();
    }

    private void NavigateToAbout(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new AboutPage();
    }
}
