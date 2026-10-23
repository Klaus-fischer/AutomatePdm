// <copyright file="AddTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using EPDM.Interop.epdm;

internal class PostAddTrigger : TriggerBase
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostAdd;

    /// <inheritdoc/>
    public override string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.FileId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.FilePath,
    };

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => d => d.mlObjectID2;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => d => d.mlObjectID1;

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData1);
    }
}

internal class PreviewAddTrigger : TriggerBase, IPreEventTrigger
{
    /// <inheritdoc/>
    public override string[] ProvidedProperties =>
    [
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.FilePath,
    ];

    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreAdd;

    /// <inheritdoc/>
    public bool Cancel { get; set; }

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => d => 0;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => d => d.mlObjectID2;

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData1);
    }
}
