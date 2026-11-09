// <copyright file="DropableCollection.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutomatePdm.GUI.DragDrop;

internal class DropableCollection<T> : ObservableCollection<T>
    where T : ICloneable<T>
{
    private T? selectedItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="DropableCollection{T}"/> class.
    /// </summary>
    public DropableCollection()
    {
        this.DropCommand = new RelayCommand<DropContext>(this.OnDrop);
    }

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
            if (context.SourceList == this)
            {
                this.Move(sourceIndex, targetIndex);
            }
            else
            {
                if (sourceIndex < targetIndex)
                {
                    targetIndex -= 1;
                }

                context.SourceList.RemoveAt(sourceIndex);
                if (targetIndex > context.TargetList.Count)
                {
                    targetIndex = context.TargetList.Count;
                }

                context.TargetList.Insert(targetIndex, item);
            }
        }
        else if (context.Action == DragAction.Move)
        {
            context.SourceList.RemoveAt(sourceIndex);

            if (targetIndex > context.TargetList.Count)
            {
                targetIndex = context.TargetList.Count;
            }

            context.TargetList.Insert(targetIndex, item);
        }
        else if (context.Action == DragAction.Copy)
        {
            if (targetIndex > context.TargetList.Count)
            {
                targetIndex = context.TargetList.Count;
            }

            item = item.Clone();
            context.TargetList.Insert(targetIndex, item);
        }

        this.SelectedItem = item;
    }
}
