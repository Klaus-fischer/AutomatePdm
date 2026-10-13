// <copyright file="MenuTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using EPDM.Interop.epdm;

internal class MenuTrigger : TriggerBase
{
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_Menu;

    public string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.FileId,
        TriggerExtensions.FolderId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.Name,
        TriggerExtensions.FilePath,
    };

    protected override WorkItem Create(EdmCmdData data, ExecutionContext ctx)
    {
        var workItem = WorkItem.Create(data.mlObjectID1, data.mlObjectID3, ctx.Vault);
        workItem.Properties.Add(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.Properties.Add(TriggerExtensions.FolderId, data.mlObjectID2);
        workItem.Properties.Add(TriggerExtensions.ParentFolderId, data.mlObjectID3);
        workItem.Properties.Add(TriggerExtensions.Name, data.mbsStrData1);
        workItem.Properties.Add(TriggerExtensions.FilePath, data.mbsStrData2);
        return workItem;
    }
}