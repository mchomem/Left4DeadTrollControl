namespace Left4DeadTrollControl.AppClient.Pages;

public partial class ScriptGenerationPage : UserControl
{
    public ScriptGenerationPage(ScriptGenerationPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
