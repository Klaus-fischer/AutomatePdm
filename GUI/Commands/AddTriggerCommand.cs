// <copyright file="AddTriggerCommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

internal class AddTriggerCommand(MainViewModel viewModel) : ViewModelCommand<MainViewModel>(viewModel)
{
    /// <inheritdoc/>
    protected override void OnExecute(object? parameter)
    {
        var trigger = new TriggerViewModel()
        {
            EditTriggerCommand = this.ViewModel.EditTriggerCommand,
        };
        this.ViewModel.Triggers.Add(trigger);
        this.ViewModel.Triggers.SelectedItem = trigger;
        this.ViewModel.Popup = trigger;
    }
}
