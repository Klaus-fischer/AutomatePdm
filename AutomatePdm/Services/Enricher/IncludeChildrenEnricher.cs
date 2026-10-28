// <copyright file="IncludeChildrenEnricher.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

internal class IncludeChildrenEnricher : IPipelineStep
{
    public IPipelineStep? Next { get; set; }

    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        foreach (var item in items)
        {
        }
    }
}
