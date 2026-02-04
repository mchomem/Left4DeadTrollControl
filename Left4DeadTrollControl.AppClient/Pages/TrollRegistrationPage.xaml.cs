namespace Left4DeadTrollControl.AppClient.Pages;

public partial class TrollRegistrationPage : UserControl
{
    public TrollRegistrationPage(TrollRegistrationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    // Keeps only UI logic that cannot be in the ViewModel
    private void SteamIdTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string text)
    {
        var result = Regex.IsMatch(text, "^[0-9]+$");
        return result;
    }
}
