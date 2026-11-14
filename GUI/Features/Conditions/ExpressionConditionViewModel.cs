// <copyright file="ExpressionConditionViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

public class ExpressionConditionViewModel : ConditionViewModel, IConditionBuilder
{
    private ExpressionViewModel right = new TextExpressionViewModel();
    private ExpressionOperator @operator = ExpressionOperator.Equal;
    private ExpressionViewModel left = Empty;
    private bool caseSensivity;

    public static ExpressionViewModel Empty { get; } = new ExpressionViewModel();

    public ExpressionViewModel Left
    {
        get => this.left;
        set => this.Set(ref this.left, value);
    }

    public ExpressionOperator Operator
    {
        get => this.@operator;
        set => this.Set(ref this.@operator, value);
    }

    public ExpressionViewModel Right
    {
        get => this.right;
        set => this.Set(ref this.right, value);
    }

    public bool CaseSensivity
    {
        get { return this.caseSensivity; }
        set { this.Set(ref this.caseSensivity, value); }
    }

    public override ICondition Create()
        => new ExpressionCondition()
        {
            First = this.Left.Create(),
            Operator = this.Operator,
            Second = this.Right.Create(),
            Negate = this.Negate,
        };
}
