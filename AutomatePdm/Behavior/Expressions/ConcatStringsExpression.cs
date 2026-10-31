// <copyright file="ConcatStringsExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Text;

public class ConcatStringsExpression : IExpression
{
    public required ExpressionCollection ExpressionColletion { get; init; }

    public string Resolve(WorkItem workItem, ExecutionContext ctx)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var expression in this.ExpressionColletion)
        {
            sb.Append(expression.Resolve(workItem, ctx));
        }

        return sb.ToString();
    }
}
