// <copyright file="ICondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public interface ICondition
{
    /// <summary>
    /// Gets or sets a value indicating whether the output should be negated.
    /// </summary>
    public bool Negate { get; set; }

    /// <summary>
    /// Checks if the condition is valid.
    /// </summary>
    /// <param name="item">Item to check.</param>
    /// <param name="ctx">The current Execution context.</param>
    /// <returns>True if condition is valid.</returns>
    bool IsValid(WorkItem item, ExecutionContext ctx);
}
