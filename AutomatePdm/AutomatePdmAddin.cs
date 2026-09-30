// <copyright file="AutomatePdmAddin.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;
using System.Runtime.InteropServices;
using AutomatePdm.Services;
using EPDM.Interop.epdm;

/// <summary>
/// The entry main point for this add-in.
/// </summary>
[ComVisible(true)]
[Guid("0743AC4B-7229-450C-A7AF-51EF04AC9DD1")]
public class AutomatePdmAddin : IEdmAddIn5
{
    private const int AdminSetupCmdId = 1;

    public AutomatePdmAddin()
    {
        this.PipeBuilder = new PipeBuilderService();
    }

    internal IEdmVault5 Vault { get; set; } = null!;

    internal PipeBuilderService PipeBuilder { get; }

    /// <inheritdoc/>
    public void GetAddInInfo(ref EdmAddInInfo poInfo, IEdmVault5 poVault, IEdmCmdMgr5 poCmdMgr)
    {
        this.Vault = poVault;
        this.SetAddinInfo(ref poInfo);
        this.RegisterAdminSetupCmd(poCmdMgr);
        this.RegisterHooks(poCmdMgr);
    }

    /// <inheritdoc/>
    public void OnCmd(ref EdmCmd poCmd, ref EdmCmdData[] ppoData)
    {
        this.Vault = (IEdmVault5)poCmd.mpoVault;

        if (poCmd.meCmdType == EdmCmdType.EdmCmd_Menu &&
            poCmd.mlCmdID == AdminSetupCmdId)
        {
            this.AdminSetup();
            return;
        }

        this.HandleCommands(ref poCmd, ref ppoData);
    }

    private void SetAddinInfo(ref EdmAddInInfo poInfo)
    {
        poInfo.mlAddInVersion = 1;
        poInfo.mbsAddInName = "AutomatePDM";
        poInfo.mbsCompany = "SIM-Automation";
        poInfo.mbsDescription = "Enables advanced automation rules to automate your PDM";
        poInfo.mlRequiredVersionMajor = 32;
        poInfo.mlRequiredVersionMinor = 5;
    }

    private void RegisterAdminSetupCmd(IEdmCmdMgr5 poCmdMgr)
    {
        poCmdMgr.AddCmd(AdminSetupCmdId, "Einstellungen", (int)EdmMenuFlags.EdmMenu_Administration);
        poCmdMgr.AddHook(EdmCmdType.EdmCmd_Menu);
    }

    private void AdminSetup()
    {
    }

    private void RegisterHooks(IEdmCmdMgr5 poCmdMgr)
    {
        var hooks = this.PipeBuilder.GetHooks(this.Vault);
        foreach (var hook in hooks)
        {
            poCmdMgr.AddHook(hook);
        }
    }

    private void HandleCommands(ref EdmCmd poCmd, ref EdmCmdData[] ppoData)
    {
        var pipe
    }
}
