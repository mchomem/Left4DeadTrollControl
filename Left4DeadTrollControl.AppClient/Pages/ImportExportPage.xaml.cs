namespace Left4DeadTrollControl.AppClient.Pages;

/// <summary>
/// Interaction logic for ImportExportPage.xaml
/// </summary>
public partial class ImportExportPage : UserControl
{
    public ImportExportPage(ImportExportPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
