// <copyright file="AddFolderCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class AddFolderCmdObject : BaseCmdObject
{
    public int FolderId => this.CmdData.mlObjectID1;

    public int ParentFolderId => this.CmdData.mlObjectID3;

    public string NewFolderName => this.CmdData.mbsStrData1 ?? string.Empty;
}
