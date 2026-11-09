// <copyright file="DragDropManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.DragDrop;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Central coordinator for drag sessions and drop-target registration.
/// </summary>
public static class DragDropManager
{
    private static readonly List<ItemsControl> RegisteredTargets = new();

    private static DragHandle? activeHandle;
    private static object? activeData;
    private static ItemsControl? sourceControl;
    private static IList? sourceList;
    private static int sourceIndex = -1;
    private static DragPreviewAdorner? previewAdorner;
    private static AdornerLayer? adornerLayer;
    private static readonly Dictionary<ItemsControl, bool> PreviousAllowDrop = new();
    private static List<string> activeTargetKeys = new();

    public static readonly DependencyProperty IsRootProperty =
        DependencyProperty.RegisterAttached(
            "IsRoot",
            typeof(bool),
            typeof(DragDropManager),
            new PropertyMetadata(false));

    public static bool GetIsRoot(DependencyObject obj)
        => (bool)obj.GetValue(IsRootProperty);

    public static void SetIsRoot(DependencyObject obj, bool value)
        => obj.SetValue(IsRootProperty, value);

    internal static void RefreshDropTarget(ItemsControl control)
    {
        if (string.IsNullOrWhiteSpace(DropTarget.GetKey(control)))
        {
            UnregisterDropTarget(control);
            return;
        }

        if (!RegisteredTargets.Contains(control))
        {
            RegisteredTargets.Add(control);
            control.AddHandler(DragDrop.DragOverEvent, new DragEventHandler(OnDragOver), true);
            control.AddHandler(DragDrop.DropEvent, new DragEventHandler(OnDrop), true);
        }

        DropTarget.SetIsRegistered(control, true);
    }

    internal static void UnregisterDropTarget(ItemsControl control)
    {
        if (!RegisteredTargets.Remove(control))
        {
            return;
        }

        control.RemoveHandler(DragDrop.DragOverEvent, new DragEventHandler(OnDragOver));
        control.RemoveHandler(DragDrop.DropEvent, new DragEventHandler(OnDrop));
        DropTarget.SetIsRegistered(control, false);
    }

    internal static void StartDrag(DragHandle handle)
    {
        if (activeHandle is not null)
        {
            return;
        }

        object? data = handle.DragData ?? handle.DataContext;

        if (data is null)
        {
            return;
        }

        activeHandle = handle;
        activeData = data;
        sourceControl = FindAncestorItemsControl(handle);
        sourceList = sourceControl?.ItemsSource as IList;
        sourceIndex = sourceList?.IndexOf(data) ?? -1;

        activeTargetKeys = handle.AllowedDropTargets
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        DragDropState.SetIsDraggingActive(handle, true);

        try
        {
            ActivateTargets(handle);

            CreatePreview(handle);

            DragDropEffects allowedEffects = GetAllowedEffects(handle);

            if (allowedEffects == DragDropEffects.None)
            {
                return;
            }

            DragDrop.DoDragDrop(handle, data, allowedEffects);
        }
        finally
        {
            Cleanup();
        }
    }

    private static DragDropEffects GetAllowedEffects(DragHandle handle)
    {
        DragDropEffects effects = DragDropEffects.None;

        if (handle.AllowMove)
        {
            effects |= DragDropEffects.Move;
        }

        if (handle.AllowCopy)
        {
            effects |= DragDropEffects.Copy;
        }

        return effects;
    }

    private static void ActivateTargets(DragHandle handle)
    {
        foreach (ItemsControl target in RegisteredTargets.ToArray())
        {
            string? key = DropTarget.GetKey(target);
            Type? acceptType = DropTarget.GetAcceptType(target);

            bool keyAllowed = key is not null && activeTargetKeys.Contains(key);
            bool typeAllowed = acceptType is null ||
                               (activeData is not null && acceptType.IsInstanceOfType(activeData));

            if (!keyAllowed || !typeAllowed)
            {
                continue;
            }

            if (!PreviousAllowDrop.ContainsKey(target))
            {
                PreviousAllowDrop[target] = target.AllowDrop;
            }

            target.AllowDrop = true;
        }
    }

    private static void CreatePreview(DragHandle handle)
    {
        FrameworkElement? previewElement = handle.PreviewElement;

        if (previewElement is null)
        {
            return;
        }

        DependencyObject? root = FindRoot(handle);

        if (root is not UIElement rootElement)
        {
            return;
        }

        adornerLayer = AdornerLayer.GetAdornerLayer(rootElement);

        if (adornerLayer is null)
        {
            return;
        }

        previewAdorner = new DragPreviewAdorner(rootElement, previewElement);
        adornerLayer.Add(previewAdorner);
    }

    private static void OnDragOver(object sender, DragEventArgs e)
    {
        if (sender is not ItemsControl target ||
            activeHandle is null ||
            activeData is null)
        {
            return;
        }

        if (!IsTargetAllowed(target, activeData))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        DragDropEffects available = GetAllowedEffects(activeHandle);
        DragDropEffects requested = (Keyboard.Modifiers & ModifierKeys.Control) != 0
            ? DragDropEffects.Copy
            : DragDropEffects.Move;

        if ((available & requested) == 0)
        {
            requested = (available & DragDropEffects.Move) != 0
                ? DragDropEffects.Move
                : (available & DragDropEffects.Copy) != 0
                    ? DragDropEffects.Copy
                    : DragDropEffects.None;
        }

        e.Effects = requested;
        e.Handled = true;

        UpdatePreviewPosition(target, e);
    }

    private static void OnDrop(object sender, DragEventArgs e)
    {
        if (sender is not ItemsControl target ||
            activeHandle is null ||
            activeData is null)
        {
            return;
        }

        if (!IsTargetAllowed(target, activeData))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        ICommand? command = DropTarget.GetDropCommand(target);

        if (command is null)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        DragDropEffects effect = e.Effects;
        DragAction action = effect == DragDropEffects.Copy
            ? DragAction.Copy
            : DragAction.Move;

        object? insertBefore = GetInsertBefore(target, e);
        IList? targetList = target.ItemsSource as IList;
        int insertIndex = insertBefore is null
            ? target.Items.Count
            : target.Items.IndexOf(insertBefore);

        var context = new DropContext
        {
            Data = activeData,
            Action = action,
            TargetControl = target,
            TargetList = targetList,
            SourceList = sourceList,
            SourceControl = sourceControl,
            InsertBefore = insertBefore,
            InsertIndex = insertIndex,
        };

        if (command.CanExecute(context))
        {
            command.Execute(context);
            e.Effects = effect;
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }

        e.Handled = true;
    }

    private static bool IsTargetAllowed(ItemsControl target, object data)
    {
        string? key = DropTarget.GetKey(target);
        Type? acceptType = DropTarget.GetAcceptType(target);

        return key is not null &&
               activeTargetKeys.Contains(key) &&
               (acceptType is null || acceptType.IsInstanceOfType(data));
    }

    private static object? GetInsertBefore(ItemsControl target, DragEventArgs e)
    {
        DependencyObject? hit = e.OriginalSource as DependencyObject;

        while (hit is not null && hit != target)
        {
            if (hit is FrameworkElement element &&
                element.DataContext is object item &&
                target.Items.Contains(item))
            {
                return item;
            }

            hit = GetParent(hit);
        }

        return null;
    }

    private static void UpdatePreviewPosition(ItemsControl target, DragEventArgs e)
    {
        if (previewAdorner is null || adornerLayer is null)
        {
            return;
        }

        Point point = e.GetPosition(previewAdorner.AdornedElement);
        previewAdorner.UpdatePosition(point);
    }

    private static void Cleanup()
    {
        if (previewAdorner is not null && adornerLayer is not null)
        {
            adornerLayer.Remove(previewAdorner);
        }

        previewAdorner = null;
        adornerLayer = null;

        foreach (KeyValuePair<ItemsControl, bool> entry in PreviousAllowDrop)
        {
            entry.Key.AllowDrop = entry.Value;
        }

        PreviousAllowDrop.Clear();

        if (activeHandle is not null)
        {
            DragDropState.SetIsDraggingActive(activeHandle, false);
        }

        activeHandle = null;
        activeData = null;
        sourceControl = null;
        sourceList = null;
        sourceIndex = -1;
        activeTargetKeys.Clear();
    }

    private static ItemsControl? FindAncestorItemsControl(DependencyObject child)
    {
        DependencyObject? current = child;

        while (current is not null)
        {
            if (current is ItemsControl itemsControl)
            {
                return itemsControl;
            }

            current = GetParent(current);
        }

        return null;
    }

    private static DependencyObject? FindRoot(DependencyObject child)
    {
        DependencyObject? current = child;
        DependencyObject? last = child;

        while (current is not null)
        {
            if (GetIsRoot(current))
            {
                return current;
            }

            last = current;
            current = GetParent(current);
        }

        return last;
    }

    private static DependencyObject? GetParent(DependencyObject obj)
    {
        if (obj is Visual or System.Windows.Media.Media3D.Visual3D)
        {
            DependencyObject? parent = VisualTreeHelper.GetParent(obj);

            if (parent is not null)
            {
                return parent;
            }
        }

        return LogicalTreeHelper.GetParent(obj);
    }
}
