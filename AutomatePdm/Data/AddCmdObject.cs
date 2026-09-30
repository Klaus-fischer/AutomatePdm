// <copyright file="AddCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;
public class AddCmdObject : BaseCmdObject
{
    public static AddCmdObject Empty { get; } = new AddCmdObject();

    public int FileId => this.CmdData.mlObjectID2;

    public int ParentFolderId => this.CmdData.mlObjectID1;

    public string FilePath => this.CmdData.mbsStrData1;
}
