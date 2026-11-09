// <copyright file="GetFileVariablesAction.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;
using System;
using System.Text.Json.Serialization;

public class GetFileVariablesAction : IPipelineStep
{
    /// <inheritdoc/>
    public Metadata Metadata { get; set; }

    public string[] VariableNames { get; init; } = Array.Empty<string>();

    public string ConfigurationName { get; init; } = string.Empty;

    /// <inheritdoc/>
    [JsonIgnore]
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
        if (item.File.GetEnumeratorVariable() is not IEdmEnumeratorVariable10 enumerator)
        {
            ctx.Log($"File {item.File.Name} does not support variable enumeration.");
            return;
        }

        foreach (var propertyName in this.VariableNames)
        {
            if (!enumerator.GetVar2(
                        propertyName,
                        this.ConfigurationName,
                        item.Folder.ID,
                        out var valueObject))
            {
                ctx.Log($"Variable {propertyName} not found for file {item.File.Name}.");
                continue;
            }

            item.AddProperty(propertyName, valueObject?.ToString() ?? string.Empty);
        }
    }
}
