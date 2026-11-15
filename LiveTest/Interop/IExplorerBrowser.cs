using System.Runtime.InteropServices;

namespace AutomatePdm.LiveTest;

// The declaration order must match the native IExplorerBrowser vtable.
[ComImport]
[Guid("DFD3B6B5-C10C-4BE9-85F6-A66969F402F6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IExplorerBrowser
{
    void Initialize(IntPtr hwndParent, ref NativeMethods.RECT rect, IntPtr folderSettings);
    void Destroy();
    void SetRect(IntPtr deferWindowPos, ref NativeMethods.RECT rect);
    void SetPropertyBag([MarshalAs(UnmanagedType.LPWStr)] string propertyBag);
    void SetEmptyText([MarshalAs(UnmanagedType.LPWStr)] string emptyText);
    void SetFolderSettings(ref FOLDERSETTINGS folderSettings);
    void Advise(IntPtr explorerBrowserEvents, out uint cookie);
    void Unadvise(uint cookie);
    void SetOptions(uint flags);
    void GetOptions(out uint flags);
    void BrowseToIDList(IntPtr pidl, uint flags);
    void BrowseToObject([MarshalAs(UnmanagedType.IUnknown)] object punk, uint flags);
    void FillFromObject([MarshalAs(UnmanagedType.IUnknown)] object? punk, uint flags);
    void RemoveAll();
    IntPtr GetCurrentView(ref Guid iid);
}

[StructLayout(LayoutKind.Sequential)]
internal struct FOLDERSETTINGS
{
    public uint ViewMode;
    public uint Flags;
}
