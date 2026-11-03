// <copyright file="AndCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class AndCondition : ICondition
{
    /// <inheritdoc/>
    public bool Negate { get; init; }

    public required IReadOnlyList<ICondition> Conditions { get; init; }

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var result = true;

        foreach (var condition in this.Conditions)
        {
            if (!condition.IsValid(item, ctx))
            {
                result = false;
                break;
            }
        }

        return this.Negate ^ result;
    }
}