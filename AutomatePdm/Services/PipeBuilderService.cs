// <copyright file="PipeBuilderService.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using System.Collections.Generic;
using System.Text;
using EPDM.Interop.epdm;

internal class PipeBuilderService
{
    private Dictionary<int, ITrigger> menuTrigger = new();
    private Dictionary<string, ITrigger> taskTrigger = new();
    private Dictionary<EdmCmdType, List<ITrigger>> hookTriggers = new();

    public void LoadSetup(IEdmVault5 vault)
    {

    }

    public IEnumerable<EdmCmdType> Hooks
    {
        get
        {
            if (this.taskTrigger.Any())
            {
                yield return EdmCmdType.EdmCmd_TaskRun;
            }

            foreach (var hook in this.hookTriggers.Keys.Distinct())
            {
                yield return hook;
            }
        }
    }

    public void HandleCommand(ref EdmCmdData[] data, ExecutionContext ctx)
    {
        if (ctx.CmdType == EdmCmdType.EdmCmd_Menu &&
            this.menuTrigger.TryGetValue(ctx.CmdID, out var menuTrigger))
        {
            menuTrigger.Execute(ref data, ctx);
            return;
        }

        if (ctx.CmdType == EdmCmdType.EdmCmd_TaskRun &&
            ctx.Extra is IEdmTaskInstance taskInstance &&
            this.taskTrigger.TryGetValue(taskInstance.TaskName, out var taskTrigger))
        {
            taskTrigger.Execute(ref data, ctx);
            return;
        }

        if (this.hookTriggers.TryGetValue(ctx.CmdType, out var triggers))
        {
            foreach (var trigger in triggers)
            {
                trigger.Execute(ref data, ctx);
                if (ctx.Cancel)
                {
                    break;
                }
            }
        }
    }
}

public class ExecutionContext(EdmCmd cmd)
{
    public IEdmVault5 Vault { get; } = (IEdmVault5)cmd.mpoVault;

    public StringBuilder Log { get; } = new StringBuilder();

    public object? Extra { get; } = cmd.mpoExtra;

    public bool Cancel { get; set; }

    public EdmCmdType CmdType { get; } = cmd.meCmdType;

    public int CmdID { get; } = cmd.mlCmdID;

    public int ParentWnd { get; } = cmd.mlParentWnd;

    public int CurrentFolderID { get; } = cmd.mlCurrentFolderID;
}

public interface ITrigger
{
    string[] ProvidedProperties { get; }

    void Execute(ref EdmCmdData[] data, ExecutionContext ctx);

    IPipelineStep? Next { get; set; }
}

public interface IPipelineStep
{
    void Execute(WorkItem item, ExecutionContext ctx);

    public IPipelineStep? Next { get; set; }
}

public class WorkItem
{
    public Dictionary<string, object> Properties { get; } = new();
}