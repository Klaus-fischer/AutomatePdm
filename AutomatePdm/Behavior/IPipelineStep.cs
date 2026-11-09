// <copyright file="IPipelineStep.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Defines a step in a processing pipeline
/// that can be executed on work items
/// within a given execution context.
/// </summary>
[JsonConverter(typeof(IPipelineStepConverter))]
public interface IPipelineStep
{
    /// <summary>
    /// Gets or sets the metadata for the pipeline step, including title and description.
    /// </summary>
    public Metadata Metadata { get; set; }

    /// <summary>
    /// Gets or sets the next pipeline step to execute after this one.
    /// </summary>
    public IPipelineStep? Next { get; set; }

    /// <summary>
    /// Executes the pipeline step on the provided work items within the given execution context.
    /// </summary>
    /// <param name="items">The work items to process.</param>
    /// <param name="ctx">The execution context.</param>
    void Execute(WorkItem[] items, ExecutionContext ctx);
}

/// <summary>
/// A custom JSON converter for the <see cref="IPipelineStep"/> interface,
/// enabling serialization and deserialization of pipeline steps in a polymorphic manner.
/// </summary>
internal class IPipelineStepConverter : JsonConverter<IPipelineStep>
{
    private static readonly FrozenDictionary<string, Type> PipelineStepTypes;

    static IPipelineStepConverter()
    {
        PipelineStepTypes = typeof(IPipelineStep).Assembly.GetTypes()
            .Where(t => typeof(IPipelineStep).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Where(t => !t.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Any())
            .ToFrozenDictionary(o => o.Name);
    }

    /// <inheritdoc/>
    public override IPipelineStep? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        IPipelineStep? result = null;
        IPipelineStep? current = null;
        foreach (var item in root.EnumerateArray())
        {
            var typeName = item.GetProperty("type").GetString();
            if (typeName == null)
            {
                continue;
            }

            if (!PipelineStepTypes.TryGetValue(typeName, out var expressionType))
            {
                continue;
            }

            var optionsProperty = item.GetProperty("options");
            var optionsJson = optionsProperty.GetRawText();
            var pipelineStepObject = JsonSerializer.Deserialize(optionsJson, expressionType, options);

            if (pipelineStepObject is IPipelineStep pipelineStep)
            {
                result ??= pipelineStep;

                if (current != null)
                {
                    current.Next = pipelineStep;
                }

                current = pipelineStep;
            }
        }

        return result;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, IPipelineStep value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        var pipelineStep = value;

        while (pipelineStep is not null)
        {
            writer.WriteStartObject();
            writer.WriteString("type", pipelineStep.GetType().Name);
            writer.WritePropertyName("options");
            JsonSerializer.Serialize(writer, pipelineStep, pipelineStep.GetType(), options);
            writer.WriteEndObject();

            pipelineStep = pipelineStep.Next;
        }

        writer.WriteEndArray();
    }
}