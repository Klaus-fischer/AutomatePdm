// <copyright file="StateChangeTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;

public class PostStateChangeTrigger : TriggerBase
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostState;

    /// <inheritdoc/>
    public override string[] ProvidedProperties =>
    [
        TriggerExtensions.FileId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.StateTransitionId,
        TriggerExtensions.UserId,
        TriggerExtensions.FilePath,
        TriggerExtensions.DestinationStateName,
        TriggerExtensions.SourceStateId,
        TriggerExtensions.DestinationStateId,
    ];

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => d => d.mlObjectID1;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => d => d.mlObjectID2;

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.AddProperty(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.AddProperty(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.StateTransitionId, data.mlObjectID3);
        workItem.AddProperty(TriggerExtensions.UserId, data.mlObjectID4);
        workItem.AddProperty(TriggerExtensions.FilePath, data.mbsStrData1);
        workItem.AddProperty(TriggerExtensions.DestinationStateName, data.mbsStrData2);
        workItem.AddProperty(TriggerExtensions.SourceStateId, data.mlLongData1);
        workItem.AddProperty(TriggerExtensions.DestinationStateId, data.mlLongData2);
    }
}

internal class PreviewStateChangeTrigger : PostStateChangeTrigger, IPreEventTrigger
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreState;

    /// <inheritdoc/>
    public bool Cancel { get; set; }
}
