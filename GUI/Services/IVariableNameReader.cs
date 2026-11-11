// <copyright file="IVariableNameReader.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

using System;
using System.Linq;
using EPDM.Interop.epdm;

public interface IVariableNameReader
{
    /// <summary>
    /// Gets the collection of names of variables.
    /// </summary>
    string[] VariableNames { get; }
}

public class VariableNameReader : IVariableNameReader
{
    private readonly Lazy<string[]> variableNamesReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableNameReader"/> class.
    /// </summary>
    /// <param name="variableMgr7">The Variable Mgr.</param>
    public VariableNameReader(IEdmVariableMgr7 variableMgr7)
    {
        this.variableNamesReader = new(() => variableMgr7.GetVariables().Select(o => o.Name).ToArray());
    }

    /// <inheritdoc/>
    public string[] VariableNames => this.variableNamesReader.Value;
}

public class VariableNameReaderMock : IVariableNameReader
{
    /// <inheritdoc/>
    public string[] VariableNames { get; } =
    [
        "Artikelnummer",
        "Zeichnungsnummer",
        "Bezeichnung",
        "Werkstoff",
        "Revision",
    ];
}