// <copyright file="PropertyExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

/// <summary>
/// Used to resolve a cached property from a work item.
/// </summary>
public class PropertyExpression : IExpression
{
    /// <summary>
    /// Gets the name of the property to resolve.
    /// </summary>
    public required string PropertyName { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string Resolve(WorkItem workItem, ExecutionContext ctx)
    {
       return workItem.GetStringProperty(this.PropertyName) ?? string.Empty;
    }
}
