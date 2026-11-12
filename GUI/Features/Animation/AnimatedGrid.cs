// <copyright file="AnimatedGrid.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.Animation;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

public class AnimatedGrid : Grid
{
    private readonly GridLayoutAnimator animator;

    public AnimatedGrid()
    {
        this.animator = new GridLayoutAnimator(this);
    }

    public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register(
            nameof(Columns),
            typeof(string),
            typeof(AnimatedGrid),
            new PropertyMetadata(string.Empty, OnColumnsChanged));

    public static readonly DependencyProperty RowsProperty =
        DependencyProperty.Register(
            nameof(Rows),
            typeof(string),
            typeof(AnimatedGrid),
            new PropertyMetadata(string.Empty, OnRowsChanged));

    public static readonly DependencyProperty AnimationDurationProperty =
        DependencyProperty.Register(
            nameof(AnimationDuration),
            typeof(Duration),
            typeof(AnimatedGrid),
            new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(300))));

    public static readonly DependencyProperty EasingFunctionProperty =
        DependencyProperty.Register(
            nameof(EasingFunction),
            typeof(IEasingFunction),
            typeof(AnimatedGrid),
            new PropertyMetadata(new CubicEase { EasingMode = EasingMode.EaseInOut }));

    public static readonly DependencyProperty IsAnimationEnabledProperty =
        DependencyProperty.Register(
            nameof(IsAnimationEnabled),
            typeof(bool),
            typeof(AnimatedGrid),
            new PropertyMetadata(true));

    public string Columns
    {
        get => (string)this.GetValue(ColumnsProperty);
        set => this.SetValue(ColumnsProperty, value);
    }

    public string Rows
    {
        get => (string)this.GetValue(RowsProperty);
        set => this.SetValue(RowsProperty, value);
    }

    public Duration AnimationDuration
    {
        get => (Duration)this.GetValue(AnimationDurationProperty);
        set => this.SetValue(AnimationDurationProperty, value);
    }

    public IEasingFunction? EasingFunction
    {
        get => (IEasingFunction?)this.GetValue(EasingFunctionProperty);
        set => this.SetValue(EasingFunctionProperty, value);
    }

    public bool IsAnimationEnabled
    {
        get => (bool)this.GetValue(IsAnimationEnabledProperty);
        set => this.SetValue(IsAnimationEnabledProperty, value);
    }

    private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AnimatedGrid grid)
        {
            grid.ApplyColumns((string)e.NewValue);
        }
    }

    private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AnimatedGrid grid)
        {
            grid.ApplyRows((string)e.NewValue);
        }
    }

    private void ApplyColumns(string definitionString)
    {
        List<GridLength> parsed = DefinitionParser.Parse(definitionString);

        // Überzählige Definitionen entfernen
        while (this.ColumnDefinitions.Count > parsed.Count)
        {
            this.ColumnDefinitions.RemoveAt(this.ColumnDefinitions.Count - 1);
        }

        for (int i = 0; i < parsed.Count; i++)
        {
            GridLength target = parsed[i];

            if (i >= this.ColumnDefinitions.Count)
            {
                // Neue Spalte hinzufügen (bei laufendem UI optional von 0 animieren)
                var newCol = new ColumnDefinition
                {
                    Width = (this.IsLoaded && this.IsAnimationEnabled) ? new GridLength(0, GridUnitType.Pixel) : target,
                };
                this.ColumnDefinitions.Add(newCol);

                if (this.IsLoaded && this.IsAnimationEnabled)
                {
                    this.animator.AnimateColumn(newCol, i, target);
                }
            }
            else
            {
                // Bestehende ColumnDefinition wiederverwenden und animieren
                this.animator.AnimateColumn(this.ColumnDefinitions[i], i, target);
            }
        }
    }

    private void ApplyRows(string definitionString)
    {
        List<GridLength> parsed = DefinitionParser.Parse(definitionString);

        while (this.RowDefinitions.Count > parsed.Count)
        {
            this.RowDefinitions.RemoveAt(this.RowDefinitions.Count - 1);
        }

        for (int i = 0; i < parsed.Count; i++)
        {
            GridLength target = parsed[i];

            if (i >= this.RowDefinitions.Count)
            {
                var newRow = new RowDefinition
                {
                    Height = (this.IsLoaded && this.IsAnimationEnabled) ? new GridLength(0, GridUnitType.Pixel) : target,
                };
                this.RowDefinitions.Add(newRow);

                if (this.IsLoaded && this.IsAnimationEnabled)
                {
                    this.animator.AnimateRow(newRow, i, target);
                }
            }
            else
            {
                this.animator.AnimateRow(this.RowDefinitions[i], i, target);
            }
        }
    }
}
