// <copyright file="DragDropManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI.DragDrop;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    private static readonly List<FrameworkElement> RegisteredTargets = new();

    private static DragHandle? activeHandle;
    private static object? activeData;
    private static ItemsControl? sourceControl;
    private static IList? sourceList;
    private static DragPreviewAdorner? previewAdorner;
    private static AdornerLayer? adornerLayer;
    private static readonly Dictionary<FrameworkElement, bool> PreviousAllowDrop = new();
    private static List<string> activeTargetKeys = new();
    private static DropIndicatorAdorner? dropIndicatorAdorner;
    private static AdornerLayer? dropIndicatorLayer;
    private static ItemsControl? currentDropTarget;
    private static int currentInsertIndex;

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

    internal static void RefreshDropTarget(FrameworkElement control)
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

    internal static void UnregisterDropTarget(FrameworkElement control)
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

    private static void ShowDropIndicator(ItemsControl target, int insertIndex)
    {
        if (currentDropTarget != target)
        {
            HideDropIndicator();

            dropIndicatorLayer =
                AdornerLayer.GetAdornerLayer(target);

            if (dropIndicatorLayer == null)
            {
                return;
            }

            dropIndicatorAdorner =
                new DropIndicatorAdorner(target);

            dropIndicatorLayer.Add(dropIndicatorAdorner);
            currentDropTarget = target;
        }

        currentInsertIndex = insertIndex;

        double y = GetInsertionY(target, insertIndex);

        dropIndicatorAdorner?.UpdatePosition(y);
    }

    private static void HideDropIndicator()
    {
        if (dropIndicatorAdorner != null && dropIndicatorLayer != null)
        {
            dropIndicatorLayer.Remove(dropIndicatorAdorner);
        }

        dropIndicatorAdorner = null;
        dropIndicatorLayer = null;
        currentDropTarget = null;
    }

    internal static int GetInsertionIndex(ItemsControl itemsControl, Point position)
    {
        for (int i = 0; i < itemsControl.Items.Count; i++)
        {
            DependencyObject? container =
                itemsControl.ItemContainerGenerator.ContainerFromIndex(i);

            if (container is not FrameworkElement element)
            {
                continue;
            }

            Point itemPosition = element.TranslatePoint(
                new Point(0, 0),
                itemsControl);

            double midpoint =
                itemPosition.Y + (element.ActualHeight / 2);

            if (position.Y < midpoint)
            {
                return i;
            }
        }

        return itemsControl.Items.Count;
    }

    private static double GetInsertionY(ItemsControl target, int index)
    {
        if (target.Items.Count == 0)
        {
            return 2;
        }

        if (index >= target.Items.Count)
        {
            DependencyObject? lastContainer =
                target.ItemContainerGenerator.ContainerFromIndex(
                    target.Items.Count - 1);

            if (lastContainer is FrameworkElement lastItem)
            {
                Point position = lastItem.TranslatePoint(
                    new Point(0, lastItem.ActualHeight),
                    target);

                return position.Y;
            }

            return target.ActualHeight;
        }

        DependencyObject? container =
            target.ItemContainerGenerator.ContainerFromIndex(index);

        if (container is FrameworkElement item)
        {
            return item.TranslatePoint(
                new Point(0, 0),
                target).Y;
        }

        return target.ActualHeight;
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
        foreach (FrameworkElement target in RegisteredTargets.ToArray())
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
        if (e.Handled == true ||
            sender is not FrameworkElement target ||
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
        if (sender is not FrameworkElement target ||
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
        DragAction action = effect switch
        {
            DragDropEffects.Copy | DragDropEffects.Move => e.KeyStates.HasFlag(DragDropKeyStates.ControlKey) ? DragAction.Copy : DragAction.Move,
            DragDropEffects.Copy => DragAction.Copy,
            DragDropEffects.Move => DragAction.Move,
            _ => DragAction.Move,
        };

        var context = new DropContext
        {
            Data = activeData,
            Action = action,
            SourceList = sourceList,
            SourceControl = sourceControl,
            InsertIndex = -1,
            InsertBefore = null,
        };

        if (target is ItemsControl itemsControl)
        {
            object? insertBefore = GetInsertBefore(itemsControl, e);
            IList? targetList = itemsControl.ItemsSource as IList;
            int insertIndex = currentInsertIndex;

            context = new DropContext
            {
                Data = activeData,
                Action = action,
                SourceList = sourceList,
                SourceControl = sourceControl,
                InsertBefore = insertBefore,
                InsertIndex = insertIndex,
                TargetControl = itemsControl,
                TargetList = targetList,
            };
        }

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

    private static bool IsTargetAllowed(FrameworkElement target, object data)
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

    private static void UpdatePreviewPosition(FrameworkElement target, DragEventArgs e)
    {
        if (previewAdorner is null || adornerLayer is null)
        {
            return;
        }

        Point point = e.GetPosition(previewAdorner.AdornedElement);
        previewAdorner.UpdatePosition(point);

        if (target is ItemsControl itemsControl)
        {
            int insertIndex = GetInsertionIndex(itemsControl, e.GetPosition(itemsControl));
            ShowDropIndicator(itemsControl, insertIndex);
        }
    }

    private static void Cleanup()
    {
        if (previewAdorner is not null && adornerLayer is not null)
        {
            adornerLayer.Remove(previewAdorner);
        }

        previewAdorner = null;
        adornerLayer = null;
        HideDropIndicator();

        foreach (var entry in PreviousAllowDrop)
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
