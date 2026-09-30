// <copyright file="CopyFolderCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class CopyFolderCmdObject : BaseCmdObject
{
    public int NewFolderId => this.CmdData.mlObjectID1;

    public int SourceFolderId => this.CmdData.mlObjectID2;

    public int DestinationParentFolderId => this.CmdData.mlObjectID3;

    public string NewFolderPath => this.CmdData.mbsStrData1;
}
