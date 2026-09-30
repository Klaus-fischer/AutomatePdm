// <copyright file="BaseCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

using System;
using EPDM.Interop.epdm;

public class BaseCmdObject
{
    public EdmCmdType CommandType => this.Command.meCmdType;

    public EdmCmd Command { get; private set; }

    public EdmCmdData CmdData { get; private set; }

    public bool Cancel { get; set; }

    public void Assign(EdmCmd command, EdmCmdData cmdData)
    {
        this.Command = command;
        this.CmdData = cmdData;
        this.OnAssign();
    }

    protected virtual void OnAssign()
    {
    }

    public void Assign(EdmCmdData[] allCmdData)
    {
        this.OnAssign(allCmdData);
    }

    protected virtual void OnAssign(EdmCmdData[] allCmdData)
    {
    }

    public void UpdateValues(ref EdmCmd cmd, ref EdmCmdData data)
    {
        if (this.Cancel)
        {
            cmd.mbCancel |= Convert.ToInt16(true);
        }

        this.OnUpdateValues(ref cmd, ref data);
    }

    protected virtual void OnUpdateValues(ref EdmCmd cmd, ref EdmCmdData data)
    {
    }
}
