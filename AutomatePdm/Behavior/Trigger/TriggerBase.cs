// <copyright file="TriggerBase.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;

public abstract class TriggerBase : ITrigger
{
    /// <inheritdoc/>
    public abstract EdmCmdType CmdType { get; }

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public abstract string[] ProvidedProperties { get; }

    /// <summary>
    /// Gets the file Id from the EdmCmdData in the specific Trigger.
    /// </summary>
    protected abstract Func<EdmCmdData, int> GetFileId { get; }

    /// <summary>
    /// Gets the folder Id of parent Folder from the EdmCmdData in the specific Trigger.
    /// </summary>
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

        var filesSelection = data.Select(this.GetFileId);
        var folderSelection = data.Select(this.GetFolderId);

        var (files, folders) = vault.GetFilesAndFolder(filesSelection, folderSelection);
        var result = new List<WorkItem>(data.Length);

        foreach (var d in data)
        {
            var file = files[this.GetFileId(d)];
            var folder = folders[this.GetFolderId(d)];

            var item = new WorkItem(file, folder);
            this.EnrichProperties(d, item, ctx);
            result.Add(item);
        }

        return result.ToArray();
    }

    protected abstract void EnrichProperties(EdmCmdData data, WorkItem item, ExecutionContext ctx);
}
