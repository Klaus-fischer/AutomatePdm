// <copyright file="MoveCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

public class MoveCmdObject : BaseCmdObject
{
    /// <summary>
    /// Gets iD of file to move.
    /// </summary>
    public int FileId => this.CmdData.mlObjectID1;

    /// <summary>
    /// Gets iD of source folder.
    /// </summary>
    public int SourceFolderId => this.CmdData.mlObjectID2;

    /// <summary>
    /// Gets iD of destination folder.
    /// </summary>
    public int DestinationFolderId => this.CmdData.mlObjectID3;

    /// <summary>
    /// Gets source file path.
    /// </summary>
    public string SourceFilePath => this.CmdData.mbsStrData1;

    /// <summary>
    /// Gets destination file path.
    /// </summary>
    public string DestinationFilePath => this.CmdData.mbsStrData2;
}
