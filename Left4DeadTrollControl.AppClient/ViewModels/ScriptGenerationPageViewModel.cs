namespace Left4DeadTrollControl.AppClient.ViewModels;

public class ScriptGenerationPageViewModel : INotifyPropertyChanged
{
    private readonly ITrollPlayerService _trollPlayerService;
    private readonly IConfiguration _configuration;
    private double _progressValue;
    private string _statusText = "Waiting to start the process...";
    private string _percentageText = "0%";
    private bool _isGenerating;

    public ScriptGenerationPageViewModel(ITrollPlayerService trollPlayerService, IConfiguration configuration)
    {
        _trollPlayerService = trollPlayerService;
        _configuration = configuration;
        GenerateScriptFileCommand = new RelayCommand(async () => await GenerateScriptFileAsync(), () => !IsGenerating);
    }

    #region Properties

    public double ProgressValue
    {
        get => _progressValue;
        set
        {
            _progressValue = value;
            OnPropertyChanged();
            PercentageText = $"{value:F0}%";
        }
    }

    public string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            OnPropertyChanged();
        }
    }

    public string PercentageText
    {
        get => _percentageText;
        set
        {
            _percentageText = value;
            OnPropertyChanged();
        }
    }

    public bool IsGenerating
    {
        get => _isGenerating;
        set
        {
            _isGenerating = value;
            OnPropertyChanged();
            ((RelayCommand)GenerateScriptFileCommand).RaiseCanExecuteChanged();
        }
    }

    #endregion

    #region Command

    public ICommand GenerateScriptFileCommand { get; }

    private async Task GenerateScriptFileAsync()
    {
        try
        {
            var fileName = $"autoexec_trolls_id.cfg";
            var filePath = _configuration.GetSection("ScriptGeneration:OutputDirectory").Value!;

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ApplicationException("The output directory is not configured.");
            }

            if (PathRequiresAdmin(filePath) && !IsAdmin())
            {
                MessageBox.Show($"Script generation is only possible with administrator privileges!\n\nLocation: {Path.Combine(filePath, fileName)}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            IsGenerating = true;
            ProgressValue = 0;

            string[] steps = new[]
            {
                "Loading data...",
                "Validating information...",
                "Generating script commands...",
                "Formatting file...",
                "Saving script...",
                "Completed!"
            };

            // Passo 1: Carregando dados
            StatusText = steps[0];
            await UpdateProgress(0, 20);

            var result = await _trollPlayerService.GetAllAsync(new TrollPlayerFilter());

            // Passo 2: Validando
            StatusText = steps[1];
            await UpdateProgress(20, 40);
            await Task.Delay(300);

            // Passo 3: Gerando comandos
            StatusText = steps[2];
            await UpdateProgress(40, 60);

            var contentScriptFile = new StringBuilder();
            contentScriptFile.AppendLine("echo \"*** Automated script to kick the troll player from the match ***\"");
            contentScriptFile.AppendLine($"// Created file : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            contentScriptFile.AppendLine();

            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item.ProfileUrl))
                {
                    contentScriptFile.Append($"// Steam Profile Url: {item.ProfileUrl}");
                }

                if (!string.IsNullOrEmpty(item.Nickname))
                {
                    contentScriptFile.Append($" - Nickname: {item.Nickname}");
                }

                contentScriptFile.AppendLine();
                contentScriptFile.AppendLine($"banid 1 STEAM_1:1{item.SteamId} kick");
            }

            // Passo 4: Formatando
            StatusText = steps[3];
            await UpdateProgress(60, 80);
            await Task.Delay(300);

            // Passo 5: Salvando
            StatusText = steps[4];
            await UpdateProgress(80, 95);

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var fullPath = Path.Combine(filePath, fileName);

            using (var streamWriter = new StreamWriter(fullPath, false))
            {
                await streamWriter.WriteAsync(contentScriptFile.ToString());
            }

            // Passo 6: Concluído
            StatusText = steps[5];
            await UpdateProgress(95, 100);

            MessageBox.Show($"Script generated successfully!\n\nLocation: {Path.Combine(filePath, fileName)}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating script: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            StatusText = "Error generating script.";
            ProgressValue = 0;
        }
        finally
        {
            IsGenerating = false;
        }
    }

    private async Task UpdateProgress(double from, double to)
    {
        for (double i = from; i <= to; i++)
        {
            ProgressValue = i;
            await Task.Delay(15);
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

    private static bool IsAdmin()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        var isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

        return isAdmin;
    }

    private static bool PathRequiresAdmin(string path)
    {
        var isRequired = path.StartsWith(@"C:\", StringComparison.OrdinalIgnoreCase) ||
           path.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), StringComparison.OrdinalIgnoreCase);

        return isRequired;
    }
}
