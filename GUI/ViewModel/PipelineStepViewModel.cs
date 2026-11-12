// <copyright file="PipelineStepViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Windows.Input;

public class PipelineStepViewModel : ViewModelBase, ICloneable<PipelineStepViewModel>
{
    private string title = string.Empty;

    private string description = string.Empty;

    public required ICommand EditCommand { get; set; }

    public Icon Icon { get; protected internal set; } = Icon.QuickNote;

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
    public virtual string ProcessStepName{ get; }

    /// <inheritdoc/>
    public virtual PipelineStepViewModel Clone()
    {
        var result = new PipelineStepViewModel
        {
            Title = this.Title,
            Description = this.Description,
            EditCommand = this.EditCommand,
        };

        return result;
    }
}
