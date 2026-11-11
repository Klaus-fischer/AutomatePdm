// <copyright file="GridAnimation.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.Animation;

using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

public static class GridAnimation
{
    private class ColumnState
    {
        public GridLength OriginalWidth { get; set; }

        public AnimationClock? Clock { get; set; }

        public int Version { get; set; }
    }

    private static readonly ConditionalWeakTable<
        ColumnDefinition, ColumnState> States = new();

    public static readonly DependencyProperty HideColumnProperty =
        DependencyProperty.RegisterAttached(
            "HideColumn",
            typeof(bool),
            typeof(GridAnimation),
            new PropertyMetadata(true, OnShowColumnChanged));

    public static bool GetHideColumn(DependencyObject obj) =>
        (bool)obj.GetValue(HideColumnProperty);

    public static void SetHideColumn(
        DependencyObject obj, bool value) =>
        obj.SetValue(HideColumnProperty, value);

    public static readonly DependencyProperty AnimationDurationProperty =
        DependencyProperty.RegisterAttached(
            "AnimationDuration",
            typeof(Duration),
            typeof(GridAnimation),
            new PropertyMetadata(new Duration(
                TimeSpan.FromMilliseconds(300))));

    public static Duration GetAnimationDuration(DependencyObject obj) =>
        (Duration)obj.GetValue(AnimationDurationProperty);

    public static void SetAnimationDuration(
        DependencyObject obj, Duration value) =>
        obj.SetValue(AnimationDurationProperty, value);

    public static readonly DependencyProperty EasingFunctionProperty =
        DependencyProperty.RegisterAttached(
            "EasingFunction",
            typeof(IEasingFunction),
            typeof(GridAnimation),
            new PropertyMetadata(new CubicEase() { EasingMode = EasingMode.EaseInOut }));

    public static IEasingFunction? GetEasingFunction(
        DependencyObject obj) =>
        (IEasingFunction?)obj.GetValue(EasingFunctionProperty);

    public static void SetEasingFunction(
        DependencyObject obj, IEasingFunction? value) =>
        obj.SetValue(EasingFunctionProperty, value);

    public static readonly DependencyProperty ExpandedWidthProperty =
        DependencyProperty.RegisterAttached(
            "ExpandedWidth",
            typeof(GridLength),
            typeof(GridAnimation),
            new PropertyMetadata(new GridLength(1, GridUnitType.Star)));

    public static GridLength GetExpandedWidth(DependencyObject obj)
    {
        return (GridLength)obj.GetValue(ExpandedWidthProperty);
    }

    public static void SetExpandedWidth(DependencyObject obj, GridLength value)
    {
        obj.SetValue(ExpandedWidthProperty, value);
    }

    private static void OnShowColumnChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not ColumnDefinition column)
        {
            return;
        }

        var state = States.GetValue(
            column,
            c => new ColumnState
            {
                OriginalWidth = c.Width,
            });

        bool hide = (bool)e.NewValue;

        AnimateColumn(column, state, hide);
    }

    private static void AnimateColumn(
        ColumnDefinition column,
        ColumnState state,
        bool hide)
    {
        // Aktuelle Breite lesen, solange die alte Animation
        // noch aktiv ist. So funktioniert auch das Umkehren.
        GridLength currentWidth = column.Width;

        // Alte Animation ablösen.
        state.Version++;
        int version = state.Version;
        var expanded = GetExpandedWidth(column);

        GridLength targetWidth = hide
            ? new GridLength(0, expanded.GridUnitType)
            : expanded;

        var duration = GetAnimationDuration(column);

        if (duration.HasTimeSpan &&
            duration.TimeSpan <= TimeSpan.Zero)
        {
            column.ApplyAnimationClock(
                ColumnDefinition.WidthProperty, null);

            column.Width = targetWidth;
            return;
        }

        var animation = new GridLengthAnimation
        {
            From = currentWidth,
            To = targetWidth,
            Duration = duration,
            EasingFunction = GetEasingFunction(column),
        };

        var clock = animation.CreateClock();

        state.Clock = clock;

        clock.Completed += (_, _) =>
        {
            // Nur die aktuellste Animation darf abschließen.
            if (state.Version != version)
            {
                return;
            }

            column.Width = targetWidth;

            column.ApplyAnimationClock(
                ColumnDefinition.WidthProperty, null);

            state.Clock = null;
        };

        column.ApplyAnimationClock(
            ColumnDefinition.WidthProperty,
            clock,
            HandoffBehavior.SnapshotAndReplace);
    }
}
