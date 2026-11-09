// <copyright file="SetFileVariableAction.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Text.Json.Serialization;
using EPDM.Interop.epdm;

public class SetFileVariableAction : IPipelineStep
{
    /// <inheritdoc/>
    public Metadata Metadata { get; set; }

    public string VariableName { get; init; }

    public string ConfigurationName { get; init; } = string.Empty;

    public IExpression Expression { get; init; }

    /// <inheritdoc/>
    [JsonIgnore]
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        foreach (var item in items)
        {
            this.AssignProjectVariables(item, ctx);
        }

        this.Next?.Execute(items, ctx);
    }

    private void AssignProjectVariables(WorkItem item, ExecutionContext ctx)
    {
        if (item.File.GetEnumeratorVariable() is not IEdmEnumeratorVariable10 enumerator)
        {
            return;
        }

        var value = this.Expression.Resolve(item, ctx);
        enumerator.SetVar(this.VariableName, string.Empty, value);
        enumerator.Flush();
    }
}