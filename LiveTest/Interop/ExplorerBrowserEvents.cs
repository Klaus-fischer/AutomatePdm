namespace AutomatePdm.LiveTest;

using System;
using System.Runtime.InteropServices;
using System.Text;

[ComImport]
[Guid("361BBDC7-E6EE-4E13-BE58-58E2240C810F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IExplorerBrowserEvents
{
    [PreserveSig]
    int OnNavigationPending(IntPtr pidlFolder);

    [PreserveSig]
    int OnViewCreated(IShellView psv);

    [PreserveSig]
    int OnNavigationComplete(IntPtr pidlFolder);
}

[ComImport]
[Guid("000214E3-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellView
{
}

internal static class Shell32
{
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SHGetPathFromIDList(
        IntPtr pidl,
        StringBuilder pszPath);

    public static string? PidlToPath(IntPtr pidl)
    {
        const int MAX_PATH = 260;

        var path = new StringBuilder(MAX_PATH);

        return Shell32.SHGetPathFromIDList(pidl, path)
            ? path.ToString()
            : null;
    }

}

public class ExplorerBrowserEvents : IExplorerBrowserEvents
{
    public event Action<string?>? FolderChanged;

    public uint Cookie { get; set; }

    public int OnNavigationPending(IntPtr pidlFolder)
    {
        return 0; // S_OK
    }

    public int OnViewCreated(IShellView psv)
    {
        return 0; // S_OK
    }

    public int OnNavigationComplete(IntPtr pidlFolder)
    {
        var path = Shell32.PidlToPath(pidlFolder);

        if (!string.IsNullOrEmpty(path))
        {
            FolderChanged?.Invoke(path);
        }

        return 0; // S_OK
    }
}
