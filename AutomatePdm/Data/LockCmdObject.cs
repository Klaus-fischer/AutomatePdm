// <copyright file="LockCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class LockCmdObject : BaseCmdObject
{
    public int FileId => this.CmdData.mlObjectID1;

    public int ParentFolderId => this.CmdData.mlObjectID2;

    public string FilePath => this.CmdData.mbsStrData1;
}
