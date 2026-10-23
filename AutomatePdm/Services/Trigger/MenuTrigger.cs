// <copyright file="MenuTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using EPDM.Interop.epdm;

internal class MenuTrigger : TriggerBase
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_Menu;

    /// <inheritdoc/>
    public override string[] ProvidedProperties =>
    [
        TriggerExtensions.FileId,
        TriggerExtensions.FolderId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.Name,
        TriggerExtensions.FilePath,
    ];

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => d => d.mlObjectID1;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => d => d.mlObjectID3;

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.FolderId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID3);
        workItem.Properties.Add(TriggerExtensions.Name, data.mbsStrData1);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData2);
    }
}