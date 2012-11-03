using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing.Actors
{
    public interface ICollidable
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 Size { get; set; }
        bool IsSolid { get; set; }
        bool IsPlatform { get; set; }

        void StartTouch(ICollidable collidee);
        void ProcessTouches(List<ICollidable> collidees);
        void EndTouch(ICollidable collidee);
    }
}
