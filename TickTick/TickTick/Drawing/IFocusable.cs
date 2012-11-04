using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing
{
    public interface IFocusable
    {
        /// <summary>
        /// Position
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Size of the focusable
        /// </summary>
        Vector2 Size { get; }
    }
}
