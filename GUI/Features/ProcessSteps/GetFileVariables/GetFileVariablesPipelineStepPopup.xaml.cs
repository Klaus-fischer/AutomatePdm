// <copyright file="GetFileVariablesPipelineStepPopup.xaml.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// Interaktionslogik für TriggerPopup.xaml.
/// </summary>
public partial class GetFileVariablesPipelineStepPopup : UserControl, ICanTriggerExplicitUpdateSource
{
    private int lineStartIndex;

    private int lineLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFileVariablesPipelineStepPopup"/> class.
    /// </summary>
    public GetFileVariablesPipelineStepPopup()
    {
        this.InitializeComponent();
    }

    /// <inheritdoc/>
    public void UpdateSource()
    {
        this.TitleTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        this.DescriptionTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        this.VariableNamesTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        this.ConfigurationNameTb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
    }

    private string[] VariableNames => (this.FindResource("VariableNameReader") as IVariableNameReader)?.VariableNames ?? [];

    private void VariableNames_TextChanged(object sender, TextChangedEventArgs e)
    {
        int caretIndex = this.VariableNamesTb.CaretIndex;
        int lineIndex = this.VariableNamesTb.GetLineIndexFromCharacterIndex(caretIndex);

        if (lineIndex < 0)
        {
            return;
        }

        // 1. Start- und Endposition der aktuellen Zeile bestimmen
        this.lineStartIndex = this.VariableNamesTb.GetCharacterIndexFromLineIndex(lineIndex);
        string fullLineText = this.VariableNamesTb.GetLineText(lineIndex).TrimEnd('\r', '\n');
        this.lineLength = fullLineText.Length;

        // Text von Zeilenanfang bis zum aktuellen Cursor auslesen
        int typedLength = caretIndex - this.lineStartIndex;
        if (typedLength <= 0 || typedLength > fullLineText.Length)
        {
            this.SuggestionPopup.IsOpen = false;
            return;
        }

        string currentInput = fullLineText.Substring(0, typedLength).TrimStart();

        if (string.IsNullOrEmpty(currentInput))
        {
            this.SuggestionPopup.IsOpen = false;
            return;
        }

        var existing = this.VariableNamesTb.Text.Split(['\r', '\n', ';', ',', ' '], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries)
            .Except([currentInput])
            .ToArray();

        // 2. Passende Namen filtern (auch mitten im Namen, z.B. Nachname, mit .Contains möglich)
        var matches = this.VariableNames
            .Where(n => n.StartsWith(currentInput, StringComparison.OrdinalIgnoreCase) &&
                        !n.Equals(currentInput, StringComparison.OrdinalIgnoreCase))
            .Except(existing)
            .ToList();

        if (matches.Any())
        {
            this.SuggestionList.ItemsSource = matches;
            this.SuggestionList.SelectedIndex = 0;

            // 3. Popup direkt unter dem Cursor platzieren
            Rect caretRect = this.VariableNamesTb.GetRectFromCharacterIndex(caretIndex);
            this.SuggestionPopup.HorizontalOffset = 0;
            this.SuggestionPopup.VerticalOffset = caretRect.Bottom;
            this.SuggestionPopup.IsOpen = true;
        }
        else
        {
            this.SuggestionPopup.IsOpen = false;
        }
    }

    private void VariableNames_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (!this.SuggestionPopup.IsOpen)
        {
            return;
        }

        if (e.Key == Key.Down)
        {
            if (this.SuggestionList.SelectedIndex < this.SuggestionList.Items.Count - 1)
            {
                this.SuggestionList.SelectedIndex++;
            }

            e.Handled = true;
        }
        else if (e.Key == Key.Up)
        {
            if (this.SuggestionList.SelectedIndex > 0)
            {
                this.SuggestionList.SelectedIndex--;
            }

            e.Handled = true;
        }
        else if (e.Key == Key.Enter || e.Key == Key.Tab)
        {
            this.InsertSelectedName();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            this.SuggestionPopup.IsOpen = false;
            e.Handled = true;
        }
    }

    private void SuggestionList_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.InsertSelectedName();
    }

    private void InsertSelectedName()
    {
        if (this.SuggestionList.SelectedItem is string selectedName)
        {
            // Gesamte aktuelle Zeile durch den gewählten Namen + Zeilenumbruch ersetzen
            string replacement = selectedName + Environment.NewLine;

            this.VariableNamesTb.Text = this.VariableNamesTb.Text
                .Remove(this.lineStartIndex, this.lineLength)
                .Insert(this.lineStartIndex, replacement);

            // Cursor direkt in die neue Zeile setzen
            this.VariableNamesTb.CaretIndex = this.VariableNamesTb.Text.Length - 1;
            this.SuggestionPopup.IsOpen = false;
            this.VariableNamesTb.Focus();
        }
    }
}
