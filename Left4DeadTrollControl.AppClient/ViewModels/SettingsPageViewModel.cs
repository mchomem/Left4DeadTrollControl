using System.Text.Json;

namespace Left4DeadTrollControl.AppClient.ViewModels;

public class SettingsPageViewModel : INotifyPropertyChanged
{
    private readonly string _appSettingsPath;
    private readonly IConfiguration _configuration;
    private string _outputDirectory = string.Empty;

    public SettingsPageViewModel(IConfiguration configuration)
    {
        _appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        _configuration = configuration;

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

            using (JsonDocument document = JsonDocument.Parse(jsonContent))
            {
                var root = document.RootElement;

                if (root.TryGetProperty("ScriptGeneration", out JsonElement scriptGeneration))
                {
                    if (scriptGeneration.TryGetProperty("OutputDirectory", out JsonElement outputDirectory))
                    {
                        OutputDirectory = outputDirectory.GetString() ?? string.Empty;
                    }
                }
            }
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

            using (JsonDocument document = JsonDocument.Parse(jsonContent))
            {
                var root = document.RootElement.Clone();
                var jsonObject = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(root);

                var updatedScriptGenerationConnection = new Dictionary<string, object>
                {
                    { "OutputDirectory", OutputDirectory },
                };

                jsonObject["ScriptGeneration"] = JsonDocument.Parse(JsonSerializer.Serialize(updatedScriptGenerationConnection)).RootElement;

                var options = new JsonSerializerOptions { WriteIndented = true };
                string updatedJson = JsonSerializer.Serialize(jsonObject, options);
                await File.WriteAllTextAsync(_appSettingsPath, updatedJson);
            }

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
