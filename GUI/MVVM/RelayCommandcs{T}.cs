// <copyright file="RelayCommandcs{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Windows.Input;

public class RelayCommand<T>(Action<T> executeHandler, Func<T?, bool>? canExecuteHandler = null) : ICommand
    where T : class
{
    private readonly Action<T> executeHandler = executeHandler;
    private readonly Func<T?, bool> canExecuteHandler = canExecuteHandler ?? (o => true);

    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <inheritdoc/>
    public bool CanExecute(object? parameter)
        => parameter is T arg && this.canExecuteHandler(arg);

    /// <inheritdoc/>
    public void Execute(object? parameter)
    {
        if (parameter is not T arg || !this.canExecuteHandler(arg))
        {
            return;
        }

        this.executeHandler(arg);
    }
}
