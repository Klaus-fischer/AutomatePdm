// <copyright file="ExpressionCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;

internal class ExpressionCondition(IExpression first, IExpression second) : ICondition
{
    public bool Negate { get; set; }

    public bool CaseSensitive { get; set; }

    public IExpression First { get; } = first;

    public IExpression Second { get; } = second;

    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var first = this.First.Resolve(item, ctx);
        var second = this.Second.Resolve(item, ctx);

        var comparer = this.CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

        return this.Negate ^ comparer.Equals(first, second);
    }
}
