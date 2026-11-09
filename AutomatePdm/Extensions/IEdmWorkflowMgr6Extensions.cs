// <copyright file="IEdmWorkflowMgr6Extensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Generic;
using EPDM.Interop.epdm;

public static class IEdmWorkflowMgr6Extensions
{
    public static IEnumerable<IEdmWorkflow6> EnumerateWorkFlows(this IEdmWorkflowMgr6 workflowMgr)
    {
        var pos = workflowMgr.GetFirstWorkflowPosition();

        while (!pos.IsNull)
        {
            yield return workflowMgr.GetNextWorkflow(pos);
        }
    }

    public static IEnumerable<IEdmState6> EnumerateStates(this IEdmWorkflow6 workflow)
    {
        var pos = workflow.GetFirstStatePosition();

        while (!pos.IsNull)
        {
            yield return workflow.GetNextState(pos);
        }
    }
}
