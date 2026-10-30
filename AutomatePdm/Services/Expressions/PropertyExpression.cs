// <copyright file="PropertyExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

internal class PropertyExpression(string propertyName, string configurationName = "") : IExpression
{
    public string PropertyName { get; } = propertyName;

    public string ConfigurationName { get; } = configurationName;

    public string Resolve(WorkItem workItem, ExecutionContext ctx)
    {
        var enumerator = workItem.File.GetEnumeratorVariable();
        if (!enumerator.GetVar(
            this.PropertyName,
            this.ConfigurationName,
            out var valueObject))
        {
            ctx.Log($"Variable {this.PropertyName} not found for file {workItem.File.Name}");
            return string.Empty;
        }

        return valueObject as string ?? string.Empty;
    }
}
