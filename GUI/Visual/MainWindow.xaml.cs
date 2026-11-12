// <copyright file="MainWindow.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Windows;
using AutomatePdm.GUI.DesignTime;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Using a DependencyProperty as the backing store for IsSimulationMode.
    /// This enables animation, styling, binding, etc.
    /// </summary>
    public static readonly DependencyProperty IsSimulationModeProperty =
        DependencyProperty.RegisterAttached(
            nameof(IsSimulationMode),
            typeof(bool),
            typeof(MainWindow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
        this.DataContext = DesignTimeFactory.MainViewModel;
    }

    public static bool GetIsSimulationMode(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsSimulationModeProperty);
    }

    public static void SetIsSimulationMode(DependencyObject obj, bool value)
    {
        obj.SetValue(IsSimulationModeProperty, value);
    }

    public bool IsSimulationMode
    {
        get => GetIsSimulationMode(this);
        set => SetIsSimulationMode(this, value);
    }
}