// <copyright file="MainViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using AutomatePdm.GUI.DragDrop;

public class MainViewModel : ViewModelBase
{
    private static readonly FrozenSet<Type> PipelineStepTypes =
        typeof(MainViewModel).Assembly.GetTypes()
            .Where(t => typeof(PipelineStepViewModel).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Where(t => typeof(IPipelineStepBuilder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToFrozenSet();

    private string popupTitle = string.Empty;
    private INotifyPropertyChanged? popup = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        this.AddTriggerCommand = new AddTriggerCommand(this);
        this.EditTriggerCommand = new EditTriggerCommand(this);
        this.EditScriptCommand = new EditScriptCommand(this);
        this.DeleteCommand = new RelayCommand<DropContext>(this.DeleteOnDrop);
        this.DragableProcessSteps = this.GetPipelineSteps();
    }

    public DropAbleCollection<TriggerViewModel> Triggers { get; } = [];

    public IReadOnlyCollection<PipelineStepViewModel> DragableProcessSteps { get; }

    /// <summary>
    /// Gets the command to add a new trigger.
    /// </summary>
    public ICommand AddTriggerCommand { get; }

    /// <summary>
    /// Gets the command to edit a trigger.
    /// </summary>
    public ICommand EditTriggerCommand { get; }

    /// <summary>
    /// Gets the command to edit a script associated with a pipeline step.
    /// </summary>
    public ICommand EditScriptCommand { get; }

    /// <summary>
    /// Gets the command to remove a drag drop element.
    /// </summary>
    public ICommand DeleteCommand { get; }

    /// <summary>
    /// Gets or sets the title of the popup window.
    /// </summary>
    public string PopupTitle
    {
        get => this.popupTitle;
        set => this.Set(ref this.popupTitle, value);
    }

    /// <summary>
    /// Gets or sets the popup view model that implements <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public INotifyPropertyChanged? Popup
    {
        get => this.popup;
        set => this.Set(ref this.popup, value);
    }

    private PipelineStepViewModel[] GetPipelineSteps()
    {
        var result = new PipelineStepViewModel[PipelineStepTypes.Count];

        for (int i = 0; i < PipelineStepTypes.Count; i++)
        {
            var type = PipelineStepTypes.ElementAt(i);
            if (Activator.CreateInstance(type) is not PipelineStepViewModel viewModel)
            {
                throw new InvalidOperationException($"Could not create instance of {type.FullName}");
            }

            viewModel.EditCommand = this.EditScriptCommand;
            result[i] = viewModel;
        }

        return result;
    }

    private void DeleteOnDrop(DropContext context)
    {
        if (context.SourceList is not IList list)
        {
            return;
        }

        var sourceIndex = list.IndexOf(context.Data);
        if (sourceIndex < 0)
        {
            return;
        }

        list.RemoveAt(sourceIndex);
    }
}