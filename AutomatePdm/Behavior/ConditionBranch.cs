// <copyright file="ConditionBranch.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public class ConditionBranch(ICondition condition) : IPipelineStep
{
    public ICondition Condition { get; } = condition;

    public IPipelineStep? Branch { get; set; }

    public IPipelineStep? ElseBranch { get; set; }

    /// <inheritdoc/>
    public IPipelineStep? Next { get; set; }

    /// <inheritdoc/>
    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        var branchItems = items.Where(i => this.Condition.IsValid(i, ctx)).ToArray();
        var elseBranchItems = items.Except(branchItems).ToArray();

        this.Branch?.Execute(branchItems, ctx);
        this.ElseBranch?.Execute(elseBranchItems, ctx);

        this.Next?.Execute(items, ctx);
    }
}
