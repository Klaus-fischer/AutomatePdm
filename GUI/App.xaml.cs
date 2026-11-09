// <copyright file="App.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using EPDM.Interop.epdm;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : Application
{
    private readonly string vaultName;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        this.vaultName = ConfigurationManager.AppSettings["VaultName"] ?? string.Empty;

#if DEBUG
        this.InitializeOffline();
#else
        this.Initialize();
#endif
    }

    internal IEdmVault7 Vault { get; private set; }

    internal IStatusReader StatusReader { get; private set; }

    [MemberNotNull(nameof(Vault), nameof(StatusReader))]
    private void Initialize()
    {
        this.Vault = new EdmVault5();
        this.Vault.LoginAuto(this.vaultName, 0);
        this.StatusReader = new StatusReader((IEdmWorkflowMgr6)this.Vault.CreateUtility(EdmUtility.EdmUtil_WorkflowMgr));
    }

    [MemberNotNull(nameof(Vault), nameof(StatusReader))]
    private void InitializeOffline()
    {
        this.Vault = null!;
        this.StatusReader = new StatusReaderMock();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        this.Resources["StatusReader"] = this.StatusReader;
        base.OnStartup(e);
    }
}
