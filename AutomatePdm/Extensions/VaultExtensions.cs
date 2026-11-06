// <copyright file="VaultExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Generic;
using EPDM.Interop.epdm;

internal static class VaultExtensions
{
    public static (IDictionary<int, IEdmFile5> Files, IDictionary<int, IEdmFolder5> Folders) GetFilesAndFolder(this IEdmVault9 vault, IEnumerable<int> fileIds, IEnumerable<int> folderIds)
    {
        var filesSelection = fileIds
            .Distinct()
            .Select(id => new EdmObjectInfo { moObjectID = id, meType = EdmObjectType.EdmObject_File, });

        var folderSelection = folderIds
            .Distinct()
            .Select(id => new EdmObjectInfo { moObjectID = id, meType = EdmObjectType.EdmObject_Folder, });

        var objects = filesSelection.Concat(folderSelection).ToArray();

        vault.GetObjects(ref objects);

        var files = objects.Where(o => o.meType == EdmObjectType.EdmObject_File)
                           .Where(o => o.mhResult == 0)
                           .ToDictionary(f => (int)f.moObjectID, f => (IEdmFile5)f.mpoObject);

        var folders = objects.Where(o => o.meType == EdmObjectType.EdmObject_Folder)
                             .Where(o => o.mhResult == 0)
                             .ToDictionary(f => (int)f.moObjectID, f => (IEdmFolder5)f.mpoObject);

        return (files, folders);
    }
}
