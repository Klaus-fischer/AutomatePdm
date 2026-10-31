namespace AutomatePdm.Tests;

using AutomatePdm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

[TestClass]
public sealed class ActionsSerializationTests
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
}
