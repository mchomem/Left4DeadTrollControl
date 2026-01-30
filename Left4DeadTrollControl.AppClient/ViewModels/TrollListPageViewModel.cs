namespace Left4DeadTrollControl.AppClient.ViewModels;

public class TrollListPageViewModel : INotifyPropertyChanged
{
    private readonly ITrollPlayerService _trollPlayerService;

    public TrollListPageViewModel(ITrollPlayerService trollPlayerService)
    {
        _trollPlayerService = trollPlayerService;
        ListCommand = new RelayCommand(async () => await ListAsync());
        DeleteCommand = new RelayCommand<Guid>(async (id) => await DeleteAsync(id));
        
        // Load data on initialization
        _ = ListAsync();
    }

    #region Properties

    private ObservableCollection<TrollPlayerDto> _trolls = new();
    public ObservableCollection<TrollPlayerDto> Trolls
    {
        get => _trolls;
        set
        {
            _trolls = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalRecords));
            OnPropertyChanged(nameof(HasRecords));
        }
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
        }
    }

    public int TotalRecords => Trolls?.Count ?? 0;
    public bool HasRecords => TotalRecords > 0;

    #endregion

    #region Commands

    public ICommand ListCommand { get; }
    public ICommand DeleteCommand { get; }

    private async Task ListAsync()
    {
        try
        {
            var filter = new TrollPlayerFilter
            {
                SteamId = SearchText
            };

            var result = await _trollPlayerService.GetAllAsync(filter);
            
            Trolls = new ObservableCollection<TrollPlayerDto>(result);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error listing: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task DeleteAsync(Guid id)
    {
        try
        {
            var confirmResult = MessageBox.Show(
                "Are you sure you want to delete this troll?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.Yes)
            {
                await _trollPlayerService.DeleteAsync(id);
                await ListAsync(); // Reload list
                
                MessageBox.Show("Troll deleted successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
