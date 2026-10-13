// <copyright file="AddTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using EPDM.Interop.epdm;

internal class PostAddTrigger : TriggerBase
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostAdd;

    public string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.FileId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.FilePath,
    };

    protected override WorkItem Create(EdmCmdData data, ExecutionContext ctx)
    {
        var workItem = WorkItem.Create(data.mlObjectID2, data.mlObjectID1, ctx.Vault);
        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData1);
        return workItem;
    }
}

internal class PreviewAddTrigger : TriggerBase, IPreEventTrigger
{
    public string[] ProvidedProperties => new string[]
    {
            TriggerExtensions.ParentFolderId,
            TriggerExtensions.FilePath,
    };

    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreAdd;

    protected override WorkItem Create(EdmCmdData data, ExecutionContext ctx)
    {
        var workItem = WorkItem.Create(0, data.mlObjectID2, ctx.Vault);
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData1);
        return workItem;
    }

    public bool Cancel { get; set; }
}
