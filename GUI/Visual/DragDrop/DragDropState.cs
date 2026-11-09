// <copyright file="DragDropState.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.DragDrop;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

/// <summary>
/// Tracks drag state on a handle and marks its visual ancestors.
/// </summary>
public static class DragDropState
{
    public static readonly DependencyProperty IsDraggingActiveProperty =
        DependencyProperty.RegisterAttached(
            "IsDraggingActive",
            typeof(bool),
            typeof(DragDropState),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

    public static bool GetIsDraggingActive(DependencyObject obj)
        => (bool)obj.GetValue(IsDraggingActiveProperty);

    public static void SetIsDraggingActive(DependencyObject obj, bool value)
    {
        var context = obj.GetValue(FrameworkElement.DataContextProperty);

        // goes up the logical tree until it finds a parent with a different DataContext,
        // and sets the IsDraggingActive property on the root element with the same DataContext.
        // the IsDraggingActive inherits down the logical tree, so all children of the root element will have the same value.
        var parent = GetParent(obj);
        while (parent is FrameworkElement fe && fe.DataContext == context)
        {
            obj = parent;
            parent = GetParent(obj);
        }

        obj.SetValue(IsDraggingActiveProperty, value);
    }

    private sealed class StateEntry
    {
        public required DependencyObject Element { get; init; }

        public required bool PreviousValue { get; init; }
    }

    private static DependencyObject? GetParent(DependencyObject obj)
    {
        if (obj is Visual or Visual3D)
        {
            DependencyObject? visualParent = VisualTreeHelper.GetParent(obj);

            if (visualParent is not null)
            {
                return visualParent;
            }
        }

        return LogicalTreeHelper.GetParent(obj);
    }
}
