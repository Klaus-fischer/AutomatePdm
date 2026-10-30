// <copyright file="ScheduleTaskAction.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;
using EPDM.Interop.epdm;

public class ScheduleTaskAction : IPipelineStep
{
    public string TaskName { get; init; } = string.Empty;

    public string Comment { get; init; } = string.Empty;

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        if (ctx.Vault is not IEdmVault7 vault)
        {
            throw new InvalidProgramException("Vault could not be casted as IEdmVault7");
        }

        var taskMgr = (IEdmTaskMgr)vault.CreateUtility(EdmUtility.EdmUtil_TaskMgr);
        var task = taskMgr.GetTasks().FirstOrDefault(o => o.mbsTaskName == this.TaskName);

        if (task.mlTaskID == 0)
        {
            ctx.Log($"Task '{this.TaskName}' not found.");
            this.Next?.Execute(items, ctx);
            return;
        }

        EdmSelItem2[] taskItems = [..items.Select(o => new EdmSelItem2()
        {
            meType = EdmObjectType.EdmObject_File,
            mlID = o.File.ID,
            mlParentID = o.Folder.ID,
            mlVersion = o.File.CurrentVersion,
        })];

        taskMgr.RunTask(task, taskItems, ctx.ParentWnd);

        this.Next?.Execute(items, ctx);
    }
}
