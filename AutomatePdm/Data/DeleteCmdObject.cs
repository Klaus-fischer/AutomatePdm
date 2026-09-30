// <copyright file="DeleteCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class DeleteCmdObject : BaseCmdObject
{
    /// <summary>
    /// Gets the ID of file to delete.
    /// </summary>
    public int FileId => this.CmdData.mlObjectID1;

    /// <summary>
    /// Gets the ID of folder to delete file in.
    /// </summary>
    public int ParentFolderId => this.CmdData.mlObjectID2;

    /// <summary>
    /// Gets the Path to file to delete.
    /// </summary>
    public string FilePath => this.CmdData.mbsStrData1;
}
