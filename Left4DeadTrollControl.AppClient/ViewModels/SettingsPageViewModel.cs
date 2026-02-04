namespace Left4DeadTrollControl.AppClient.ViewModels;

public class SettingsPageViewModel : INotifyPropertyChanged
{
    private readonly string _appSettingsPath;
    private string _outputDirectory = string.Empty;

    public SettingsPageViewModel()
    {
        _appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        SaveSettingsCommand = new RelayCommand(async () => await SaveSettingsAsync(), CanSave);
        _ = LoadSettingsAsync();
    }

    #region Properties

    public string OutputDirectory
    {
        get => _outputDirectory;
        set
        {
            _outputDirectory = value;
            OnPropertyChanged();
            ((RelayCommand)SaveSettingsCommand).RaiseCanExecuteChanged();
        }
    }

    #endregion

    #region Command

    public ICommand SaveSettingsCommand { get; }

    private bool CanSave() => !string.IsNullOrWhiteSpace(OutputDirectory);

    public async Task LoadSettingsAsync()
    {
        try
        {
            if (!File.Exists(_appSettingsPath))
            {
                return;
            }

            var jsonContent = await File.ReadAllTextAsync(_appSettingsPath);
            var optionsRead = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            SettingsDataWrapper settings = JsonSerializer.Deserialize<SettingsDataWrapper>(jsonContent, optionsRead)!;

            OutputDirectory = settings.ScriptGeneration.OutputDirectory;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task SaveSettingsAsync()
    {
        try
        {
            var jsonContent = await File.ReadAllTextAsync(_appSettingsPath);
            var optionsRead = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            SettingsDataWrapper settings = JsonSerializer.Deserialize<SettingsDataWrapper>(jsonContent, optionsRead)!;
            settings.ScriptGeneration.OutputDirectory = OutputDirectory;

            var optionWrite = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(settings, optionWrite);
            await File.WriteAllTextAsync(_appSettingsPath, jsonString);

            MessageBox.Show("Saved successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error to save settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}

public class SettingsDataWrapper
{
    public ScriptGeneration ScriptGeneration { get; set; } = new();
}

public class ScriptGeneration
{
    public string OutputDirectory { get; set; } = string.Empty;
}
