// <copyright file="ExecutionContext.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Text;
using EPDM.Interop.epdm;

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