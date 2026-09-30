// <copyright file="RenameFileCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class RenameFileCmdObject : BaseCmdObject
{
    public int FolderId => this.CmdData.mlObjectID1;

    public int ParentFolderId => this.CmdData.mlObjectID2;

    public string OldFileName => this.CmdData.mbsStrData1;

    public string NewFileName => this.CmdData.mbsStrData2;
}
