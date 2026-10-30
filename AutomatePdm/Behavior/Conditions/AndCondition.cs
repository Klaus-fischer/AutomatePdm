// <copyright file="AndCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class AndCondition(params ICondition[] subConditions) : ICondition
{
    /// <inheritdoc/>
    public bool Negate { get; set; }

    public IReadOnlyList<ICondition> SubConditions { get; } = [.. subConditions];

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var result = true;

        foreach (var condition in this.SubConditions)
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

public class OrCondition(params ICondition[] subConditions) : ICondition
{
    /// <inheritdoc/>
    public bool Negate { get; set; }

    public IReadOnlyList<ICondition> SubConditions { get; } = [.. subConditions];

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var result = false;

        foreach (var condition in this.SubConditions)
        {
            if (condition.IsValid(item, ctx))
            {
                result = true;
                break;
            }
        }

        return this.Negate ^ result;
    }
}

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