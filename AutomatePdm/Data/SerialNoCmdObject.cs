// <copyright file="SerialNoCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

using EPDM.Interop.epdm;

public class SerialNoCmdObject : BaseCmdObject
{
    public int FileId => this.CmdData.mlObjectID1;

    public int FolderId => this.CmdData.mlObjectID2;

    public int CardId => this.CmdData.mlObjectID3;

    public int CardControlId => this.CmdData.mlObjectID4;

    public string GeneratedSerialNumber { get; set; } = string.Empty;

    public string FilePath => this.CmdData.mbsStrData2;

    public string ConfigurationName => this.CmdData.mbsStrData3;

    public int SerialNumber => this.CmdData.mlLongData1;

    protected override void OnAssign()
    {
        this.GeneratedSerialNumber = this.CmdData.mbsStrData1;
    }

    protected override void OnUpdateValues(ref EdmCmd cmd, ref EdmCmdData data)
    {
        data.mbsStrData1 = this.GeneratedSerialNumber;
    }
}
