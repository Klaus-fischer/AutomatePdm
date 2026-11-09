// <copyright file="IPipelineStepBuilder.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

internal interface IPipelineStepBuilder
{
}

internal interface IPipelineStepBuilder<T> : IPipelineStepBuilder
    where T : IPipelineStep
{
    T Build();
}
