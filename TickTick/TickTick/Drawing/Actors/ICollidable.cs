using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing.Actors
{
    interface ICollidable
    {
        Vector2 Position;
        Vector2 Size;
        bool IsSolid;
        bool IsPlatform;

        void StartTouch(ICollidable collidee);
        void EndTouch(ICollidable collidee);
    }
}
