// <copyright file="ILogger.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm;

public interface ILogger
{
    void Debug(string message);

    void Error(string message);

    void Info(string message);
}
