using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TickTick.Services
{
    /// <summary>
    /// ControllerAction is the result enumeration for the Controller classes
    /// which handle input.
    /// </summary>
    [Flags]
    public enum ControllerAction : byte
    {
        None = 0,

        Left = 1,
        Right = 2,

        Jump = 4,
    }
}
