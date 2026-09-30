// <copyright file="CopyCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class CopyCmdObject : BaseCmdObject
{
    public int FileId => this.CmdData.mlObjectID2;

    public int DestinationFolderId => this.CmdData.mlObjectID1;

    public int SourceFolderId => this.CmdData.mlObjectID3;

    public string SourceFilePath => this.CmdData.mbsStrData1;

    public string DestionationFilePath => this.CmdData.mbsStrData2;
}
