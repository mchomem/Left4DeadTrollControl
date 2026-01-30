namespace Left4DeadTrollControl.AppClient.Pages;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
        LoadSettings();
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

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CfgDirectoryTextBox.Text))
        {
            MessageBox.Show(
                "Please select a directory before saving.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (!System.IO.Directory.Exists(CfgDirectoryTextBox.Text))
        {
            var result = MessageBox.Show(
                "The specified directory does not exist. Do you want to save anyway?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        MessageBox.Show(
            "Settings saved successfully!\n\n(Persistence functionality will be implemented)",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        CfgDirectoryTextBox.Clear();
    }

    private void LoadSettings()
    {
        // Logic to load saved settings will be implemented here
    }
}
