// <copyright file="DropTarget.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.DragDrop;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// Attached configuration for supported drop target controls.
/// </summary>
public static class DropTarget
{
    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.RegisterAttached(
            "Key",
            typeof(string),
            typeof(DropTarget),
            new PropertyMetadata(null, OnTargetPropertyChanged));

    public static string? GetKey(DependencyObject obj)
        => (string?)obj.GetValue(KeyProperty);

    public static void SetKey(DependencyObject obj, string? value)
        => obj.SetValue(KeyProperty, value);

    public static readonly DependencyProperty AcceptTypeProperty =
        DependencyProperty.RegisterAttached(
            "AcceptType",
            typeof(Type),
            typeof(DropTarget),
            new PropertyMetadata(null, OnTargetPropertyChanged));

    public static Type? GetAcceptType(DependencyObject obj)
        => (Type?)obj.GetValue(AcceptTypeProperty);

    public static void SetAcceptType(DependencyObject obj, Type? value)
        => obj.SetValue(AcceptTypeProperty, value);

    public static readonly DependencyProperty DropCommandProperty =
        DependencyProperty.RegisterAttached(
            "DropCommand",
            typeof(ICommand),
            typeof(DropTarget),
            new PropertyMetadata(null, OnTargetPropertyChanged));

    public static ICommand? GetDropCommand(DependencyObject obj)
        => (ICommand?)obj.GetValue(DropCommandProperty);

    public static void SetDropCommand(DependencyObject obj, ICommand? value)
        => obj.SetValue(DropCommandProperty, value);

    private static readonly DependencyProperty IsRegisteredProperty =
        DependencyProperty.RegisterAttached(
            "IsRegistered",
            typeof(bool),
            typeof(DropTarget),
            new PropertyMetadata(false));

    internal static bool GetIsRegistered(DependencyObject obj)
        => (bool)obj.GetValue(IsRegisteredProperty);

    internal static void SetIsRegistered(DependencyObject obj, bool value)
        => obj.SetValue(IsRegisteredProperty, value);

    private static void OnTargetPropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not ItemsControl control)
        {
            return;
        }

        if (control.IsLoaded)
        {
            DragDropManager.RefreshDropTarget(control);
        }
        else
        {
            control.Loaded += OnTargetLoaded;
            control.Unloaded += OnTargetUnloaded;
        }
    }

    private static void OnTargetLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is ItemsControl control)
        {
            control.Loaded -= OnTargetLoaded;
            DragDropManager.RefreshDropTarget(control);
        }
    }

    private static void OnTargetUnloaded(object sender, RoutedEventArgs e)
    {
        if (sender is ItemsControl control)
        {
            DragDropManager.UnregisterDropTarget(control);
        }
    }
}
