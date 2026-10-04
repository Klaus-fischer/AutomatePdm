// <copyright file="StateChangeTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using EPDM.Interop.epdm;

internal class PostStateChangeTrigger : TriggerBase
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostState;

    public string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.FileId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.StateTransitionId,
        TriggerExtensions.UserId,
        TriggerExtensions.FilePath,
        TriggerExtensions.DestinationStateName,
        TriggerExtensions.SourceStateId,
        TriggerExtensions.DestinationStateId,
    };

    protected override WorkItem Create(EdmCmdData data, ExecutionContext ctx)
    {
        var workItem = WorkItem.Create(data.mlObjectID1, data.mlObjectID2, ctx.Vault);

        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.StateTransitionId, data.mlObjectID3);
        workItem.Properties.Add(TriggerExtensions.UserId, data.mlObjectID4);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData1);
        workItem.Properties.Add(TriggerExtensions.DestinationStateName, data.mbsStrData2);
        workItem.Properties.Add(TriggerExtensions.SourceStateId, data.mlLongData1);
        workItem.Properties.Add(TriggerExtensions.DestinationStateId, data.mlLongData2);
        return workItem;
    }
}

internal class PreviewStateChangeTrigger : PostStateChangeTrigger, IPreEventTrigger
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreState;

    public bool Cancel { get; set; }
}
