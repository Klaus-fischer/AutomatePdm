// <copyright file="PipeBuilderService.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Services;

using System.Collections.Generic;

using EPDM.Interop.epdm;

internal class PipeBuilderService
{
    public IEnumerable<EdmCmdType> GetHooks(IEdmVault5 vault) { yield break; }

    public IPipeline Build(EdmCmd poCmd, IEdmVault5 vault)
    {
        return null;
    }
}

internal interface IPipeline
{
    void Execute(ref EdmCmd poCmd, ref EdmCmdData[] ppoData);
}