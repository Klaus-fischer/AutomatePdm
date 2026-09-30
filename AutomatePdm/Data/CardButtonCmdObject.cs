// <copyright file="CardButtonCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

using EPDM.Interop.epdm;

public class CardButtonCmdObject : BaseCmdObject
{
    public string Comment => this.Command.mbsComment;

    /// <summary>
    /// Gets the ID of file; 0 if a folder is selected.
    /// </summary>
    public int FileId => this.CmdData.mlObjectID1;

    /// <summary>
    /// Gets the ID of folder; 0 if a file is selected.
    /// </summary>
    public int FolderId => this.CmdData.mlObjectID2;

    /// <summary>
    /// Gets the ID of file data card.
    /// </summary>
    public int CardId => this.CmdData.mlObjectID3;

    /// <summary>
    /// Gets or sets the Name of active configuration. Can be changed to switch to a new configuration.
    /// </summary>
    public string ActiveConfigurationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the Path to file.
    /// </summary>
    public string PathToFile => this.CmdData.mbsStrData2;

    /// <summary>
    /// Gets or sets Optionally return a EdmCardFlag return code here.
    /// </summary>
    public EdmCardFlag ReturnCode { get; set; }

    /// <summary>
    /// Gets or sets Optionally return the ID of a card control to set focus to here.
    /// </summary>
    public int IdOfNextCardControl { get; set; }

    /// <summary>
    /// Gets the Pointer to an IEdmStrLst5 interface with the names of all configurations.
    /// </summary>
    public IEdmStrLst5 ConfigurationNames { get; private set; } = null!;

    public IEdmEnumeratorVariable8 Variables { get; private set; } = null!;

    protected override void OnAssign()
    {
        this.ActiveConfigurationName = this.CmdData.mbsStrData1;
        this.ReturnCode = (EdmCardFlag)this.CmdData.mlLongData1;
        this.IdOfNextCardControl = this.CmdData.mlLongData2;
        this.ConfigurationNames = (IEdmStrLst5)this.CmdData.mpoExtra;
        this.Variables = (IEdmEnumeratorVariable8)this.Command.mpoExtra;
    }

    protected override void OnUpdateValues(ref EdmCmd cmd, ref EdmCmdData data)
    {
        data.mbsStrData1 = this.ActiveConfigurationName;
        data.mlLongData1 = (int)this.ReturnCode;
        data.mlLongData2 = this.IdOfNextCardControl;
    }
}
