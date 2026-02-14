namespace Left4DeadTrollControl.AppClient.ViewModels;

public class TrollRegistrationViewModel : INotifyPropertyChanged
{
    private readonly ITrollPlayerService _trollPlayerService;
    private Guid? _currentTrollId;
    private static int _instanceCount = 0;
    private readonly int _instanceId;

    public TrollRegistrationViewModel(ITrollPlayerService trollPlayerService)
    {
        _trollPlayerService = trollPlayerService;
        SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
        ClearCommand = new RelayCommand(Clear);
        _instanceId = System.Threading.Interlocked.Increment(ref _instanceCount);
        System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] CRIADO");
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

    #endregion

    #region Commands

    public ICommand SaveCommand { get; }
    public ICommand ClearCommand { get; }

    private bool CanSave() => !string.IsNullOrWhiteSpace(SteamId);

    public async Task LoadTrollForEdit(Guid trollId)
    {
        System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] LoadTrollForEdit chamado com ID: {trollId}");
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
                System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] Troll carregado com sucesso. IsEditMode={IsEditMode}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] ERRO ao carregar: {ex.Message}");
            MessageBox.Show($"Error loading troll data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SaveAsync()
    {
        System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] SaveAsync chamado. IsEditMode={IsEditMode}, CurrentTrollId={_currentTrollId}");
        try
        {
            if (IsEditMode && _currentTrollId.HasValue)
            {
                System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] Modo EDIÇÃO - Atualizando troll {_currentTrollId}");
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
                System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] Modo CRIAÇÃO - Criando novo troll");
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

            Clear();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[TrollRegistrationViewModel #{_instanceId}] ERRO ao salvar: {ex.Message}");
            MessageBox.Show($"Error saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Clear()
    {
        _currentTrollId = null;
        IsEditMode = false;
        SteamId = string.Empty;
        ProfileUrl = string.Empty;
        Nickname = string.Empty;
        Notes = string.Empty;
        CountCharactersTextNotes = 0;
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
