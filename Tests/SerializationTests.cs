namespace AutomatePdm.Tests;

using AutomatePdm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

[TestClass]
public sealed class SerializationTests
{
    [TestMethod]
    public void ChangeStateActionSerialization()
    {
        var action = new ChangeStateAction()
        {
            TargetStateName = "TestState",
            Comment = "State Changed with comments.",
        };

        string serialized = JsonSerializer.Serialize(action);

        Assert.IsTrue(serialized.Contains(action.TargetStateName, StringComparison.Ordinal));
        Assert.IsTrue(serialized.Contains(action.Comment, StringComparison.Ordinal));

        var deserialized = JsonSerializer.Deserialize<ChangeStateAction>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(action.TargetStateName, deserialized.TargetStateName);
        Assert.AreEqual(action.Comment, deserialized.Comment);
    }

    [TestMethod]
    public void ScheduleTaskActionSerialization()
    {
        var action = new ScheduleTaskAction()
        {
            TaskName = "TestTask",
            Comment = "Task scheduled with comments.",
        };

        string serialized = JsonSerializer.Serialize(action);

        Assert.IsTrue(serialized.Contains(action.TaskName, StringComparison.Ordinal));
        Assert.IsTrue(serialized.Contains(action.Comment, StringComparison.Ordinal));

        var deserialized = JsonSerializer.Deserialize<ScheduleTaskAction>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(action.TaskName, deserialized.TaskName);
        Assert.AreEqual(action.Comment, deserialized.Comment);
    }

    [TestMethod]
    public void TextExpressionSerialization()
    {
        var expression = new TextExpression() { Text = "Test Text" };
        string serialized = JsonSerializer.Serialize(expression);
        Assert.IsTrue(serialized.Contains(expression.Text, StringComparison.Ordinal));
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
        Assert.IsNotNull(deserialized);
    }

    [TestMethod]
    public void PropertyExpressionSerialization()
    {
        var expression = new PropertyExpression()
        {
            PropertyName = "TestProperty",
            ConfigurationName = "TestConfiguration"
        };

        string serialized = JsonSerializer.Serialize(expression);
        Assert.IsTrue(serialized.Contains(expression.PropertyName, StringComparison.Ordinal));
        Assert.IsTrue(serialized.Contains(expression.ConfigurationName, StringComparison.Ordinal));

        var deserialized = JsonSerializer.Deserialize<PropertyExpression>(serialized);
        Assert.IsNotNull(deserialized);

        Assert.AreEqual(expression.PropertyName, deserialized.PropertyName);
        Assert.AreEqual(expression.ConfigurationName, deserialized.ConfigurationName);
    }

    [TestMethod]
    public void ConcatStringsExpressionSerialization()
    {
        var expression = new ConcatStringsExpression()
        {
            Expressions = [new TextExpression() { Text = "Hello, " }, new TextExpression() { Text = "World!" }],
        };

        string serialized = JsonSerializer.Serialize(expression);

        Assert.IsTrue(serialized.Contains("Hello, ", StringComparison.Ordinal));
        Assert.IsTrue(serialized.Contains("World!", StringComparison.Ordinal));

        var deserialized = JsonSerializer.Deserialize<ConcatStringsExpression>(serialized);
        Assert.IsNotNull(deserialized);

        Assert.IsTrue(deserialized.Expressions.OfType<TextExpression>().Select(o => o.Text)
            .SequenceEqual(expression.Expressions.OfType<TextExpression>().Select(o => o.Text)));
    }
}
