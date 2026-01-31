namespace Left4DeadTrollControl.AppClient.Pages;

public partial class SettingsPage : UserControl
{
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Select the Left4Dead CFG directory",
            InitialDirectory = !string.IsNullOrWhiteSpace(CfgDirectoryTextBox.Text) 
                ? CfgDirectoryTextBox.Text 
                : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        };

        if (dialog.ShowDialog() == true)
        {
            CfgDirectoryTextBox.Text = dialog.FolderName;
        }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        CfgDirectoryTextBox.Clear();
    }
}
