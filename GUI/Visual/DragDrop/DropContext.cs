using AutomatePdm.GUI.Visual.DragDrop;
using System.Collections;
using System.Windows.Controls;

namespace WpfDragDropManager;

public sealed class DropContext
{
    public required object Data { get; init; }

    public required DragAction Action { get; init; }

    public required ItemsControl TargetControl { get; init; }

    public IList? TargetList { get; init; }

    public IList? SourceList { get; init; }

    public ItemsControl? SourceControl { get; init; }

    /// <summary>
    /// The item before which Data should be inserted. Null means append.
    /// </summary>
    public object? InsertBefore { get; init; }

    /// <summary>
    /// Index at which Data should be inserted. This is a convenience value.
    /// </summary>
    public int InsertIndex { get; init; }
}
