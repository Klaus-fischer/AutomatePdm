// <copyright file="OneOfCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class OneOfCondition : ICondition
{
    /// <inheritdoc/>
    public bool Negate { get; init; }

    public required IReadOnlyList<ICondition> Conditions { get; init; }

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var valid = 0;

        foreach (var condition in this.Conditions)
        {
            if (condition.IsValid(item, ctx))
            {
                valid++;
            }
        }

        return this.Negate ^ (valid == 1);
    }
}