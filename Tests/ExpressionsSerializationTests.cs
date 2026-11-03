namespace AutomatePdm.Tests;

using AutomatePdm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

[TestClass]
public sealed class ExpressionsSerializationTests
{
    [TestMethod]
    public void TextExpressionSerialization()
    {
        var expression = new TextExpression() { Text = "Test Text" };

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<TextExpression>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(expression.Text, deserialized.Text);
    }

    [TestMethod]
    public void StateNameExpressionSerialization()
    {
        var expression = new StateNameExpression();

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<StateNameExpression>(serialized);

        Assert.IsTrue(deserialized is StateNameExpression);
    }

    [TestMethod]
    public void PropertyExpressionSerialization()
    {
        var expression = new PropertyExpression()
        {
            PropertyName = "TestProperty",
        };

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<PropertyExpression>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(expression.PropertyName, deserialized.PropertyName);
    }

    [TestMethod]
    public void ConcatStringsExpressionSerialization()
    {
        var expression = new ConcatStringsExpression()
        {
            ExpressionColletion = [new TextExpression() { Text = "Hello, " }, new TextExpression() { Text = "World!" }],
        };

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<ConcatStringsExpression>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.IsTrue(deserialized.ExpressionColletion.OfType<TextExpression>().Select(o => o.Text)
            .SequenceEqual(expression.ExpressionColletion.OfType<TextExpression>().Select(o => o.Text)));
    }
}
