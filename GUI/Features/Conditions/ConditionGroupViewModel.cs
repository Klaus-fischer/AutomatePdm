// <copyright file="ConditionGroupViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System.Collections.ObjectModel;
using System.Linq;
using AutomatePdm;

public class GroupConditionViewModel : ConditionViewModel, IConditionBuilder
{
    private GroupConditionType conditionType;

    public ObservableCollection<ConditionViewModel> Conditions { get; } = new();

    public GroupConditionType ConditionType
    {
        get { return this.conditionType; }
        set { this.Set(ref this.conditionType, value); }
    }

    /// <inheritdoc/>
    public override ICondition Create()
    {
        return this.ConditionType switch
        {
            GroupConditionType.AND => new AndCondition()
            {
                Conditions = [.. this.Conditions.Select(o => o.Create())],
            },
            GroupConditionType.OR => new OrCondition()
            {
                Conditions = [.. this.Conditions.Select(o => o.Create())],
            },
            GroupConditionType.ExactOne => new OneOfCondition()
            {
                Conditions = [.. this.Conditions.Select(o => o.Create())],
            },
            _ => base.Create(),
        };
    }
}

public enum GroupConditionType
{
    AND,
    OR,
    ExactOne,
}