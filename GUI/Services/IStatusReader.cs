// <copyright file="IStatusReader.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Linq;
using EPDM.Interop.epdm;

public interface IStatusReader
{
    PdmStatus[] StatusNames { get; }
}

public record PdmStatus(string StatusName, string WorkflowName);

internal class StatusReader : IStatusReader
{
    private readonly Lazy<PdmStatus[]> statusNameReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusReader"/> class.
    /// </summary>
    /// <param name="workflowMgr"></param>
    public StatusReader(IEdmWorkflowMgr6 workflowMgr)
    {
        this.statusNameReader = new Lazy<PdmStatus[]>(() => workflowMgr
            .EnumerateWorkFlows()
            .SelectMany(wf => wf.EnumerateStates(), (wf, s) => new PdmStatus(s.Name, wf.Name))
            .ToArray());
    }

    /// <inheritdoc/>
    public PdmStatus[] StatusNames => this.statusNameReader.Value;
}

public class StatusReaderMock : IStatusReader
{
    public PdmStatus[] StatusNames { get; } =
    [
        new PdmStatus("In Bearbeitung", "CAD"),
        new PdmStatus("In Prüfung", "CAD"),
        new PdmStatus("Freigegeben", "CAD"),
    ];
}
