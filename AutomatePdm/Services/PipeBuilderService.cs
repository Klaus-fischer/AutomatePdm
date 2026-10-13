// <copyright file="PipeBuilderService.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;
using System.Collections.Generic;

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
