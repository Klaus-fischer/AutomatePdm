// <copyright file="ICloneable{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.GUI;

/// <summary>
/// Defines a method for creating a deep copy of an object. 
/// </summary>
/// <typeparam name="T">The type of object to clone.</typeparam>
public interface ICloneable<T>
{
    /// <summary>
    /// Creates an deep 1:1 copy of the object.
    /// </summary>
    /// <returns>A deep copy of the object.</returns>
    T Clone();
}
