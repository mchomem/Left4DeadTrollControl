namespace Left4DeadTrollControl.AppClient.Pages;

public partial class TrollListPage : UserControl
{
    private const string PlaceholderText = "Search by SteamId, Nickname...";

    public TrollListPage(TrollListPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel; // Connects the ViewModel to XAML
    }

    private void SearchTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (SearchTextBox.Text == PlaceholderText)
        {
            SearchTextBox.Text = "";
            SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
        }
    }

    private void SearchTextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            SearchTextBox.Text = PlaceholderText;
            SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
        }
    }
}
