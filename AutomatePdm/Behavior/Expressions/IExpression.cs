// <copyright file="IExpression.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(ExpressionConverter))]
public interface IExpression
{
    string Resolve(WorkItem workItem, ExecutionContext ctx);
}

internal class ExpressionConverter : JsonConverter<IExpression>
{
    private static readonly FrozenDictionary<string, Type> ExpressionTypes;

    static ExpressionConverter()
    {
        ExpressionTypes = typeof(IExpression).Assembly.GetTypes()
            .Where(t => typeof(IExpression).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Where(t => !t.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Any())
            .ToFrozenDictionary(o => o.Name);
    }

    /// <inheritdoc/>
    public override IExpression? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var typeName = root.GetProperty("type").GetString();
        if (typeName == null)
        {
            return null;
        }

        if (!ExpressionTypes.TryGetValue(typeName, out var expressionType))
        {
            return null;
        }

        var optionsProperty = root.GetProperty("options");
        var optionsJson = optionsProperty.GetRawText();
        var expression = JsonSerializer.Deserialize(optionsJson, expressionType, options);

        return expression as IExpression;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, IExpression value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.GetType().Name);
        writer.WritePropertyName("options");
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
        writer.WriteEndObject();
    }
}