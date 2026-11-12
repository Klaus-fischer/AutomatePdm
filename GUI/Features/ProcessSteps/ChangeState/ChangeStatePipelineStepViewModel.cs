// <copyright file="ChangeStatePipelineStepViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

public class ChangeStatePipelineStepViewModel : PipelineStepViewModel, IPipelineStepBuilder<ChangeStateAction>
{
    private string targetStateName = string.Empty;
    private string comment = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeStatePipelineStepViewModel"/> class.
    /// </summary>
    public ChangeStatePipelineStepViewModel()
    {
        this.Title = "Ändere Status";
        this.Description = "Ändert den Status des Work Items auf den angegebenen Zielstatus.";
        this.IconCode = "\uEF90"; // Flow
    }

    public NameAndDescription[] TargetStateNameSource { get; } = [];

    /// <summary>
    /// Gets or sets the target state name.
    /// </summary>
    public string TargetStateName
    {
        get => this.targetStateName;
        set => this.Set(ref this.targetStateName, value);
    }

    /// <summary>
    /// Gets or sets the comment for the state change action.
    /// </summary>
    public string Comment
    {
        get => this.comment;
        set => this.Set(ref this.comment, value);
    }

    /// <inheritdoc/>
    public override string ProcessStepName => nameof(ChangeStateAction);

    /// <inheritdoc/>
    public ChangeStateAction Build()
        => new ChangeStateAction
        {
            TargetStateName = this.TargetStateName,
            Comment = this.Comment,
            Metadata = new Metadata(this.Title, this.Description),
        };

    /// <inheritdoc/>
    public override PipelineStepViewModel Clone()
        => new ChangeStatePipelineStepViewModel
        {
            Title = this.Title,
            Description = this.Description,
            TargetStateName = this.TargetStateName,
            Comment = this.Comment,
            EditCommand = this.EditCommand,
        };
}
