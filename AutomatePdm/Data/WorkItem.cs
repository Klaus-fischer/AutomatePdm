// <copyright file="WorkItem.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Generic;
using EPDM.Interop.epdm;

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

    public IEdmFile5 File { get; }

    public IEdmFolder5 Folder { get; }

    public string FilePath
    {
        get => this.filePathReader.Value;
        set => this.filePathReader = new Lazy<string>(() => value, true);
    }

    public string FileName => Path.GetFileName(this.FilePath);

    public void AddProperty(string key, string value)
    {
        if (key == TriggerExtensions.FilePath)
        {
            this.FilePath = value;
            return;
        }

        this.stringProperties[key] = value;
    }

    public string? GetStringProperty(string key)
    {
        if (key == TriggerExtensions.FilePath)
        {
            return this.FilePath;
        }

        if (key == TriggerExtensions.FileName)
        {
            return this.FileName;
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