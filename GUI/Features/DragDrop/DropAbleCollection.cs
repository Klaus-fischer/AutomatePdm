// <copyright file="DropAbleCollection.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.DragDrop;

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public interface ISupportMove
{
    /// <summary>
    /// Move item at oldIndex to newIndex.
    /// </summary>
    /// <param name="oldIndex">The index of the item to move.</param>
    /// <param name="newIndex">The index to move the item to.</param>
    void Move(int oldIndex, int newIndex);
}

public class DropAbleCollection<T> : ObservableCollection<T>, ISupportMove
    where T : ICloneable<T>
{
    private T? selectedItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="DropAbleCollection{T}"/> class.
    /// </summary>
    public DropAbleCollection()
    {
        this.DropCommand = new RelayCommand<DropContext>(this.OnDrop);
    }

    /// <summary>
    /// Gets the type of the items in the collection.
    /// </summary>
    public Type Type => typeof(T);

    /// <summary>
    /// Gets the key associated with the collection.
    /// </summary>
    public string Key { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the selected item in the collection.
    /// </summary>
    public T? SelectedItem
    {
        get => this.selectedItem;
        set
        {
            this.selectedItem = value;
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(this.SelectedItem)));
        }
    }

    /// <summary>
    /// Gets the command that handles the drop action for the collection.
    /// </summary>
    public ICommand DropCommand { get; }

    private void OnDrop(DropContext context)
    {
        if (context.Data is not T item ||
            context.SourceList is null ||
            context.TargetList is null)
        {
            return;
        }

        var sourceIndex = context.SourceList.IndexOf(item);
        int targetIndex = context.InsertIndex;
        if (sourceIndex < 0)
        {
            return;
        }

        if (context.Action == DragAction.Move && context.SourceList == context.TargetList)
        {
            if (sourceIndex < targetIndex)
            {
                targetIndex -= 1;
            }

            if (context.SourceList is ISupportMove moveAble)
            {
                moveAble.Move(sourceIndex, targetIndex);
            }
            else
            {
                MoveItem(context.SourceList, context.TargetList, item, sourceIndex, targetIndex);
            }
        }
        else if (context.Action == DragAction.Move)
        {
            MoveItem(context.SourceList, context.TargetList, item, sourceIndex, targetIndex);
        }
        else if (context.Action == DragAction.Copy)
        {
            CopyItem(context.TargetList, ref item, ref targetIndex);
        }

        this.SelectedItem = item;
    }

    private static void MoveItem(IList source, IList target, T item, int sourceIndex, int targetIndex)
    {
        source.RemoveAt(sourceIndex);

        if (targetIndex > target!.Count)
        {
            targetIndex = target.Count;
        }

        target.Insert(targetIndex, item);
    }

    private static void CopyItem(IList target, ref T item, ref int targetIndex)
    {
        if (targetIndex > target.Count)
        {
            targetIndex = target.Count;
        }

        item = item.Clone();
        target.Insert(targetIndex, item);
    }
}
