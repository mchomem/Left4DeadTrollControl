namespace Left4DeadTrollControl.AppClient.ViewModels;

public class ImportExportPageViewModel : INotifyPropertyChanged
{
    private readonly ITrollPlayerService _trollPlayerService;
    private double _progressValue;
    private string _statusText = "Waiting to start the process...";
    private string _percentageText = "0%";
    private bool _isGenerating;

    public ImportExportPageViewModel(ITrollPlayerService trollPlayerService)
    {
        _trollPlayerService = trollPlayerService;
        ImportCommand = new RelayCommand(async () => await ImportAsync(), () => !IsGenerating);
        ExportCommand = new RelayCommand(async () => await ExportAsync(), () => !IsGenerating);
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
            ((RelayCommand)ImportCommand).RaiseCanExecuteChanged();
        }
    }

    #endregion

    #region Command

    public ICommand ImportCommand { get; }
    public ICommand ExportCommand { get; }

    private async Task ImportAsync()
    {
        try
        {
            string[] steps = new[]
            {
                "Loading file data...",
                "Checking if file is not empty...",
                "Reading content file...",
                "Adding data to system...",
                "Completed!"
            };

            IsGenerating = true;
            ProgressValue = 0;

            // Step 1: Loading file data
            StatusText = steps[0];
            await UpdateProgress(0, 20);

            var trollPlayers = new List<TrollPlayerInsertDto>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file to import";
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() ?? false)
            {
                string filePath = openFileDialog.FileName;

                // Step 2: Checking if file is not empty
                StatusText = steps[1];
                await UpdateProgress(20, 40);

                // Read all lines at once
                string[] allLines = await File.ReadAllLinesAsync(filePath);

                if (allLines.Length == 0)
                {
                    MessageBox.Show("The csv file is empty.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Step 3: Reading content file
                StatusText = steps[2];
                await UpdateProgress(40, 60);

                for (int i = 0; i < allLines.Length; i++)
                {
                    string line = allLines[i];

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Skip header (first non-empty line)
                    if (i == 0)
                        continue;

                    var columns = line.Split(',');

                    // Check format and validate csv file content
                    if (columns.Length != 4)
                    {
                        throw new ApplicationException($"The csv file format is invalid. The error is in line {i + 1}. Each line must contain exactly 4 columns: SteamId, ProfileUrl, Nickname, Notes.");
                    }

                    var trollPlayer = new TrollPlayerInsertDto
                    {
                        SteamId = columns[0].Trim(),
                        ProfileUrl = columns[1].Trim(),
                        Nickname = columns[2].Trim(),
                        Notes = columns[3].Trim()
                    };

                    trollPlayers.Add(trollPlayer);
                }

                // Step 4: Adding data to system
                StatusText = steps[3];
                await UpdateProgress(60, 80);
                await _trollPlayerService.CreateRangeAsync(trollPlayers);

                // Step 5: Finalizing import
                StatusText = steps[4];
                await UpdateProgress(80, 100);
                MessageBox.Show($"Total of {trollPlayers.Count} troll players imported to system.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsGenerating = false;
            ProgressValue = 0;
        }
    }

    private async Task ExportAsync()
    {
        try
        {
            string[] steps = new[]
            {
                "Loading data...",
                "Preparing data to file...",
                "Wrinting content to file...",
                "Completed!"
            };

            IsGenerating = true;
            ProgressValue = 0;

            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Title = "Select a folder to export records";

            if (openFolderDialog.ShowDialog() ?? false)
            {
                string folderPath = openFolderDialog.FolderName;

                // Step 1: Loading data
                StatusText = steps[0];
                await UpdateProgress(0, 20);

                var trollPlayers = await _trollPlayerService.GetAllAsync(new TrollPlayerFilter());
                var contentCSVFile = new StringBuilder();

                // Step 2: Preparing data to file
                StatusText = steps[1];
                await UpdateProgress(20, 60);

                int index = 0;
                foreach (var trollPlayer in trollPlayers)
                {
                    if (index == 0)
                        contentCSVFile.AppendLine($"{nameof(trollPlayer.SteamId)},{nameof(trollPlayer.ProfileUrl)},{nameof(trollPlayer.Nickname)},{nameof(trollPlayer.Notes)}");

                    contentCSVFile.AppendLine($"{trollPlayer.SteamId},{trollPlayer.ProfileUrl},{trollPlayer.Nickname},{trollPlayer.Notes}");
                    index++;
                }

                var fullPath = Path.Combine(folderPath, "troll-players.csv");

                // Step 3: Writing content to file
                StatusText = steps[2];
                await UpdateProgress(60, 80);
                using (var sw = new StreamWriter(fullPath))
                {
                    await sw.WriteAsync(contentCSVFile);
                }

                // Step 4: Finalizing export
                StatusText = steps[3];
                await UpdateProgress(80, 100);

                MessageBox.Show($"Total of {trollPlayers.Count()} troll players exported to file.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
}
