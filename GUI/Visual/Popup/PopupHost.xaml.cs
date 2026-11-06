// <copyright file="PopupHost.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Windows.Controls;
using System.Windows.Media;

/// <summary>
/// Interaktionslogik für PopupHost.xaml.
/// </summary>
public partial class PopupHost : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PopupHost"/> class.
    /// </summary>
    public PopupHost()
    {
        this.InitializeComponent();
    }

    private MainViewModel ViewModel => (MainViewModel)this.DataContext;

    private void CancelClick(object sender, System.Windows.RoutedEventArgs e)
    {
        this.ViewModel.Popup = null;
    }

    private void AcceptClick(object sender, System.Windows.RoutedEventArgs e)
    {
        if (VisualTreeHelper.GetChild(this.ContentHost, 0) is ICanTriggerExplicitUpdateSource trigger)
        {
            trigger.UpdateSource();
        }

        this.ViewModel.Popup = null;
    }
}
