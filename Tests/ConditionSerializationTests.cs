namespace AutomatePdm.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
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

}
