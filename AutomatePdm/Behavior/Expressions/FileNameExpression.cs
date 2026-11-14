// <copyright file="FileNameExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class FileNameExpression : IExpression
{
    /// <inheritdoc/>
    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => workItem.FileName;
}

public class FilePathExpression : IExpression
{
    /// <inheritdoc/>
    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => workItem.FilePath;
}

public class ProjectNameExpression : IExpression
{
    /// <inheritdoc/>
    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => workItem.Folder.Name;
}

public class ProjectPathExpression : IExpression
{
    /// <inheritdoc/>
    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => workItem.Folder.LocalPath;
}

public class FileExtensionExpression : IExpression
{
    /// <inheritdoc/>
    public string Resolve(WorkItem workItem, ExecutionContext ctx)
        => Path.GetExtension(workItem.FileName);
}