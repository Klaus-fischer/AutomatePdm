// <copyright file="IEdmVariableMgr7Extensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Generic;
using EPDM.Interop.epdm;

public static class IEdmVariableMgr7Extensions
{
    /// <summary>
    /// Enumerates all variables of the vault.
    /// </summary>
    /// <param name="variableMgr">The variable manager.</param>
    /// <returns>An Enumeration of <see cref="IEdmVariable5"/>.</returns>
    public static IEnumerable<IEdmVariable5> GetVariables(this IEdmVariableMgr7 variableMgr)
    {
        var pos = variableMgr.GetFirstVariablePosition();
        while (!pos.IsNull)
        {
            yield return variableMgr.GetNextVariable(pos);
        }
    }
}
