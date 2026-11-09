// <copyright file="EditScriptCommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

internal class EditScriptCommand(MainViewModel viewModel) : ViewModelCommand<MainViewModel>(viewModel)
{
    public override bool CanExecute(object? parameter)
        => parameter is ProcessStepViewModel;

    /// <inheritdoc/>
    protected override void OnExecute(object? parameter)
    {
        if (parameter is not ProcessStepViewModel processStepViewModel)
        {
            return;
        }

        this.ViewModel.Popup = processStepViewModel;
        this.ViewModel.PopupTitle = "Eigenschaften bearbeiten";
    }
}
