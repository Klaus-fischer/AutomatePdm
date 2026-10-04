// <copyright file="ITrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using AutomatePdm.Data;
using EPDM.Interop.epdm;
using System.Xml.Linq;

public interface ITrigger
{
    EdmCmdType CmdType { get; }

    string[] ProvidedProperties { get; }

    void Execute(ref EdmCmdData[] data, ExecutionContext ctx);

    IPipelineStep? Next { get; set; }
}

internal interface IPreEventTrigger
{
    bool Cancel { get; set; }
}

internal abstract class TriggerBase : ITrigger
{
    public abstract EdmCmdType CmdType { get; }

    public IPipelineStep? Next { get; set; }

    public string[] ProvidedProperties { get; }

    protected abstract WorkItem Create(EdmCmdData data, ExecutionContext ctx);

    public void Execute(ref EdmCmdData[] data, ExecutionContext ctx)
    {
        WorkItem[] workItems = new WorkItem[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            workItems[i] = this.Create(data[i], ctx);
        }

        this.Next?.Execute(workItems, ctx);
    }
}
