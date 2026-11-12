// <copyright file="GridAnimation.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.Animation;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

internal class GridLayoutAnimator
{
    private readonly AnimatedGrid grid;

    public GridLayoutAnimator(AnimatedGrid grid)
    {
        this.grid = grid;
    }

    public void AnimateColumn(ColumnDefinition colDef, int colIndex, GridLength targetLength)
    {
        // Aktuelle effektive Breite holen (fängt laufende Animationen sauber ab)
        double currentActualWidth = colDef.ActualWidth;
        GridLength currentLength = colDef.Width;

        // Laufende Animation stoppen und aktuellen Pixelwert einfrieren
        colDef.BeginAnimation(ColumnDefinition.WidthProperty, null);

        if (!this.grid.IsAnimationEnabled || !this.grid.IsLoaded)
        {
            colDef.Width = targetLength;
            return;
        }

        // Fall 1: Star -> Star (Direkte Stern-Interpolation)
        if (currentLength.IsStar && targetLength.IsStar)
        {
            this.StartAnimation(colDef, ColumnDefinition.WidthProperty, currentLength, targetLength, null);
            return;
        }

        // Fall 2: Pixel-basierter Übergang (Pixel->Pixel, Auto->Pixel, Pixel->Auto, Star->Auto, Auto->Star, Pixel->Star)
        double startPixels = currentActualWidth;
        double targetPixels = targetLength.IsAuto
            ? this.MeasureAutoColumnWidth(colIndex)
            : targetLength.IsStar
                ? this.EstimateStarColumnWidth(colIndex, targetLength.Value)
                : targetLength.Value;

        // Falls keine Veränderung vorliegt, direkt Ziel setzen
        if (Math.Abs(startPixels - targetPixels) < 0.5)
        {
            colDef.Width = targetLength;
            return;
        }

        var from = new GridLength(startPixels, GridUnitType.Pixel);
        var to = new GridLength(targetPixels, GridUnitType.Pixel);

        this.StartAnimation(colDef, ColumnDefinition.WidthProperty, from, to, () =>
        {
            // Nach Abschluss der Animation die echte Ziel-Einheit (z.B. Auto oder Star) wiederherstellen
            colDef.BeginAnimation(ColumnDefinition.WidthProperty, null);
            colDef.Width = targetLength;
        });
    }

    public void AnimateRow(RowDefinition rowDef, int rowIndex, GridLength targetLength)
    {
        double currentActualHeight = rowDef.ActualHeight;
        GridLength currentLength = rowDef.Height;

        rowDef.BeginAnimation(RowDefinition.HeightProperty, null);

        if (!this.grid.IsAnimationEnabled || !this.grid.IsLoaded)
        {
            rowDef.Height = targetLength;
            return;
        }

        if (currentLength.IsStar && targetLength.IsStar)
        {
            this.StartAnimation(rowDef, RowDefinition.HeightProperty, currentLength, targetLength, null);
            return;
        }

        double startPixels = currentActualHeight;
        double targetPixels = targetLength.IsAuto
            ? this.MeasureAutoRowHeight(rowIndex)
            : targetLength.IsStar
                ? this.EstimateStarRowHeight(rowIndex, targetLength.Value)
                : targetLength.Value;

        if (Math.Abs(startPixels - targetPixels) < 0.5)
        {
            rowDef.Height = targetLength;
            return;
        }

        var from = new GridLength(startPixels, GridUnitType.Pixel);
        var to = new GridLength(targetPixels, GridUnitType.Pixel);

        this.StartAnimation(rowDef, RowDefinition.HeightProperty, from, to, () =>
        {
            rowDef.BeginAnimation(RowDefinition.HeightProperty, null);
            rowDef.Height = targetLength;
        });
    }

    private void StartAnimation(DefinitionBase target, DependencyProperty property, GridLength from, GridLength to, Action? onCompleted)
    {
        var animation = new GridLengthAnimation
        {
            From = from,
            To = to,
            Duration = this.grid.AnimationDuration,
            EasingFunction = this.grid.EasingFunction,
            FillBehavior = FillBehavior.HoldEnd,
        };

        if (onCompleted != null)
        {
            animation.Completed += (s, e) => onCompleted();
        }

        target.BeginAnimation(property, animation);
    }

    private double MeasureAutoColumnWidth(int colIndex)
    {
        double maxWidth = 0;
        foreach (UIElement child in this.grid.Children)
        {
            if (child == null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            if (Grid.GetColumn(child) == colIndex && Grid.GetColumnSpan(child) == 1)
            {
                child.Measure(new Size(double.PositiveInfinity, this.grid.ActualHeight > 0 ? this.grid.ActualHeight : double.PositiveInfinity));
                maxWidth = Math.Max(maxWidth, child.DesiredSize.Width);
            }
        }

        return maxWidth;
    }

    private double MeasureAutoRowHeight(int rowIndex)
    {
        double maxHeight = 0;
        foreach (UIElement child in this.grid.Children)
        {
            if (child == null || child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            if (Grid.GetRow(child) == rowIndex && Grid.GetRowSpan(child) == 1)
            {
                child.Measure(new Size(this.grid.ActualWidth > 0 ? this.grid.ActualWidth : double.PositiveInfinity, double.PositiveInfinity));
                maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
            }
        }

        return maxHeight;
    }

    private double EstimateStarColumnWidth(int targetColIndex, double starWeight)
    {
        if (starWeight <= 0)
        {
            return 0;
        }

        double fixedWidth = 0;
        double totalStars = 0;

        for (int i = 0; i < this.grid.ColumnDefinitions.Count; i++)
        {
            if (i == targetColIndex)
            {
                totalStars += starWeight;
            }
            else
            {
                var w = this.grid.ColumnDefinitions[i].Width;
                if (w.IsStar)
                {
                    totalStars += w.Value;
                }
                else
                {
                    fixedWidth += this.grid.ColumnDefinitions[i].ActualWidth;
                }
            }
        }

        double availableWidth = Math.Max(0, this.grid.ActualWidth - fixedWidth);
        return totalStars > 0 ? (availableWidth * (starWeight / totalStars)) : 0;
    }

    private double EstimateStarRowHeight(int targetRowIndex, double starWeight)
    {
        if (starWeight <= 0)
        {
            return 0;
        }

        double fixedHeight = 0;
        double totalStars = 0;

        for (int i = 0; i < this.grid.RowDefinitions.Count; i++)
        {
            if (i == targetRowIndex)
            {
                totalStars += starWeight;
            }
            else
            {
                var h = this.grid.RowDefinitions[i].Height;
                if (h.IsStar)
                {
                    totalStars += h.Value;
                }
                else
                {
                    fixedHeight += this.grid.RowDefinitions[i].ActualHeight;
                }
            }
        }

        double availableHeight = Math.Max(0, this.grid.ActualHeight - fixedHeight);
        return totalStars > 0 ? (availableHeight * (starWeight / totalStars)) : 0;
    }
}
