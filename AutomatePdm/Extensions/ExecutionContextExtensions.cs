// <copyright file="ExecutionContextExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

using System;
using System.Runtime.CompilerServices;

internal static class ExecutionContextExtensions
{
    public static void Log(this ExecutionContext context, WorkItem item, Exception ex, [CallerMemberName] string callerMember = "", [CallerFilePath] string filePath = "")
    {
        context.Log.AppendLine($"Error: {ex.GetType().Name} caught in {Path.GetFileName(filePath)}:{callerMember}()");
        context.Log.AppendLine($"File: {item.File.GetLocalPath(item.Folder.ID)}");
        context.Log.AppendLine(ex.Message);
        context.Log.AppendLine(ex.StackTrace);

        while (ex.InnerException is Exception innerEx)
        {
            ex = innerEx;
            context.Log.AppendLine($"Inner Exception: {ex.GetType().Name}");
            context.Log.AppendLine(ex.Message);
            context.Log.AppendLine(ex.StackTrace);
        }
    }

    public static void Log(this ExecutionContext context, string comment)
    {
        context.Log.AppendLine(comment);
    }
}
