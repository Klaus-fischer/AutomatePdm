// <copyright file="ITrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;
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