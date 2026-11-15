using System.ComponentModel;
using System.IO;

namespace AutomatePdm.LiveTest;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private string directory;

    public string Directory
    {
        get => directory;
        set
        {
            directory = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Directory)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
