// <copyright file="TriggerViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

public class TriggerViewModel : ViewModelBase, ICloneable<TriggerViewModel>
{
    private static readonly Lazy<NameAndDescription[]> TriggerNameSourceCollection;

    private string title = string.Empty;

    private string description = string.Empty;

    private string triggerName = string.Empty;

    static TriggerViewModel()
    {
        TriggerNameSourceCollection = new Lazy<NameAndDescription[]>(() => typeof(ITrigger).Assembly.GetTypes()
            .Where(t => typeof(ITrigger).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(o => new NameAndDescription(
                o.Name,
                o.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty))
            .ToArray());
    }

    /// <summary>
    /// Gets or sets the title of the trigger.
    /// </summary>
    public string Title
    {
        get => this.title;
        set => this.Set(ref this.title, value);
    }

    /// <summary>
    /// Gets or sets the description of the trigger.
    /// </summary>
    public string Description
    {
        get => this.description;
        set => this.Set(ref this.description, value);
    }

    /// <summary>
    /// Gets or sets the display name of the trigger.
    /// </summary>
    public string TriggerName
    {
        get => this.triggerName;
        set => this.Set(ref this.triggerName, value);
    }

    public required ICommand EditTriggerCommand { get; set; }

    /// <summary>
    /// Gets the source of items.
    /// </summary>
    public NameAndDescription[] TriggerNameSource { get; } = TriggerNameSourceCollection.Value;

    public DropAbleCollection<ProcessStepViewModel> Script { get; } = [];

    /// <inheritdoc/>
    public TriggerViewModel Clone()
    {
        var result = new TriggerViewModel
        {
            Title = this.Title,
            Description = this.Description,
            TriggerName = this.TriggerName,
            EditTriggerCommand = this.EditTriggerCommand,
        };

        foreach (var step in this.Script)
        {
            result.Script.Add(step.Clone());
        }

        return result;
    }
}
