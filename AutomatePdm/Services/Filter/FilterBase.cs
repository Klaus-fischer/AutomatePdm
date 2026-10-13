// <copyright file="FilterBase.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using System.Linq;
using System.Text.RegularExpressions;

internal abstract class FilterBase : IPipelineStep
{
    public IPipelineStep? Next { get; set; }

    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        items = [.. items.Where(i => this.Filter(i, ctx))];

        if (items.Length == 0 || this.Next == null)
        {
            return;
        }

        this.Next.Execute(items, ctx);
    }

    protected abstract bool Filter(WorkItem item, ExecutionContext ctx);
}

internal class StatusFilter(string[] validStates) : FilterBase
{
    private readonly string[] validStates = validStates;

    protected override bool Filter(WorkItem item, ExecutionContext ctx)
    {
        var status = item.File.CurrentState.Name;
        return this.validStates.Contains(status);
    }
}

internal class FilePathFilter(string regularExpression, bool caseSensitive) : FilterBase
{
    private readonly Regex regex = new Regex(regularExpression, RegexOptions.Compiled | (caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase));

    protected override bool Filter(WorkItem item, ExecutionContext ctx)
    {
        var fileName = item.Properties.GetValueOrDefault(TriggerExtensions.FilePath, item.File.GetLocalPath(item.Folder.ID)) as string;
        return this.regex.IsMatch(fileName ?? string.Empty);
    }
}