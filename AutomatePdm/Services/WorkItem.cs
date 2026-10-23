// <copyright file="WorkItem.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Generic;
using EPDM.Interop.epdm;

public class WorkItem(IEdmFile5 file, IEdmFolder5 folder)
{
    internal IEdmFile5 File { get; } = file;

    internal IEdmFolder5 Folder { get; } = folder;

    public Dictionary<string, object> Properties { get; } = new();
}