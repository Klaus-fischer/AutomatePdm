// <copyright file="IExpressionBuilder.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

public class TextExpressionViewModel : ExpressionViewModel
{
    private string text;

    public string Text
    {
        get { return text; }
        set { this.Set(ref text, value); }
    }

    /// <inheritdoc/>
    public override IExpression Create()
        => new TextExpression()
        {
            Text = this.Text,
        };
}