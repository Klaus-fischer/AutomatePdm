// <copyright file="ViewModelBase.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ViewModelBase : INotifyPropertyChanged
{
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Setter method for a Property to invoke PropertyChanged notification.
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <param name="current">The current value as reference.</param>
    /// <param name="value">The new value.</param>
    /// <param name="alsoNotify">Name of dependent properties to also notify.</param>
    /// <param name="propertyName">Name of the property.</param>
    protected void Set<T>(ref T current, T value, string[]? alsoNotify = null, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(current, value))
        {
            return;
        }

        current = value;

        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (alsoNotify is not null)
        {
            foreach (var property in alsoNotify)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
