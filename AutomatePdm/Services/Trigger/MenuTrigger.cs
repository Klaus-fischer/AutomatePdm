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
        workItem.AddProperty(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.AddProperty(TriggerExtensions.FolderId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.ParentFolderId, data.mlObjectID3);
        workItem.AddProperty(TriggerExtensions.Name, data.mbsStrData1);
        workItem.AddProperty(TriggerExtensions.FilePath, data.mbsStrData2);
    }
}