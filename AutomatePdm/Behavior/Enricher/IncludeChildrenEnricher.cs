// <copyright file="IncludeChildrenEnricher.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class IncludeChildrenEnricher : IPipelineStep
{
    /// <inheritdoc/>
    public Metadata Metadata { get; set; }

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        foreach (var item in items)
        {
        }
    }
}
