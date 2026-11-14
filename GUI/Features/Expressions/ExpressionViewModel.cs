// <copyright file="ExpressionViewModel.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using AutomatePdm;

public class ExpressionViewModel : ViewModelBase, IExpressionBuilder
{
    /// <inheritdoc/>
    public virtual IExpression Create()
        => new EmptyExpression();

    private class EmptyExpression : IExpression
    {
        public string Resolve(WorkItem workItem, ExecutionContext ctx)
            => string.Empty;
    }
}