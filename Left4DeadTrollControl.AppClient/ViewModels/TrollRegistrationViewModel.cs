namespace Left4DeadTrollControl.AppClient.ViewModels;

public class TrollRegistrationViewModel : INotifyPropertyChanged
{
    private readonly ITrollPlayerService _trollPlayerService;
    private Guid? _currentTrollId;

    public TrollRegistrationViewModel(ITrollPlayerService trollPlayerService)
    {
        _trollPlayerService = trollPlayerService;
        SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
        BackToListCommand = new RelayCommand(BackToList);
    }

    #region Properties

    private bool _isEditMode;
    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            _isEditMode = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    public string PageTitle => IsEditMode ? "Edit Troll" : "Troll Registration";

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
            CountCharactersTextNotes = value?.Length ?? 0;
        }
    }

    private int _countCharactersTextNotes;
    public int CountCharactersTextNotes
    {
        get => _countCharactersTextNotes;
        set
        {
            _countCharactersTextNotes = value;
            OnPropertyChanged();
        }
    }

    private string _formattedCreatedAt;
    public string FormattedCreatedAt
    {
        get => _formattedCreatedAt;
        set
        {
            _formattedCreatedAt = value;
            OnPropertyChanged();
        }
    }

    private string _formattedUpdatedAt;
    public string FormattedUpdatedAt
    {
        get => _formattedUpdatedAt;
        set
        {
            _formattedUpdatedAt = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public ICommand SaveCommand { get; }
    public ICommand BackToListCommand { get; }

    private bool CanSave() => !string.IsNullOrWhiteSpace(SteamId);

    public async Task LoadTrollForEdit(Guid trollId)
    {
        try
        {
            var troll = await _trollPlayerService.GetAsync(trollId);

            if (troll != null)
            {
                _currentTrollId = trollId;
                IsEditMode = true;
                SteamId = troll.SteamId;
                ProfileUrl = troll.ProfileUrl;
                Nickname = troll.Nickname;
                Notes = troll.Notes;
                FormattedCreatedAt = troll.FormattedCreatedAt;
                FormattedUpdatedAt = troll.FormattedUpdatedAt;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading troll data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            if (IsEditMode && _currentTrollId.HasValue)
            {
                var updatedTrollPlayer = new TrollPlayerUpdateDto
                {
                    Id = _currentTrollId.Value,
                    SteamId = SteamId,
                    ProfileUrl = ProfileUrl,
                    Nickname = Nickname,
                    Notes = Notes
                };

                await _trollPlayerService.UpdateAsync(_currentTrollId.Value, updatedTrollPlayer);

                MessageBox.Show("Troll updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Create a new record.
                var newTrollPlayer = new TrollPlayerInsertDto
                {
                    SteamId = SteamId,
                    ProfileUrl = ProfileUrl,
                    Nickname = Nickname,
                    Notes = Notes
                };

                await _trollPlayerService.CreateAsync(newTrollPlayer);

                MessageBox.Show("Troll registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            BackToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BackToList()
    {
        if(System.Windows.Application.Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.ContentArea.Content = App.GetService<TrollListPage>();
        }
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
