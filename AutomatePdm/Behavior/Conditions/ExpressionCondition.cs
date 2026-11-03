// <copyright file="ExpressionCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;

public class ExpressionCondition : ICondition
{
    public bool Negate { get; set; }

    public bool CaseSensitive { get; init; }

    public required IExpression First { get; init; }

    public required IExpression Second { get; init; }

    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var first = this.First.Resolve(item, ctx);
        var second = this.Second.Resolve(item, ctx);

        var comparer = this.CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

        return this.Negate ^ comparer.Equals(first, second);
    }
}
