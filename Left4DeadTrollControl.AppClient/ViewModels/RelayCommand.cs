namespace Left4DeadTrollControl.AppClient.ViewModels;

public class RelayCommand : ICommand
{
    private readonly Func<Task> _executeAsync;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
    }

    public RelayCommand(Action execute, Func<bool> canExecute = null)
        : this(() => { execute(); return Task.CompletedTask; }, canExecute)
    {
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

    public async void Execute(object parameter) => await _executeAsync();

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

// Versão genérica para comandos com parâmetros
public class RelayCommand<T> : ICommand
{
    private readonly Func<T, Task> _executeAsync;
    private readonly Func<T, bool> _canExecute;

    public RelayCommand(Func<T, Task> executeAsync, Func<T, bool> canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
    }

    public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        : this(param => { execute(param); return Task.CompletedTask; }, canExecute)
    {
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        if (parameter == null && default(T) != null)
            return false;

        if (parameter != null && !(parameter is T))
            return false;

        return _canExecute == null || _canExecute((T)parameter);
    }

    public async void Execute(object parameter)
    {
        if (CanExecute(parameter))
        {
            await _executeAsync((T)parameter);
        }
    }

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
