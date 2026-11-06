// <copyright file="ViewModelCommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.ComponentModel;
using System.Windows.Input;

/// <summary>
/// Declares an view model command.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ViewModelCommand<T> : ICommand
    where T : INotifyPropertyChanged
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelCommand{T}"/> class.
    /// </summary>
    /// <param name="viewModel">The dependent view model.</param>
    public ViewModelCommand(T viewModel)
    {
        this.ViewModel = viewModel;
        this.ViewModel.PropertyChanged += this.OnViewModelPropertyChanged;
    }

    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Gets  the target view model of this command.
    /// </summary>
    public T ViewModel { get; }

    /// <inheritdoc/>
    public virtual bool CanExecute(object? parameter) => true;

    /// <inheritdoc/>
    public void Execute(object? parameter)
    {
        if (!this.CanExecute(parameter))
        {
            return;
        }

        this.OnExecute(parameter);
    }

    /// <summary>
    /// the execute logic.
    /// </summary>
    /// <param name="parameter">The parameter of the command.</param>
    protected abstract void OnExecute(object? parameter);

    /// <summary>
    /// If Command depends on Properties of the view model return true.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    /// <returns>True if command depends on the property.</returns>
    protected virtual bool DependsOn(string propertyName) => false;

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (this.DependsOn(e.PropertyName ?? string.Empty))
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
