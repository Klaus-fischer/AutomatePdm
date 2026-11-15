using System.IO;

namespace AutomatePdm.LiveTest;

public sealed class MainViewModel
{
    public string InitialDirectory { get; }

    public MainViewModel(string? directoryArgument)
    {
        var candidate = string.IsNullOrWhiteSpace(directoryArgument)
            ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            : directoryArgument.Trim().Trim('"');

        InitialDirectory = Path.GetFullPath(candidate);
    }
}
