// <copyright file="ChangeStatePipelineStepPopup.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Windows.Controls;

/// <summary>
/// Interaktionslogik für TriggerPopup.xaml.
/// </summary>
public partial class ChangeStatePipelineStepPopup : UserControl, ICanTriggerExplicitUpdateSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeStatePipelineStepPopup"/> class.
    /// </summary>
    public ChangeStatePipelineStepPopup()
    {
        this.InitializeComponent();
    }

    /// <inheritdoc/>
    public void UpdateSource()
    {
        this.TitleTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        this.DescriptionTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        this.TargetStateCb.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateSource();
        this.CommentTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
    }
}
