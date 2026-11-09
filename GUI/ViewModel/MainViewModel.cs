// <copyright file="MainViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutomatePdm.GUI.DragDrop;

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
        this.DropTriggerCommand = new RelayCommand<DropContext>(this.DropTrigger);
        this.DropScriptCommand = new RelayCommand<DropContext>(this.DropScript);
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

    public ICommand DropScriptCommand { get; }

    private void DropTrigger(DropContext context)
    {
        if (context.Data is not TriggerViewModel trigger ||
            context.SourceList is null ||
            context.TargetList is null)
        {
            return;
        }

        var sourceIndex = context.SourceList.IndexOf(trigger);
        if (sourceIndex < 0)
        {
            return;
        }

        if (context.Action == DragAction.Move)
        {
            int targetIndex = context.InsertIndex;
            context.SourceList.RemoveAt(sourceIndex);

            if (context.SourceList == context.TargetList &&
                sourceIndex < targetIndex)
            {
                targetIndex -= 1;
            }

            if (targetIndex > context.TargetList.Count)
            {
                targetIndex = context.TargetList.Count;
            }

            context.TargetList.Insert(targetIndex, trigger);
        }

        if (context.Action == DragAction.Copy)
        {
            int targetIndex = context.InsertIndex;
            if (targetIndex > context.TargetList.Count)
            {
                targetIndex = context.TargetList.Count;
            }

            trigger = trigger.Clone();
            context.TargetList.Insert(targetIndex, trigger);
        }

        if (context.TargetList == this.Triggers)
        {
            this.SelectedTrigger = trigger;
        }
    }

    private void DropScript(DropContext context)
    {
        if (context.Data is not ProcessStepViewModel processStep ||
            context.SourceList is null ||
            context.TargetList is null)
        {
            return;
        }

        var sourceIndex = context.SourceList.IndexOf(processStep);
        if (sourceIndex < 0)
        {
            return;
        }

        if (context.Action == DragAction.Move)
        {
            int targetIndex = context.InsertIndex;
            context.SourceList.RemoveAt(sourceIndex);

            if (context.SourceList == context.TargetList &&
                sourceIndex < targetIndex)
            {
                targetIndex -= 1;
            }

            if (targetIndex > context.TargetList.Count)
            {
                targetIndex = context.TargetList.Count;
            }

            context.TargetList.Insert(targetIndex, processStep);
        }

        if (context.Action == DragAction.Copy)
        {
            int targetIndex = context.InsertIndex;
            if (targetIndex > context.TargetList.Count)
            {
                targetIndex = context.TargetList.Count;
            }

            processStep = processStep.Clone();
            context.TargetList.Insert(targetIndex, processStep);
        }
    }
}