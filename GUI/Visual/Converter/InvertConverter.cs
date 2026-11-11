// <copyright file="InvertConverter.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Globalization;
using System.Windows.Data;

/// <summary>
/// A Value converter to invert boolean values.
/// </summary>
[ValueConversion(typeof(bool), typeof(bool))]
public class InvertConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }

        return Binding.DoNothing;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => this.Convert(value, targetType, parameter, culture);
}
