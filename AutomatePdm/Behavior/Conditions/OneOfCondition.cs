// <copyright file="OneOfCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class OneOfCondition(params ICondition[] subConditions) : ICondition
{
    /// <inheritdoc/>
    public bool Negate { get; set; }

    public IReadOnlyList<ICondition> SubConditions { get; } = [.. subConditions];

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var valid = 0;

        foreach (var condition in this.SubConditions)
        {
            if (condition.IsValid(item, ctx))
            {
                valid++;
            }
        }

        return this.Negate ^ (valid == 1);
    }
}