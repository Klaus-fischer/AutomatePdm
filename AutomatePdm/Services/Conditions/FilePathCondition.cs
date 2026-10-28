// <copyright file="FilePathCondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Text.RegularExpressions;

public class FilePathCondition(string regularExpression, bool caseSensitive) : ICondition
{
    private readonly Regex regex = new Regex(regularExpression, RegexOptions.Compiled | (caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase));

    /// <inheritdoc/>
    public bool Negate { get; set; }

    /// <inheritdoc/>
    public bool IsValid(WorkItem item, ExecutionContext ctx)
    {
        var fileName = item.GetStringProperty(TriggerExtensions.FilePath);
        return this.Negate ^ this.regex.IsMatch(fileName ?? string.Empty);
    }
}