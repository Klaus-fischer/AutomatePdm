# ExplorerBrowserWpf

A minimal .NET 8 WPF sample hosting the native Windows Shell `IExplorerBrowser` view.

## Run

```powershell
dotnet run -- "C:\Projects\MyAddin"
```

The first command-line argument is used as the initial directory. If omitted, Documents is used.

## Use in XAML

```xml
<controls:ExplorerBrowserHost
    InitialDirectory="{Binding InitialDirectory}" />
```

The control also exposes `Navigate(string directory)` for imperative navigation when needed.

## Notes

- Windows desktop only. Build and run on Windows.
- COM activation and Shell navigation happen on the WPF UI thread (STA).
- The sample uses the native Shell view, not the complete Explorer application window.
- `EBO_SHOWFRAMES` enables the standard ExplorerBrowser panes. The host does not reproduce the standalone Explorer window's complete command/address bar.
- The initial implementation intentionally keeps the MVVM surface small: the ViewModel supplies the initial directory; the native Shell view owns file selection, context menus, drag/drop and navigation.
- The code is a starting implementation and should be tested on the target Windows versions and with the Shell extensions installed on the development machine.
