// <copyright file="WorkItem.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using EPDM.Interop.epdm;
using System.Collections.Generic;

public class WorkItem
{
    private readonly Dictionary<string, string> stringProperties = new();
    private readonly Dictionary<string, int> intProperties = new();
    private Lazy<string> filePathReader;

    public WorkItem(IEdmFile5 file, IEdmFolder5 folder)
    {
        this.File = file;
        this.Folder = folder;
        this.filePathReader = new Lazy<string>(() => this.File.GetLocalPath(this.Folder.ID));
    }

    internal IEdmFile5 File { get; }

    internal IEdmFolder5 Folder { get; }

    public void AddProperty(string key, string value)
    {
        this.stringProperties[key] = value;
    }

    public string? GetStringProperty(string key)
    {
        if (key == TriggerExtensions.FilePath)
        {
            if (!this.stringProperties.TryGetValue(TriggerExtensions.FilePath, out var value))
            {
                value = this.filePathReader.Value;
                this.stringProperties.Add(TriggerExtensions.FilePath, value);
            }

            return value;
        }

        return this.stringProperties.TryGetValue(key, out var result) ? result : null;
    }

    public void AddProperty(string key, int value)
    {
        this.intProperties[key] = value;
    }

    public int? GetIntProperty(string key)
    {
        return this.intProperties.TryGetValue(key, out var result) ? result : null;
    }
}