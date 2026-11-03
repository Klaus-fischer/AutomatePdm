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
}
