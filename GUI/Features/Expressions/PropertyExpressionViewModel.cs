// <copyright file="PropertyExpressionViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

public class PropertyExpressionViewModel : ExpressionViewModel
{
    private string propertyName;

    public string PropertyName
    {
        get => this.propertyName;
        set => this.Set(ref this.propertyName, value);
    }

    public override IExpression Create()
        => new PropertyExpression() { PropertyName = this.PropertyName, };
}
