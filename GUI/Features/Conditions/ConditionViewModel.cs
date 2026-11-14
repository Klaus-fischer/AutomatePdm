// <copyright file="ConditionViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using AutomatePdm;

public class ConditionViewModel : ViewModelBase, IConditionBuilder
{
    private bool negate;
    private string title;

    public bool Negate
    {
        get => this.negate;
        set => this.Set(ref this.negate, value);
    }

    public string Title
    {
        get => this.title;
        set => this.Set(ref this.title, value);
    }

    public virtual ICondition Create()
    {
        return new FalseCondition();
    }

    private class FalseCondition : ICondition
    {
        public bool Negate { get; init; }

        public bool IsValid(WorkItem item, ExecutionContext ctx)
            => false;
    }
}
