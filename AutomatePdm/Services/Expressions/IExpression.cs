// <copyright file="IExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public interface IExpression
{
    string Resolve(WorkItem workItem, ExecutionContext ctx);
}
