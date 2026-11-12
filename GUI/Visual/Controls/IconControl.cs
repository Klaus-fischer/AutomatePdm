// <copyright file="IconControl.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using AutomatePdm.GUI.DragDrop;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

public class IconControl : TextBlock
{
    /// <summary>
    /// Using a DependencyProperty as the backing store for Icon.
    /// This enables animation, styling, binding, etc...
    /// </summary>
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(
            nameof(Icon),
            typeof(Icon),
            typeof(IconControl),
            new PropertyMetadata(Icon.Stop, OnIconChanged));

    private static IconConverter converter = new();

    static IconControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(IconControl),
            new FrameworkPropertyMetadata(typeof(IconControl)));
        FontFamilyProperty.OverrideMetadata(
            typeof(IconControl),
            new FrameworkPropertyMetadata(new FontFamily("Segoe MDL2 Assets")));
        VerticalAlignmentProperty.OverrideMetadata(
            typeof(IconControl),
            new FrameworkPropertyMetadata(VerticalAlignment.Center));
        HorizontalAlignmentProperty.OverrideMetadata(
            typeof(IconControl),
            new FrameworkPropertyMetadata(HorizontalAlignment.Center));
        TextAlignmentProperty.OverrideMetadata(
              typeof(IconControl),
              new FrameworkPropertyMetadata(TextAlignment.Center));
        FontSizeProperty.OverrideMetadata(
            typeof(IconControl),
            new FrameworkPropertyMetadata(16.0));

        RegisterDefaultDataTemplate();
    }

    private static void RegisterDefaultDataTemplate()
    {
        if (Application.Current == null)
        {
            return;
        }

        var key = new DataTemplateKey(typeof(Icon));
        if (!Application.Current.Resources.Contains(key))
        {
            var factory = new FrameworkElementFactory(typeof(IconControl));
            factory.SetBinding(IconProperty, new Binding()); 

            var template = new DataTemplate(typeof(Icon))
            {
                VisualTree = factory,
            };
            template.Seal();

            Application.Current.Resources.Add(key, template);
        }
    }

    /// <summary>
    /// Gets or sets the icon of the symbol.
    /// </summary>
    public Icon Icon
    {
        get { return (Icon)this.GetValue(IconProperty); }
        set { this.SetValue(IconProperty, value); }
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is IconControl icon)
        {
            icon.Text = converter.Convert(icon.Icon);
        }
    }
}
