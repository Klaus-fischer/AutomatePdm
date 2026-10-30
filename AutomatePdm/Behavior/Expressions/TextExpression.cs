// <copyright file="TextExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;

public class TextExpression : IExpression
{
    public required string Text { get; init; }

    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => Text;
}
