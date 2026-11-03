// <copyright file="StatusCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Linq;

public class StatusCondition : ICondition
{
    public string[] ValidStates { get; set; } = Array.Empty<string>();

    /// <inheritdoc/>
    public bool Negate { get; set; }

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var status = item.File.CurrentState.Name;
        return this.Negate ^ this.ValidStates.Contains(status);
    }
}
