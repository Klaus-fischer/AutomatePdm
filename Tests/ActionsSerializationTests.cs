namespace AutomatePdm.Tests;

using AutomatePdm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

[TestClass]
public sealed class ActionsSerializationTests
{
    [TestMethod]
    public void IPipelineStepSerialization()
    {
        IPipelineStep action = new ChangeStateAction()
        {
            TargetStateName = "FirstState",
            Comment = "State Changed with comments.",
            Next = new ChangeStateAction()
            {
                TargetStateName = "SecondState",
                Comment = "Next State Changed with comments.",
                Next = new ScheduleTaskAction()
                {
                    TaskName = "FinalTask",
                    Comment = "Final Task scheduled with comments."
                }
            }
        };

        string serialized = JsonSerializer.Serialize(action, typeof(IPipelineStep));

        var deserialized = JsonSerializer.Deserialize<IPipelineStep>(serialized);

        if (deserialized is not ChangeStateAction firstAction ||
            firstAction.Next is not ChangeStateAction secondAction ||
            secondAction.Next is not ScheduleTaskAction finalAction)
        {
            Assert.Fail("Deserialized object structure does not match the expected hierarchy.");
            return;
        }

        Assert.AreEqual("FirstState", firstAction.TargetStateName);
        Assert.AreEqual("State Changed with comments.", firstAction.Comment);

        Assert.AreEqual("SecondState", secondAction.TargetStateName);
        Assert.AreEqual("Next State Changed with comments.", secondAction.Comment);

        Assert.AreEqual("FinalTask", finalAction.TaskName);
        Assert.AreEqual("Final Task scheduled with comments.", finalAction.Comment);
    }

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
    public void GetFileVariableActionsSerialization()
    {
        var expression = new GetFileVariablesAction()
        {
            VariableNames = ["TestVariable1", "TestVariable2"],
            ConfigurationName = "@",
        };

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<GetFileVariablesAction>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(expression.VariableNames.Length, deserialized.VariableNames.Length);
        Assert.AreEqual(expression.VariableNames[0], deserialized.VariableNames[0]);
        Assert.AreEqual(expression.VariableNames[1], deserialized.VariableNames[1]);
        Assert.AreEqual(expression.ConfigurationName, deserialized.ConfigurationName);
    }

    [TestMethod]
    public void SetFileVariablesActionSerialization()
    {
        var expression = new SetFileVariableAction()
        {
            VariableName = "TestVariable1",
            ConfigurationName = "@",
            Expression = new TextExpression()
            {
                Text = "TestValue",
            },
        };

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<SetFileVariableAction>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(expression.VariableName, deserialized.VariableName);
        Assert.AreEqual(expression.ConfigurationName, deserialized.ConfigurationName);
        Assert.IsTrue(deserialized.Expression is TextExpression);
        Assert.AreEqual(((TextExpression)expression.Expression).Text, (deserialized.Expression as TextExpression)?.Text);
    }

    [TestMethod]
    public void GetProjectVariablesActionSerialization()
    {
        var expression = new GetProjectVariablesAction()
        {
            VariableNames = ["TestVariable1", "TestVariable2"],
        };

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<GetProjectVariablesAction>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(expression.VariableNames.Length, deserialized.VariableNames.Length);
        Assert.AreEqual(expression.VariableNames[0], deserialized.VariableNames[0]);
        Assert.AreEqual(expression.VariableNames[1], deserialized.VariableNames[1]);
    }

    [TestMethod]
    public void SetProjectVariablesActionSerialization()
    {
        var expression = new SetProjectVariableAction()
        {
            VariableName = "TestVariable1",
            Expression = new TextExpression()
            {
                Text = "TestValue",
            },
        };

        string serialized = JsonSerializer.Serialize(expression);
        var deserialized = JsonSerializer.Deserialize<SetProjectVariableAction>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.AreEqual(expression.VariableName, deserialized.VariableName);
        Assert.IsTrue(deserialized.Expression is TextExpression);
        Assert.AreEqual(((TextExpression)expression.Expression).Text, (deserialized.Expression as TextExpression)?.Text);
    }
}