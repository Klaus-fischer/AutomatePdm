// <copyright file="BoolToKeyConverter.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Globalization;
using System.Windows.Data;

[ValueConversion(typeof(bool), typeof(object))]
public class BoolToKeyConverter : IValueConverter
{
    public object TrueValue { get; set; } = Binding.DoNothing;

    public object FalseValue { get; set; } = Binding.DoNothing;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? this.TrueValue : this.FalseValue;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (Equals(value, this.TrueValue))
        {
            return true;
        }
        else if (Equals(value, this.FalseValue))
        {
            return false;
        }

        return Binding.DoNothing;
    }
}
