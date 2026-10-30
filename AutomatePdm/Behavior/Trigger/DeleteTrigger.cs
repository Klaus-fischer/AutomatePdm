// <copyright file="DeleteTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;

public class PostDeleteTrigger : TriggerBase
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PostDelete;

    public override string[] ProvidedProperties => new string[]
    {
        TriggerExtensions.FileId,
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.FilePath,
    };

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFileId => d => d.mlObjectID1;

    /// <inheritdoc/>
    protected override Func<EdmCmdData, int> GetFolderId => d => d.mlObjectID2;

    /// <inheritdoc/>
    protected override void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.AddProperty(TriggerExtensions.FileId, data.mlObjectID1);
        workItem.AddProperty(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.FilePath, data.mbsStrData1);
    }
}

public class PreviewDeleteTrigger : PostDeleteTrigger, IPreEventTrigger
{
    /// <inheritdoc/>
    public override EdmCmdType CmdType => EdmCmdType.EdmCmd_PreDelete;

    /// <inheritdoc/>
    public bool Cancel { get; set; }
}
