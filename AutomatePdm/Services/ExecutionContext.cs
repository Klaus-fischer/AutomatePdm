// <copyright file="ExecutionContext.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using System.Collections.Generic;
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

public class WorkItem(IEdmFile5 file, IEdmFolder5 folder)
{
    internal IEdmFile5 File { get; } = file;

    internal IEdmFolder5 Folder { get; } = folder;

    public Dictionary<string, object> Properties { get; } = new();

    internal static WorkItem Create(int fileId, int folderId, IEdmVault5 vault)
    {
        var file = (IEdmFile5)vault.GetObject(EdmObjectType.EdmObject_File, fileId);
        var folder = (IEdmFolder5)vault.GetObject(EdmObjectType.EdmObject_Folder, folderId);
        return new WorkItem(file, folder);
    }

    internal static WorkItem Create(string filePath, IEdmVault5 vault)
    {
        var file = vault.GetFileFromPath(filePath, out var folder);
        return new WorkItem(file, folder);
    }
}