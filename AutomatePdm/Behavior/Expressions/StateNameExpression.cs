// <copyright file="StateNameExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class StateNameExpression : IExpression
{
    /// <inheritdoc/>
    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => workItem.File?.CurrentState.Name ?? "private";
}
