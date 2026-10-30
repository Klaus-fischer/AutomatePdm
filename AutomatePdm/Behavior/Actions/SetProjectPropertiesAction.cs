// <copyright file="SetProjectPropertiesAction.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;
using System.Text.Json.Serialization;

public class SetProjectPropertiesAction : IPipelineStep
{
    public Dictionary<string, IExpression> PropertyExpressions { get; init; } = new();

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
        if (item.Folder is not IEdmEnumeratorVariable6 enumerator)
        {
            return;
        }

        foreach (var expression in this.PropertyExpressions)
        {
            var value = expression.Value.Resolve(item, ctx);
            enumerator.SetVar(expression.Key, string.Empty, value);
        }

        enumerator.Flush();
    }
}

