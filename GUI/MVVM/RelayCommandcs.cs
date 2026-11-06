// <copyright file="RelayCommandcs.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Windows.Input;

public class RelayCommand(Action<object?> executeHandler, Func<object?, bool>? canExecuteHandler = null) : ICommand
{
    private readonly Action<object?> executeHandler = executeHandler;
    private readonly Func<object?, bool> canExecuteHandler = canExecuteHandler ?? (o => true);

    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <inheritdoc/>
    public bool CanExecute(object? parameter)
        => this.canExecuteHandler(parameter);

    /// <inheritdoc/>
    public void Execute(object? parameter)
    {
        if (!this.canExecuteHandler(parameter))
        {
            return;
        }

        this.executeHandler(parameter);
    }
}
