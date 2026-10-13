// <copyright file="IPipelineStep.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public interface IPipelineStep
{
    void Execute(WorkItem[] items, ExecutionContext ctx);

    public IPipelineStep? Next { get; set; }
}