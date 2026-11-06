// <copyright file="ListBoxDragDropButton.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

internal class ListBoxDragDropButton : Button
{
    public ListBoxDragDropButton()
    {
        this.FontSize = 16;
        this.BorderThickness = default;
        this.Background = Brushes.Transparent;
        this.Foreground = Brushes.LightGray;
        this.PreviewMouseLeftButtonDown += this.StartDragDrop;
        this.Cursor = Cursors.ScrollNS;
    }

    private void StartDragDrop(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListBoxDragDropButton button || button.FindParent<ListBox>() is not ListBox target)
        {
            return;
        }

        // Das an den Button gebundene Datenobjekt (DataContext) ermitteln
        var draggedItem = button.DataContext;

        if (draggedItem != null)
        {
            bool allowDrop = target.AllowDrop;
            target.DragOver += this.Target_DragOver;
            target.AllowDrop = true;

            // Drag & Drop Operation starten (übergibt das eigentliche Datenobjekt)
            DragDrop.DoDragDrop(button, draggedItem, DragDropEffects.Move);

            target.AllowDrop = allowDrop;
            target.DragOver -= this.Target_DragOver;
        }
    }

    private void Target_DragOver(object sender, DragEventArgs e)
    {
        if (sender is not ListBox listBox || listBox.ItemsSource is not IList itemSource)
        {
            return;
        }

        // Das fallengelassene Datenobjekt abrufen
        var droppedItem = e.Data.GetData(e.Data.GetFormats()[0]);

        if (droppedItem is null)
        {
            return;
        }

        // Ziel-ListBoxItem unter der Mausposition finden
        var hitTestResult = VisualTreeHelper.HitTest(listBox, e.GetPosition(listBox));
        var listBoxItem = FindParent<ListBoxItem>(hitTestResult?.VisualHit);

        if (listBoxItem is null)
        {
            return;
        }

        // Index des Elements ermitteln, über dem das Objekt fallen gelassen wurde
        var targetItem = listBoxItem.DataContext;

        int oldIndex = itemSource.IndexOf(droppedItem);
        int newIndex = itemSource.IndexOf(targetItem);

        if (oldIndex >= 0 && newIndex >= 0 && oldIndex != newIndex)
        {
            // Die ObservableCollection aktualisiert die UI dank .Move() vollautomatisch
            itemSource.RemoveAt(oldIndex);
            itemSource.Insert(newIndex, droppedItem);
        }
    }

    private static T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        while (child != null)
        {
            if (child is T parent) return parent;
            child = VisualTreeHelper.GetParent(child);
        }

        return null;
    }
}
