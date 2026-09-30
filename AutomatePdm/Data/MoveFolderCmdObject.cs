// <copyright file="MoveFolderCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;
public class MoveFolderCmdObject : BaseCmdObject
{
    public int FolderId => this.CmdData.mlObjectID1;

    public int ParentSourceFolderId => this.CmdData.mlObjectID2;

    public int ParentDestinationFolderId => this.CmdData.mlObjectID3;

    public string SourceFolderPath => this.CmdData.mbsStrData1;

    public string DestinationFolderPath => this.CmdData.mbsStrData2;
}
