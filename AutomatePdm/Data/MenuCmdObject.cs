// <copyright file="MenuCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

using EPDM.Interop.epdm;
using System;
using System.Collections.Generic;
using System.Linq;

public class MenuCmdObject : BaseCmdObject
{
    /// <summary>
    /// Gets the ID of file; 0 if a folder is selected.
    /// </summary>
    public int FileId => this.CmdData.mlObjectID1;

    /// <summary>
    /// Gets the ID of folder; 0 if a file is selected.
    /// </summary>
    public int FolderId => this.CmdData.mlObjectID2;

    /// <summary>
    /// Gets the ID of parent folder of the selected file or folder.
    /// </summary>
    public int ParentFolderId => this.CmdData.mlObjectID3;

    /// <summary>
    /// Gets the Name of file or folder, not the full path.
    /// </summary>
    public string Name => this.CmdData.mbsStrData1;

    public int CommandId => this.Command.mlCmdID;

    public IReadOnlyList<MenuCmdItem> PpoCmdData { get; private set; } = Array.Empty<MenuCmdItem>();

    protected override void OnAssign(EdmCmdData[] allCmdData)
    {
        this.PpoCmdData = allCmdData
            .Select(o => new MenuCmdItem(o))
            .ToList()
            .AsReadOnly();
    }
}

public class MenuCmdItem
{
    private EdmCmdData cmdData;

    public MenuCmdItem(EdmCmdData cmdData)
    {
        this.cmdData = cmdData;
    }

    /// <summary>
    /// Gets the ID of file; 0 if a folder is selected.
    /// </summary>
    public int FileId => this.cmdData.mlObjectID1;

    /// <summary>
    /// Gets the ID of folder; 0 if a file is selected.
    /// </summary>
    public int FolderId => this.cmdData.mlObjectID2;

    /// <summary>
    /// Gets the ID of parent folder of the selected file or folder.
    /// </summary>
    public int ParentFolderId => this.cmdData.mlObjectID3;

    /// <summary>
    /// Gets the Name of file or folder, not the full path.
    /// </summary>
    public string Name => this.cmdData.mbsStrData1;
}
