// <copyright file="ChangeStateAction.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using System;
using EPDM.Interop.epdm;

internal class ChangeStateAction : IPipelineStep
{
    public string TargetStateName { get; init; } = string.Empty;

    public string Comment { get; init; } = string.Empty;

    public EdmStateFlags Flags { get; init; }

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        foreach (var item in items)
        {
            try
            {
                item.File.ChangeState(
                    this.TargetStateName,
                    item.Folder.ID,
                    this.Comment,
                    ctx.ParentWnd,
                    (int)this.Flags);
            }
            catch (Exception ex)
            {
                ctx.Log(item, ex);
            }
        }

        this.Next?.Execute(items, ctx);
    }
}
