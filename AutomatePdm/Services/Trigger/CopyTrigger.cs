// <copyright file="CopyTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using EPDM.Interop.epdm;

internal class PostCopyTrigger : TriggerBase
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostAdd;

    public string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.DestinationFolderId,
        TriggerExtensions.FileId,
        TriggerExtensions.SourceFolderId,
        TriggerExtensions.SourceFolderPath,
        TriggerExtensions.DestinationFolderPath,
    };

    protected override WorkItem Create(EdmCmdData data, ExecutionContext ctx)
    {
        var workItem = WorkItem.Create(data.mbsStrData2, ctx.Vault);

        workItem.Properties.Add(TriggerExtensions.DestinationFolderId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.SourceFolderId, data.mlObjectID3);
        workItem.Properties.Add(TriggerExtensions.SourceFolderPath, data.mbsStrData1);
        workItem.Properties.Add(TriggerExtensions.DestinationFolderPath, data.mbsStrData2);
        return workItem;
    }
}

internal class PreviewCopyTrigger : TriggerBase, IPreEventTrigger
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreCopy;

    public string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.DestinationFolderId,
        TriggerExtensions.FileId,
        TriggerExtensions.SourceFolderId,
        TriggerExtensions.SourceFolderPath,
        TriggerExtensions.DestinationFolderPath,
    };

    protected override WorkItem Create(EdmCmdData data, ExecutionContext ctx)
    {
        var workItem = WorkItem.Create(data.mlObjectID2, data.mlObjectID3, ctx.Vault);

        workItem.Properties.Add(TriggerExtensions.DestinationFolderId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.SourceFolderId, data.mlObjectID3);
        workItem.Properties.Add(TriggerExtensions.SourceFolderPath, data.mbsStrData1);
        workItem.Properties.Add(TriggerExtensions.DestinationFolderPath, data.mbsStrData2);
        return workItem;
    }

    public bool Cancel { get; set; }
}
