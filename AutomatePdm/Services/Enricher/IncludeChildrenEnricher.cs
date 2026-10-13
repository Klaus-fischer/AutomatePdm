namespace AutomatePdm;

using AutomatePdm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class IncludeChildrenEnricher : IPipelineStep
{
    public IPipelineStep? Next { get; set; }

    public void Execute(WorkItem[] items, ExecutionContext ctx)
    {
        foreach (var item in items)
        {
        }
    }
}
