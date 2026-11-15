
using System;
using System.Runtime.InteropServices;

namespace AutomatePdm.LiveTest;

// IObjectWithSite des ExplorerBrowser
[ComImport]
[Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IObjectWithSite
{
    [PreserveSig]
    int SetSite([MarshalAs(UnmanagedType.IUnknown)] object? site);

    [PreserveSig]
    int GetSite(ref Guid riid, out IntPtr ppvSite);
}

// IServiceProvider
[ComVisible(true)]
[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IShellServiceProvider
{
    [PreserveSig]
    int QueryService(
        ref Guid guidService,
        ref Guid riid,
        out IntPtr ppvObject);
}

// IExplorerPaneVisibility
[ComVisible(true)]
[Guid("E07010EC-BC17-44C0-97B0-46C7C95B9EDC")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IExplorerPaneVisibility
{
    [PreserveSig]
    int GetPaneState(ref Guid explorerPane, out uint state);
}

internal static class ExplorerPaneIds
{
    public static readonly Guid NavPane =
        new("CB316B22-25F7-42B8-8A09-540D23A43C2F");

    public static readonly Guid Commands =
        new("D9745868-CA5F-4A76-91CD-F5A129FBB076");

    public static readonly Guid CommandsOrganize =
        new("72E81700-E3EC-4660-BF24-3C3B7B648806");

    public static readonly Guid CommandsView =
        new("21F7C32D-EEAA-439B-BB51-37B96FD6A943");
}

internal static class ExplorerPaneState
{
    public const uint DefaultOn = 0x00001;
    public const uint DefaultOff = 0x00002;
    public const uint InitialState = 0x10000;
    public const uint Force = 0x20000;
}
