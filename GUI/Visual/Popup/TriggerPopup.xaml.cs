// <copyright file="TriggerPopup.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Windows.Controls;

/// <summary>
/// Interaktionslogik für TriggerPopup.xaml.
/// </summary>
public partial class TriggerPopup : UserControl, ICanTriggerExplicitUpdateSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerPopup"/> class.
    /// </summary>
    public TriggerPopup()
    {
        this.InitializeComponent();
    }

    /// <inheritdoc/>
    public void UpdateSource()
    {
        this.TitleTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        this.DescriptionTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        this.TriggerNameCb.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource();
    }
}
