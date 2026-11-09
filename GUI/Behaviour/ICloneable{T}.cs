namespace AutomatePdm.GUI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal interface ICloneable<T>
{
    /// <summary>
    /// Creates an deep 1:1 copy of the object.
    /// </summary>
    /// <returns>A deep copy of the object.</returns>
    T Clone();
}
