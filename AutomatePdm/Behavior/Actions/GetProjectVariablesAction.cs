// <copyright file="GetProjectVariablesAction.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;
using EPDM.Interop.epdm;

public class GetProjectVariablesAction : IPipelineStep
{
    public string[] VariableNames { get; init; } = Array.Empty<string>();

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        foreach (var item in items)
        {
            this.Enrich(item, ctx);
        }

        this.Next?.Execute(items, ctx);
    }

    private void Enrich(WorkItem item, ExecutionContext ctx)
    {
        if (item.Folder is not IEdmEnumeratorVariable10 enumerator)
        {
            ctx.Log($"Folder {item.Folder.Name} does not support variable enumeration.");
            return;
        }

        foreach (var propertyName in this.VariableNames)
        {
            if (!enumerator.GetVar(
                        propertyName,
                        string.Empty,
                        out var valueObject))
            {
                ctx.Log($"Variable {propertyName} not found for file {item.File.Name}.");
                continue;
            }

            item.AddProperty(propertyName, valueObject?.ToString() ?? string.Empty);
        }
    }
}
