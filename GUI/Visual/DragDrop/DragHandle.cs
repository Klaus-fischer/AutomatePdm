// <copyright file="DragHandle.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.DragDrop;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// A control that starts a drag operation when its content is dragged.
/// </summary>
public class DragHandle : ContentControl
{
    public static readonly DependencyProperty DragDataProperty =
        DependencyProperty.Register(
            nameof(DragData),
            typeof(object),
            typeof(DragHandle),
            new PropertyMetadata(null));

    public static readonly DependencyProperty AllowedDropTargetsProperty =
        DependencyProperty.Register(
            nameof(AllowedDropTargets),
            typeof(string),
            typeof(DragHandle),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty AllowMoveProperty =
        DependencyProperty.Register(
            nameof(AllowMove),
            typeof(bool),
            typeof(DragHandle),
            new PropertyMetadata(true));

    public static readonly DependencyProperty LiveReorderProperty =
        DependencyProperty.Register(
            nameof(LiveReorder),
            typeof(bool),
            typeof(DragHandle),
            new PropertyMetadata(false));

    public static readonly DependencyProperty KeepPlaceholderProperty =
        DependencyProperty.Register(
            nameof(KeepPlaceholder),
            typeof(bool),
            typeof(DragHandle),
            new PropertyMetadata(true));

    public static readonly DependencyProperty PreviewElementProperty =
        DependencyProperty.Register(
            nameof(PreviewElement),
            typeof(FrameworkElement),
            typeof(DragHandle),
            new PropertyMetadata(null));

    private Point dragStartPoint;
    private bool isMouseDown;

    static DragHandle()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(DragHandle),
            new FrameworkPropertyMetadata(typeof(DragHandle)));
    }

    public DragHandle()
    {
        this.PreviewMouseLeftButtonDown += this.OnPreviewMouseLeftButtonDown;
        this.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
        this.PreviewMouseMove += this.OnPreviewMouseMove;
    }

    public object? DragData
    {
        get => this.GetValue(DragDataProperty);
        set => this.SetValue(DragDataProperty, value);
    }


    public string AllowedDropTargets
    {
        get => (string)this.GetValue(AllowedDropTargetsProperty);
        set => this.SetValue(AllowedDropTargetsProperty, value);
    }

    public bool AllowMove
    {
        get => (bool)this.GetValue(AllowMoveProperty);
        set => this.SetValue(AllowMoveProperty, value);
    }

    public static readonly DependencyProperty AllowCopyProperty =
        DependencyProperty.Register(
            nameof(AllowCopy),
            typeof(bool),
            typeof(DragHandle),
            new PropertyMetadata(true));

    public bool AllowCopy
    {
        get => (bool)this.GetValue(AllowCopyProperty);
        set => this.SetValue(AllowCopyProperty, value);
    }

    public bool LiveReorder
    {
        get => (bool)this.GetValue(LiveReorderProperty);
        set => this.SetValue(LiveReorderProperty, value);
    }

    public bool KeepPlaceholder
    {
        get => (bool)this.GetValue(KeepPlaceholderProperty);
        set => this.SetValue(KeepPlaceholderProperty, value);
    }

    public FrameworkElement? PreviewElement
    {
        get => (FrameworkElement?)this.GetValue(PreviewElementProperty);
        set => this.SetValue(PreviewElementProperty, value);
    }

    private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.dragStartPoint = e.GetPosition(null);
        this.isMouseDown = true;
    }

    private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.isMouseDown = false;
    }

    private void OnPreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!this.isMouseDown || e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        Point currentPosition = e.GetPosition(null);

        if (Math.Abs(currentPosition.X - this.dragStartPoint.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(currentPosition.Y - this.dragStartPoint.Y) < SystemParameters.MinimumVerticalDragDistance)
        {
            return;
        }

        this.isMouseDown = false;
        DragDropManager.StartDrag(this);
    }
}
