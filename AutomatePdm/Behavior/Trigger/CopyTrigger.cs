// <copyright file="CopyTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;

public class PostCopyTrigger : TriggerBase
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostAdd;

    /// <inheritdoc/>
    public override string[] ProvidedProperties =>
    [
        TriggerExtensions.DestinationFolderId,
        TriggerExtensions.FileId,
        TriggerExtensions.SourceFolderId,
        TriggerExtensions.SourceFolderPath,
        TriggerExtensions.DestinationFolderPath,
    ];

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => d => d.mlObjectID2;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => d => d.mlObjectID3; // may also be mlObjectID1, depends on file id changed pre Copy (old file), post copy (new file)

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.AddProperty(TriggerExtensions.DestinationFolderId, data.mlObjectID1);
        workItem.AddProperty(TriggerExtensions.FileId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.SourceFolderId, data.mlObjectID3);
        workItem.AddProperty(TriggerExtensions.SourceFolderPath, data.mbsStrData1);
        workItem.AddProperty(TriggerExtensions.DestinationFolderPath, data.mbsStrData2);
    }
}

public class PreviewCopyTrigger : TriggerBase, IPreEventTrigger
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreCopy;

    public override string[] ProvidedProperties =>
    [
        TriggerExtensions.DestinationFolderId,
        TriggerExtensions.FileId,
        TriggerExtensions.SourceFolderId,
        TriggerExtensions.SourceFolderPath,
        TriggerExtensions.DestinationFolderPath,
    ];

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => d => d.mlObjectID2;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => d => d.mlObjectID3;

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.AddProperty(TriggerExtensions.DestinationFolderId, data.mlObjectID1);
        workItem.AddProperty(TriggerExtensions.FileId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.SourceFolderId, data.mlObjectID3);
        workItem.AddProperty(TriggerExtensions.SourceFolderPath, data.mbsStrData1);
        workItem.AddProperty(TriggerExtensions.DestinationFolderPath, data.mbsStrData2);
    }

    /// <inheritdoc/>
    public bool Cancel { get; set; }
}
