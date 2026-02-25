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

    public void NavigateToRegistration()
    {
        ContentArea.Content = App.GetService<TrollRegistrationPage>();
    }

    private void NavigateTolisting(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = App.GetService<TrollListPage>();
    }

    private void ImportExport_Click(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = App.GetService<ImportExportPage>();
    }

    private void NavigateToProgress(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = App.GetService<ScriptGenerationPage>();
    }

    private void NavigateToSettings(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = App.GetService<SettingsPage>();
    }

    private void NavigateToAbout(object sender, RoutedEventArgs e)
    {
        ContentArea.Content = new AboutPage();
    }

    public async void NavigateToRegistrationWithId(Guid id)
    {
        var registrationPage = App.GetService<TrollRegistrationPage>();
        var viewModel = registrationPage.DataContext as TrollRegistrationViewModel;
        
        if (viewModel != null)
        {
            await viewModel.LoadTrollForEdit(id);
        }
        
        ContentArea.Content = registrationPage;
    }

    
}
