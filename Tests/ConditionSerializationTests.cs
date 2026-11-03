namespace AutomatePdm.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.Json;

[TestClass]
public class ConditionSerializationTests
{
    [TestMethod]
    public void StatusConditionSerialization()
    {
        var statusCondition = new StatusCondition() { ValidStates = ["Release", "Review"], Negate = true };

        string serialized = JsonSerializer.Serialize(statusCondition);
        var deserialized = JsonSerializer.Deserialize<StatusCondition>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(statusCondition.ValidStates.Length, deserialized.ValidStates.Length);
        Assert.AreEqual(statusCondition.ValidStates[0], deserialized.ValidStates[0]);
        Assert.AreEqual(statusCondition.ValidStates[1], deserialized.ValidStates[1]);
        Assert.AreEqual(statusCondition.Negate, deserialized.Negate);
    }

    [TestMethod]
    public void ExpressionConditionSerialization()
    {
        var expressionCondition = new ExpressionCondition()
        {
            First = new TextExpression() { Text = "Test" },
            Second = new TextExpression() { Text = "TestSecond" },
            CaseSensitive = true
        };

        string serialized = JsonSerializer.Serialize(expressionCondition);
        var deserialized = JsonSerializer.Deserialize<ExpressionCondition>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(expressionCondition.CaseSensitive, deserialized.CaseSensitive);
        Assert.AreEqual(((TextExpression)expressionCondition.First).Text, ((TextExpression)deserialized.First).Text);
        Assert.AreEqual(((TextExpression)expressionCondition.Second).Text, ((TextExpression)deserialized.Second).Text);
    }

    [TestMethod]
    public void AndConditionSerialization()
    {
        var andCondition = new AndCondition()
        {
            Conditions = [
                new StatusCondition() { ValidStates = ["Release"] },
                new ExpressionCondition()
                {
                    First = new TextExpression() { Text = "Test" },
                    Second = new TextExpression() { Text = "TestSecond" }
                }
            ],
            Negate = true,
        };

        string serialized = JsonSerializer.Serialize(andCondition);
        var deserialized = JsonSerializer.Deserialize<AndCondition>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.IsTrue(deserialized.Negate);
        Assert.AreEqual(andCondition.Conditions.Count, deserialized.Conditions.Count);
        Assert.IsTrue(deserialized.Conditions[0] is StatusCondition);
        Assert.IsTrue(deserialized.Conditions[1] is ExpressionCondition);
    }

    [TestMethod]
    public void OrConditionSerialization()
    {
        var orCondition = new OrCondition()
        {
            Conditions = [
                new StatusCondition() { ValidStates = ["Release"] },
                new ExpressionCondition()
                {
                    First = new TextExpression() { Text = "Test" },
                    Second = new TextExpression() { Text = "TestSecond" }
                }
            ],
            Negate = true,
        };

        string serialized = JsonSerializer.Serialize(orCondition);
        var deserialized = JsonSerializer.Deserialize<OrCondition>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.IsTrue(deserialized.Negate);
        Assert.AreEqual(orCondition.Conditions.Count, deserialized.Conditions.Count);
        Assert.IsTrue(deserialized.Conditions[0] is StatusCondition);
        Assert.IsTrue(deserialized.Conditions[1] is ExpressionCondition);
    }

    [TestMethod]
    public void OneOfConditionSerialization()
    {
        var oneOfCondition = new OneOfCondition()
        {
            Conditions = [
                new StatusCondition() { ValidStates = ["Release"] },
                new ExpressionCondition()
                {
                    First = new TextExpression() { Text = "Test" },
                    Second = new TextExpression() { Text = "TestSecond" }
                }
            ],
            Negate = true,
        };

        string serialized = JsonSerializer.Serialize(oneOfCondition);
        var deserialized = JsonSerializer.Deserialize<OrCondition>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.IsTrue(deserialized.Negate);
        Assert.AreEqual(oneOfCondition.Conditions.Count, deserialized.Conditions.Count);
        Assert.IsTrue(deserialized.Conditions[0] is StatusCondition);
        Assert.IsTrue(deserialized.Conditions[1] is ExpressionCondition);
    }
}
