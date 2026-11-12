// <copyright file="Class1.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.Animation;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

internal static class DefinitionParser
{
    public static List<GridLength> Parse(string? input)
    {
        var result = new List<GridLength>();
        if (string.IsNullOrWhiteSpace(input))
        {
            return result;
        }

        var tokens = input.Split([';', ','], StringSplitOptions.RemoveEmptyEntries);
        foreach (var rawToken in tokens)
        {
            var token = rawToken.Trim();
            if (token.Equals("Auto", StringComparison.OrdinalIgnoreCase))
            {
                result.Add(GridLength.Auto);
            }
            else if (token.EndsWith('*'))
            {
                var numberPart = token.Substring(0, token.Length - 1).Trim();
                double factor = string.IsNullOrEmpty(numberPart)
                    ? 1.0
                    : double.Parse(numberPart, CultureInfo.InvariantCulture);
                result.Add(new GridLength(factor, GridUnitType.Star));
            }
            else
            {
                double pixels = double.Parse(token, CultureInfo.InvariantCulture);
                result.Add(new GridLength(pixels, GridUnitType.Pixel));
            }
        }

        return result;
    }
}