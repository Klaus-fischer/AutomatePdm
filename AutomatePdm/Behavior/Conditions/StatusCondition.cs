// <copyright file="StatusCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Linq;

public class StatusCondition(string[] validStates) : ICondition
{
    private readonly string[] validStates = validStates;

    /// <inheritdoc/>
    public bool Negate { get; set; }

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var status = item.File.CurrentState.Name;
        return this.Negate ^ this.validStates.Contains(status);
    }
}
