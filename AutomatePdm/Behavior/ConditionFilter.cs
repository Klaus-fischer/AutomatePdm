// <copyright file="ConditionFilter.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Linq;

public class ConditionFilter : IPipelineStep
{
    public IPipelineStep? Next { get; set; }

    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        items = [.. items.Where(i => this.Condition.IsValid(i, ctx))];

        if (items.Length == 0 || this.Next == null)
        {
            return;
        }

        this.Next.Execute(items, ctx);
    }

    public required ICondition Condition { get; set; }
}
