// <copyright file="ProcessStepViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Windows.Input;

public class ProcessStepViewModel : ViewModelBase, ICloneable<ProcessStepViewModel>
{
    private string title = string.Empty;

    private string description = string.Empty;

    private string processStepName = string.Empty;

    public required ICommand EditCommand { get; set; }

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
    /// Gets or sets the display name of the process step.
    /// </summary>
    public string ProcessStepName
    {
        get => this.processStepName;
        set => this.Set(ref this.processStepName, value);
    }

    /// <inheritdoc/>
    public virtual ProcessStepViewModel Clone()
    {
        var result = new ProcessStepViewModel
        {
            Title = this.Title,
            Description = this.Description,
            ProcessStepName = this.ProcessStepName,
            EditCommand = this.EditCommand,
        };

        return result;
    }
}
