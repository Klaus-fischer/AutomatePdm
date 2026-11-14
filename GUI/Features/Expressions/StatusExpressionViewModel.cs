// <copyright file="StatusExpressionViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

public class StatusExpressionViewModel : ExpressionViewModel
{
    public override IExpression Create()
        => new StateNameExpression();
}