using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace AutomatePdm.LiveTest;

/// <summary>
/// Hosts the native Windows Shell ExplorerBrowser view inside WPF.
/// InitialDirectory can be set directly or bound to a ViewModel property.
/// </summary>
public sealed class ExplorerBrowserHost : HwndHost,
    IShellServiceProvider,
    IExplorerPaneVisibility
{
    private static readonly Guid ExplorerBrowserClsid =
        new("71F96385-DDD6-48D3-A0C1-AE06E8B055FB");

    public static readonly DependencyProperty DirectoryProperty =
        DependencyProperty.Register(
            nameof(Directory),
            typeof(string),
            typeof(ExplorerBrowserHost),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnInitialDirectoryChanged));

    private IntPtr _hostHwnd;
    private IExplorerBrowser? _browser;
    private ExplorerBrowserEvents? _browserEvents;
    private bool _browserInitialized;

    public ExplorerBrowserHost()
    {
        Unloaded += OnUnloaded;
    }


    public string? Directory
    {
        get => (string?)GetValue(DirectoryProperty);
        set => SetValue(DirectoryProperty, value);
    }
  
    private static void OnInitialDirectoryChanged(
        DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ExplorerBrowserHost)d;
        if (control._browserInitialized && e.NewValue is string path && path != string.Empty)
            control.Navigate(path);
    }

    /// <summary>Navigate to a filesystem parentDirectory.</summary>
    public void Navigate(string directory)
    {
        if (string.IsNullOrWhiteSpace(directory))
            throw new ArgumentException("A parentDirectory must be specified.", nameof(directory));

        if (!System.IO.Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Directory does not exist: {directory}");

        if (!_browserInitialized || _browser is null)
        {
            Directory = directory;
            return;
        }

        int hr = NativeMethods.SHParseDisplayName(
            Path.GetFullPath(directory),
            IntPtr.Zero,
            out IntPtr pidl,
            0,
            out _);

        Marshal.ThrowExceptionForHR(hr);

        try
        {
            _browser.BrowseToIDList(pidl, NativeMethods.SBSP_ABSOLUTE);
        }
        finally
        {
            NativeMethods.CoTaskMemFree(pidl);
        }
    }

    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        _hostHwnd = NativeMethods.CreateWindowEx(
            0,
            "static",
            string.Empty,
            NativeMethods.WS_CHILD |
            NativeMethods.WS_VISIBLE |
            NativeMethods.WS_CLIPCHILDREN |
            NativeMethods.WS_CLIPSIBLINGS,
            0, 0, 0, 0,
            hwndParent.Handle,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);

        if (_hostHwnd == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        try
        {
            var rect = new NativeMethods.RECT(0, 0, 0, 0);

            var browserType = Type.GetTypeFromCLSID(ExplorerBrowserClsid, throwOnError: true)!;
            _browser = (IExplorerBrowser)Activator.CreateInstance(browserType)!;

            var site = (IObjectWithSite)_browser;
            Marshal.ThrowExceptionForHR(site.SetSite(this));

            // Initialize with NULL folder settings, then explicitly apply them.
            _browser.Initialize(_hostHwnd, ref rect, IntPtr.Zero);

            _browserEvents = new ExplorerBrowserEvents();
            _browser.Advise(_browserEvents, out var cookie);
            _browserEvents.Cookie = cookie;
            _browserEvents.FolderChanged += this.OnBrowserFolderChanged;

            var settings = new FOLDERSETTINGS
            {
                ViewMode = 4, // FVM_DETAILS
                Flags = 0
            };
            //_browser.SetFolderSettings(ref settings);

            // Enable the Shell's standard panes (navigation/details/preview etc.).
            _browser.SetOptions(0x00000002); // EBO_SHOWFRAMES

            _browserInitialized = true;

            if (!string.IsNullOrWhiteSpace(Directory))
                Navigate(Directory);

            return new HandleRef(this, _hostHwnd);
        }
        catch
        {
            ReleaseBrowser();
            if (_hostHwnd != IntPtr.Zero)
            {
                NativeMethods.DestroyWindow(_hostHwnd);
                _hostHwnd = IntPtr.Zero;
            }
            throw;
        }
    }


    private void OnBrowserFolderChanged(string? directory)
    {
        if (directory is null)
        {
            return;
        }

        this.Directory = directory;
    }

    protected override void OnWindowPositionChanged(Rect rcBoundingBox)
    {
        base.OnWindowPositionChanged(rcBoundingBox);

        if (_browser is null || !_browserInitialized)
            return;

        // HwndHost layout coordinates are WPF device-independent units.
        // IExplorerBrowser.SetRect expects physical pixels.
        var dpi = VisualTreeHelper.GetDpi(this);
        var rect = new NativeMethods.RECT(
            0,
            0,
            Math.Max(0, (int)Math.Ceiling(rcBoundingBox.Width * dpi.DpiScaleX)),
            Math.Max(0, (int)Math.Ceiling(rcBoundingBox.Height * dpi.DpiScaleY)));

        _browser.SetRect(IntPtr.Zero, ref rect);
    }

    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        ReleaseBrowser();

        if (hwnd.Handle != IntPtr.Zero)
            NativeMethods.DestroyWindow(hwnd.Handle);

        _hostHwnd = IntPtr.Zero;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        // HwndHost normally calls DestroyWindowCore as part of its HWND lifetime.
        // Do not release the COM object here: WPF can unload/reload visual trees.
    }

    private void ReleaseBrowser()
    {
        if (_browser is null || !_browserInitialized)
        {
            return;
        }

        try
        {
            _browser.Unadvise(_browserEvents.Cookie);
            ((IObjectWithSite)_browser).SetSite(null);
            _browser.Destroy();
        }
        finally
        {
            _browserInitialized = false;
            Marshal.FinalReleaseComObject(_browser);
            _browserEvents = null;
            _browser = null;
        }
    }

    public int QueryService(ref Guid guidService, ref Guid riid, out nint ppvObject)
    {
        ppvObject = IntPtr.Zero;

        if (guidService == typeof(IExplorerPaneVisibility).GUID &&
            riid == typeof(IExplorerPaneVisibility).GUID)
        {
            ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IExplorerPaneVisibility));
            return 0;
        }

        return unchecked((int)0x80004002);
    }

    public int GetPaneState(ref Guid explorerPane, out uint state)
    {
        if (explorerPane == ExplorerPaneIds.Commands ||
            explorerPane == ExplorerPaneIds.CommandsOrganize ||
            explorerPane == ExplorerPaneIds.CommandsView)
        {
            state = ExplorerPaneState.DefaultOff | ExplorerPaneState.Force;

            return 0;
        }

        if (explorerPane == ExplorerPaneIds.NavPane)
        {
            state = ExplorerPaneState.DefaultOn | ExplorerPaneState.Force;
            return 0;
        }

        state = 0;
        return 0;
    }
}
