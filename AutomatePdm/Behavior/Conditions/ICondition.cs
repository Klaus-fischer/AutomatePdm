// <copyright file="ICondition.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(ConditionConverter))]
public interface ICondition
{
    /// <summary>
    /// Gets a value indicating whether the output should be negated.
    /// </summary>
    public bool Negate { get; init; }

    /// <summary>
    /// Checks if the condition is valid.
    /// </summary>
    /// <param name="item">Item to check.</param>
    /// <param name="ctx">The current Execution context.</param>
    /// <returns>True if condition is valid.</returns>
    bool IsValid(WorkItem item, ExecutionContext ctx);
}

internal class ConditionConverter : JsonConverter<ICondition>
{
    private static readonly FrozenDictionary<string, Type> ConditionTypes;

    static ConditionConverter()
    {
        ConditionTypes = typeof(ICondition).Assembly.GetTypes()
            .Where(t => typeof(ICondition).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Where(t => !t.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Any())
            .ToFrozenDictionary(o => o.Name);
    }

    /// <inheritdoc/>
    public override ICondition? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var typeName = root.GetProperty("type").GetString();
        if (typeName == null)
        {
            return null;
        }

        if (!ConditionTypes.TryGetValue(typeName, out var conditionType))
        {
            return null;
        }

        var optionsProperty = root.GetProperty("options");
        var optionsJson = optionsProperty.GetRawText();
        var expression = JsonSerializer.Deserialize(optionsJson, conditionType, options);

        return expression as ICondition;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ICondition value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.GetType().Name);
        writer.WritePropertyName("options");
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
        writer.WriteEndObject();
    }
}