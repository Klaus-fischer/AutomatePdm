// <copyright file="ExpressionCollection.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Frozen;

namespace AutomatePdm;

[JsonConverter(typeof(ExpressionCollectionConverter))]
public class ExpressionCollection : List<IExpression>
{
}

internal class ExpressionCollectionConverter : JsonConverter<ExpressionCollection>
{
    private static readonly FrozenDictionary<string, Type> ExpressionTypes;

    static ExpressionCollectionConverter()
    {
        ExpressionTypes = typeof(IExpression).Assembly.GetTypes()
            .Where(t => typeof(IExpression).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Where(t => !t.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Any())
            .ToFrozenDictionary(o => o.Name);
    }

    /// <inheritdoc/>
    public override ExpressionCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var collection = new ExpressionCollection();
        foreach (var item in root.EnumerateArray())
        {
            var typeName = item.GetProperty("type").GetString();
            if (typeName == null)
            {
                continue;
            }

            if (!ExpressionTypes.TryGetValue(typeName, out var expressionType))
            {
                continue;
            }

            var optionsProperty = item.GetProperty("options");
            var optionsJson = optionsProperty.GetRawText();
            var expression = JsonSerializer.Deserialize(optionsJson, expressionType, options);

            if (expression is IExpression exp)
            {
                collection.Add(exp);
            }
        }

        return collection;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ExpressionCollection value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var expression in value)
        {
            writer.WriteStartObject();
            writer.WriteString("type", expression.GetType().Name);
            writer.WritePropertyName("options");
            JsonSerializer.Serialize(writer, expression, expression.GetType(), options);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}