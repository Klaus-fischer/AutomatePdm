// <copyright file="EditTriggerCommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

internal class EditTriggerCommand(MainViewModel viewModel) : ViewModelCommand<MainViewModel>(viewModel)
{
    public override bool CanExecute(object? parameter)
        => parameter is TriggerViewModel;

    /// <inheritdoc/>
    protected override void OnExecute(object? parameter)
    {
        if (parameter is not TriggerViewModel triggerViewModel)
        {
            return;
        }

        this.ViewModel.Popup = triggerViewModel;
        this.ViewModel.PopupTitle = "Eigenschaften bearbeiten";
    }
}
