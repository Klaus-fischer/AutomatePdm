// <copyright file="TriggerBase.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;

internal abstract class TriggerBase : ITrigger
{
    /// <inheritdoc/>
    public abstract EdmCmdType CmdType { get; }

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public abstract string[] ProvidedProperties { get; }

    protected abstract Func<EdmCmdData, int> GetFileId { get; }

    protected abstract Func<EdmCmdData, int> GetFolderId { get; }

    /// <inheritdoc/>
    public void Execute(ref EdmCmdData[] data, ExecutionContext ctx)
    {
        WorkItem[] workItems = this.CreateMany(data, ctx);

        this.Next?.Execute(workItems, ctx);
    }

    private WorkItem[] CreateMany(EdmCmdData[] data, ExecutionContext ctx)
    {
        if (ctx.Vault is not IEdmVault9 vault)
        {
            throw new InvalidOperationException("Vault does not support IEdmVault9 interface.");
        }

        var filesSelection = data
            .Select(this.GetFileId)
            .Select(id => new EdmObjectInfo { moObjectID = id, meType = EdmObjectType.EdmObject_File, });

        var folderSelection = data
            .Select(this.GetFolderId)
            .Select(id => new EdmObjectInfo { moObjectID = id, meType = EdmObjectType.EdmObject_Folder, });

        var objects = filesSelection.Concat(folderSelection).ToArray();

        vault.GetObjects(ref objects);

        var files = objects.Where(o => o.meType == EdmObjectType.EdmObject_File)
                           .ToDictionary(f => (int)f.moObjectID);
        var folders = objects.Where(o => o.meType == EdmObjectType.EdmObject_Folder)
                             .ToDictionary(f => (int)f.moObjectID);

        var result = new List<WorkItem>(data.Length);

        foreach (var d in data)
        {
            var fileObjectInfo = files[this.GetFileId(d)];
            var folderObjectInfo = folders[this.GetFolderId(d)];

            if (fileObjectInfo.mhResult != 0 || folderObjectInfo.mhResult != 0)
            {
                continue;
            }

            var item = new WorkItem((IEdmFile5)fileObjectInfo.mpoObject, (IEdmFolder5)folderObjectInfo.mpoObject);
            this.EnrichProperties(d, item, ctx);
            result.Add(item);
        }

        return result.ToArray();
    }

    protected abstract void EnrichProperties(EdmCmdData data, WorkItem item, ExecutionContext ctx);
}
