// <copyright file="DeleteTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using EPDM.Interop.epdm;

internal class PostDeleteTrigger : TriggerBase
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostDelete;

    public string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.FileId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.FilePath,
    };

    protected override WorkItem Create(EdmCmdData data, ExecutionContext ctx)
    {
        var workItem = WorkItem.Create(data.mlObjectID1, data.mlObjectID2, ctx.Vault);
        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData1);
        return workItem;
    }
}

internal class PreviewDeleteTrigger : PostDeleteTrigger, IPreEventTrigger
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreDelete;

    public bool Cancel { get; set; }
}
