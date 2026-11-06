// <copyright file="StringBuilderLogger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Text;

internal class StringBuilderLogger : ILogger
{
    public StringBuilder StringBuilder { get; } = new StringBuilder();

    /// <inheritdoc/>
    public void Debug(string message)
    {
        this.StringBuilder.AppendLine(message);
    }

    /// <inheritdoc/>
    public void Error(string message)
    {
        this.StringBuilder.AppendLine(message);
    }

    /// <inheritdoc/>
    public void Info(string message)
    {
        this.StringBuilder.AppendLine(message);
    }
}
