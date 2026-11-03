// <copyright file="StatusCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Linq;

public class StatusCondition : ICondition
{
    public string[] ValidStates { get; init; } = Array.Empty<string>();

    /// <inheritdoc/>
    public bool Negate { get; init; }

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var status = item.File.CurrentState.Name;
        return this.Negate ^ this.ValidStates.Contains(status);
    }
}
