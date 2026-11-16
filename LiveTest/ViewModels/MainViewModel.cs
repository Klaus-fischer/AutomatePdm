using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace AutomatePdm.LiveTest;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly Stack<string> NavigationStack = new();
    private readonly Stack<string> NavigationBacktrace = new();
    private bool isInternalUpdate;

    private string? directory;

    public MainViewModel()
    {
        this.NavigateBackCommand = new RelayCommand(
       this.NavigateBack,
       o => this.NavigationStack.Count > 0);

        this.NavigateForewardCommand = new RelayCommand(
           this.NavigateForeward,
           o => this.NavigationBacktrace.Count > 0);

        this.NavigateUpCommand = new RelayCommand(this.NavigateUp);
    }

    public string Directory
    {
        get => this.directory;
        set
        {
            if (this.directory == value)
            {
                return;
            }

            var old = this.directory;

            this.directory = value;
            if (!this.isInternalUpdate && old is not null)
            {
                this.NavigationStack.Push(old);
                this.NavigationBacktrace.Clear();
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Directory)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand NavigateBackCommand { get; }

    public ICommand NavigateForewardCommand { get; }

    public ICommand NavigateUpCommand { get; }

    private void NavigateUp(object? obj)
    {
        var parentDirectory = Path.GetDirectoryName(this.Directory) ?? string.Empty;
        if (parentDirectory == string.Empty || parentDirectory == this.Directory)
        {
            return;
        }

        this.Directory = parentDirectory;
    }

    private void NavigateForeward(object? obj)
    {
        if (!this.NavigationBacktrace.TryPop(out var previous) ||
            this.Directory is null)
        {
            return;
        }

        this.isInternalUpdate = true;
        var old = this.directory;
        this.Directory = previous;
        this.isInternalUpdate = false;

        this.NavigationStack.Push(old);
    }

    private void NavigateBack(object? obj)
    {
        if (!this.NavigationStack.TryPop(out var previous))
        {
            return;
        }

        this.isInternalUpdate = true;
        var old = this.directory;
        this.Directory = previous;
        this.isInternalUpdate |= false;

        this.NavigationBacktrace.Push(old);
    }
}
