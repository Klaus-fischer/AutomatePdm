using System.Windows;

namespace AutomatePdm.LiveTest;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Optional: first command-line argument is the initial directory.
        string? argument = Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();
        DataContext = new MainViewModel(argument);
    }
}
