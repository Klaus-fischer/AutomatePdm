// <copyright file="MainViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutomatePdm.GUI.DragDrop;

public class MainViewModel : ViewModelBase
{
    private string popupTitle = string.Empty;

    private INotifyPropertyChanged? popup = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        this.AddTriggerCommand = new AddTriggerCommand(this);
        this.EditTriggerCommand = new EditTriggerCommand(this);
        this.EditScriptCommand = new EditScriptCommand(this);
    }

    public DropAbleCollection<TriggerViewModel> Triggers { get; } = [];

    public ICommand AddTriggerCommand { get; }

    public ICommand EditTriggerCommand { get; }

    public ICommand EditScriptCommand { get; }

    /// <summary>
    /// Gets or sets the ...
    /// </summary>
    public string PopupTitle
    {
        get => this.popupTitle;
        set => this.Set(ref this.popupTitle, value);
    }

    public INotifyPropertyChanged? Popup
    {
        get => this.popup;
        set => this.Set(ref this.popup, value);
    }
}