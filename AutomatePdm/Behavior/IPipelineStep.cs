// <copyright file="IPipelineStep.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(IPipelineStepConverter))]
public interface IPipelineStep
{
    void Execute(WorkItem[] items, ExecutionContext ctx);

    public IPipelineStep? Next { get; set; }
}

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