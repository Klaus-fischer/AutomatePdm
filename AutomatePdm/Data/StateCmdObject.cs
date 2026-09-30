// <copyright file="StateCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using EPDM.Interop.epdm;

public class StateCmdObject : BaseCmdObject
{
    public IReadOnlyList<int> TransitionFileIds { get; private set; } = Array.Empty<int>();

    /// <summary>
    /// Gets 'mlObjectID1' of EdmCmdData.
    /// </summary>
    public int FileId => this.CmdData.mlObjectID1;

    /// <summary>
    /// Gets 'mlObjectID2' of EdmCmdData.
    /// </summary>
    public int ParentFolderId => this.CmdData.mlObjectID2;

    /// <summary>
    /// Gets 'mlObjectID3' of EdmCmdData.
    /// </summary>
    public int StateTransitionId => this.CmdData.mlObjectID3;

    /// <summary>
    /// Gets 'mlObjectID4' of EdmCmdData.
    /// </summary>
    public int UserId => this.CmdData.mlObjectID4;

    /// <summary>
    /// Gets 'mbsStrData1' of EdmCmdData.
    /// </summary>
    public string FilePath => this.CmdData.mbsStrData1;

    /// <summary>
    /// Gets 'mbsStrData2' of EdmCmdData.
    /// </summary>
    public string DestinationStateName => this.CmdData.mbsStrData2;

    /// <summary>
    /// Gets 'mlLongData1' of EdmCmdData.
    /// </summary>
    public int SourceStateId => this.CmdData.mlLongData1;

    /// <summary>
    /// Gets 'mlLongData2' of EdmCmdData.
    /// </summary>
    public int DestinationStateId => this.CmdData.mlLongData2;

    protected override void OnAssign(EdmCmdData[] allCmdData)
    {
        this.TransitionFileIds = allCmdData.Select(o => o.mlObjectID1).ToList().AsReadOnly();
    }
}
