// <copyright file="DeleteFolderCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class DeleteFolderCmdObject : BaseCmdObject
{
    /// <summary>
    /// Gets the ID of folder to delete.
    /// </summary>
    public int FolderId => this.CmdData.mlObjectID1;

    /// <summary>
    /// Gets the Path to folder to delete.
    /// </summary>
    public string FilePath => this.CmdData.mbsStrData1;
}
