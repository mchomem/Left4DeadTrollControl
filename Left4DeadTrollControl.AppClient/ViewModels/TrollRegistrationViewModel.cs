namespace Left4DeadTrollControl.AppClient.ViewModels;

public class TrollRegistrationViewModel : INotifyPropertyChanged
{
    private readonly ITrollPlayerService _trollPlayerService;

    public TrollRegistrationViewModel(ITrollPlayerService trollPlayerService)
    {
        _trollPlayerService = trollPlayerService;
        SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
        ClearCommand = new RelayCommand(Clear);
    }

    #region Properties

    private string _steamId;
    public string SteamId
    {
        get => _steamId;
        set
        {
            _steamId = value;
            OnPropertyChanged();
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }

    private string _profileUrl;
    public string ProfileUrl
    {
        get => _profileUrl;
        set
        {
            _profileUrl = value;
            OnPropertyChanged();
        }
    }

    private string _nickname;
    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            OnPropertyChanged();
        }
    }

    private string _notes;
    public string Notes
    {
        get => _notes;
        set
        {
            _notes = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public ICommand SaveCommand { get; }
    public ICommand ClearCommand { get; }

    private bool CanSave() => !string.IsNullOrWhiteSpace(SteamId);

    private async Task SaveAsync()
    {
        try
        {
            var trollPlayerDto = new TrollPlayerInsertDto
            {
                SteamId = SteamId,
                ProfileUrl = ProfileUrl,
                Nickname = Nickname,
                Notes = Notes
            };

            await _trollPlayerService.CreateAsync(trollPlayerDto);

            MessageBox.Show("Troll registered successfully!", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error registering: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Clear()
    {
        SteamId = string.Empty;
        ProfileUrl = string.Empty;
        Nickname = string.Empty;
        Notes = string.Empty;
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
