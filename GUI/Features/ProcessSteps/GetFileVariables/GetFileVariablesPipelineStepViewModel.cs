// <copyright file="GetFileVariablesPipelineStepViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Linq;

public class GetFileVariablesPipelineStepViewModel : PipelineStepViewModel, IPipelineStepBuilder<GetFileVariablesAction>
{
    private string configurtionName = "@";
    private string variableNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeStatePipelineStepViewModel"/> class.
    /// </summary>
    public GetFileVariablesPipelineStepViewModel()
    {
        this.Title = "Lade Datei-Variabeln";
        this.Description = "Läd Datei-Variabeln vom PDM.";
        this.Icon = Icon.DictionaryCloud;
    }

    /// <summary>
    /// Gets or sets the Name of the configuration of the files.
    /// @ is only for SolidWorks Files valid.
    /// </summary>
    public string ConfigurationName
    {
        get => this.configurtionName;
        set => this.Set(ref this.configurtionName, value);
    }

    /// <summary>
    /// Gets or sets the collection ov variable names.
    /// Each Name must be in a separate Line.
    /// </summary>
    public string VariableNames
    {
        get => this.variableNames;
        set => this.Set(ref this.variableNames, value);
    }

    /// <inheritdoc/>
    public override string ProcessStepName => nameof(GetFileVariablesAction);

    /// <inheritdoc/>
    public GetFileVariablesAction Build()
        => new GetFileVariablesAction
        {
            Metadata = new Metadata(this.Title, this.Description),
            ConfigurationName = this.ConfigurationName,
            VariableNames = this.VariableNames
                                .Split(['\r', '\n', ';', ',', ' '], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries)
                                .Distinct()
                                .ToArray(),
        };

    /// <inheritdoc/>
    public override PipelineStepViewModel Clone()
        => new GetFileVariablesPipelineStepViewModel
        {
            Title = this.Title,
            Description = this.Description,
            ConfigurationName = this.ConfigurationName,
            VariableNames = this.VariableNames,
            EditCommand = this.EditCommand,
        };
}
