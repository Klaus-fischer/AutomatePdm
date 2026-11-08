// <copyright file="TaskRunTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;

public class TaskRunTrigger : TriggerBase
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_TaskRun;

    /// <inheritdoc/>
    public override string[] ProvidedProperties =>
    [
        TriggerExtensions.FileId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.FilePath,
        TriggerExtensions.TaskName,
    ];

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => data => data.mlObjectID1;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => data => data.mlObjectID2;

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.AddProperty(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.AddProperty(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.FilePath, data.mbsStrData1);

        if (ctx.Extra is IEdmTaskProperties taskProperties)
        {
            workItem.AddProperty(TriggerExtensions.TaskName, taskProperties.TaskName);
            workItem.AddProperty(TriggerExtensions.UserName, taskProperties.UserName);
        }
    }
}
