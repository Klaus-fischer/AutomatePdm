// <copyright file="AddTrigger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;

public class PostAddTrigger : TriggerBase
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
        workItem.AddProperty(TriggerExtensions.FileId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.ParentFolderId, data.mlObjectID1);
        workItem.AddProperty(TriggerExtensions.FilePath, data.mbsStrData1);
    }
}

internal class PreviewAddTrigger : ITrigger, IPreEventTrigger
{
    /// <inheritdoc/>
    public string[] ProvidedProperties =>
    [
        TriggerExtensions.ParentFolderId,
        TriggerExtensions.FilePath,
    ];

    /// <inheritdoc/>
    public EdmCmdType CmdType => EdmCmdType.EdmCmd_PreAdd;

    /// <inheritdoc/>
    public bool Cancel { get; set; }

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public void Execute(ref EdmCmdData[] data, ExecutionContext ctx)
    {
        var workItems = this.CreateWorkItems(data, ctx);
        this.Next?.Execute(workItems, ctx);
    }

    private WorkItem[] CreateWorkItems(EdmCmdData[] data, ExecutionContext ctx)
    {
        if (ctx.Vault is not IEdmVault9 vault)
        {
            throw new InvalidOperationException("Vault does not support IEdmVault9 interface.");
        }

        var folders = GetFolders(data, vault);

        var result = new List<WorkItem>(data.Length);

        foreach (var d in data)
        {
            var folderObjectInfo = folders[d.mlObjectID2];

            if (folderObjectInfo.mhResult != 0)
            {
                continue;
            }

            var item = new WorkItem(null!, (IEdmFolder5)folderObjectInfo.mpoObject);
            this.EnrichProperties(d, item, ctx);
            result.Add(item);
        }

        return [.. result];
    }

    private static Dictionary<object, EdmObjectInfo> GetFolders(EdmCmdData[] data, IEdmVault9 vault)
    {
        var folderSelection = data
           .Select(d => d.mlObjectID2)
           .Select(id => new EdmObjectInfo { moObjectID = id, meType = EdmObjectType.EdmObject_Folder, });

        var folderObjects = folderSelection.Concat(folderSelection).ToArray();

        vault.GetObjects(ref folderObjects);

        var folders = folderSelection.ToDictionary(o => o.moObjectID);
        return folders;
    }

    private void EnrichProperties(EdmCmdData data, WorkItem workItem, ExecutionContext ctx)
    {
        workItem.AddProperty(TriggerExtensions.ParentFolderId, data.mlObjectID2);
        workItem.AddProperty(TriggerExtensions.FilePath, data.mbsStrData1);
    }
}
