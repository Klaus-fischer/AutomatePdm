// <copyright file="MainViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class MainViewModel : ViewModelBase
{
    private TriggerViewModel? selectedTrigger = null;

    private string popupTitle = string.Empty;

    private INotifyPropertyChanged? popup = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        this.AddTriggerCommand = new AddTriggerCommand(this);
        this.EditTriggerCommand = new EditTriggerCommand(this);
    }

    public ObservableCollection<TriggerViewModel> Triggers { get; } = [];

    public TriggerViewModel? SelectedTrigger
    {
        get => this.selectedTrigger;
        set => this.Set(ref this.selectedTrigger, value);
    }

    public ICommand AddTriggerCommand { get; }

    public ICommand EditTriggerCommand { get; }

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
