// <copyright file="ExpressionCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;

public class ExpressionCondition : ICondition
{
    public bool Negate { get; init; }

    public bool CaseSensitive { get; init; }

    public ExpressionOperator Operator { get; init; } = ExpressionOperator.Equal;

    public required IExpression First { get; init; }

    public required IExpression Second { get; init; }

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var first = this.First.Resolve(item, ctx) ?? string.Empty;
        var second = this.Second.Resolve(item, ctx) ?? string.Empty;
        var result = this.Compare(first, second);

        return this.Negate ^ result;
    }

    private bool Compare(string first, string second)
    {
        var comparer = this.CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        var comparerType = this.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        return this.Operator switch
        {
            ExpressionOperator.Equal => comparer.Equals(first, second),
            ExpressionOperator.NotEqual => !comparer.Equals(first, second),
            ExpressionOperator.Contains => first.Contains(second, comparerType),
            ExpressionOperator.EndsWith => first.EndsWith(second, comparerType),
            ExpressionOperator.StartsWith => first.StartsWith(second, comparerType),
            _ => throw new NotImplementedException($"Operator {this.Operator} is not implemented."),
        };
    }
}

public enum ExpressionOperator
{
    Equal,
    NotEqual,
    Contains,
    EndsWith,
    StartsWith,
}