// <copyright file="MainViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using AutomatePdm.GUI;
using System.Windows.Input;

namespace AutomatePdm.GUI;

using AutomatePdm.GUI.DragDrop;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class MainViewModel : ViewModelBase
{
    private TriggerViewModel? selectedTrigger = null;

    private string popupTitle = string.Empty;

    private INotifyPropertyChanged? popup = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        this.AddTriggerCommand = new AddTriggerCommand(this);
        this.EditTriggerCommand = new EditTriggerCommand(this);
        this.DropTriggerCommand = new RelayCommand<DropContext>(this.Drop);
    }

    public ObservableCollection<TriggerViewModel> Triggers { get; } = [];

    public TriggerViewModel? SelectedTrigger
    {
        get => this.selectedTrigger;
        set => this.Set(ref this.selectedTrigger, value);
    }

    public ICommand AddTriggerCommand { get; }

    public ICommand EditTriggerCommand { get; }

    /// <summary>
    /// Gets or sets the ...
    /// </summary>
    public string PopupTitle
    {
        get => this.popupTitle;
        set => this.Set(ref this.popupTitle, value);
    }

    public INotifyPropertyChanged? Popup
    {
        get => this.popup;
        set => this.Set(ref this.popup, value);
    }

    public ICommand DropTriggerCommand { get; }

    private void Drop(DropContext context)
    {
        if (context.Data is not TriggerViewModel trigger)
        {
            return;
        }

        if (context.Action == DragAction.Move &&
            context.SourceList is not null &&
            context.TargetList is not null)
        {
            int sourceIndex = context.SourceList.IndexOf(trigger);
            if (sourceIndex >= 0)
            {
                context.SourceList.RemoveAt(sourceIndex);
                int targetIndex = context.InsertIndex;
                if (targetIndex > context.TargetList.Count)
                {
                    targetIndex = context.TargetList.Count;
                }

                context.TargetList.Insert(targetIndex, trigger);
            }
        }
    }
}
