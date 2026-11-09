// <copyright file="Metadata.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

/// <summary>
/// Represents metadata information for a pipeline step, including its title and description.
/// </summary>
/// <param name="Title">The title of the pipeline step.</param>
/// <param name="Description">The description of the pipeline step.</param>
public record Metadata(string Title, string Description)
{
    /// <summary>
    /// Gets an empty metadata instance with default values for title and description.
    /// </summary>
    public static Metadata Empty { get; } = new Metadata(string.Empty, string.Empty);
}
