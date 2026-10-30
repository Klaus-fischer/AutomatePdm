// <copyright file="TextExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;

public class TextExpression(string text) : IExpression
{
    public string Text { get; } = text;

    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => Text;
}
