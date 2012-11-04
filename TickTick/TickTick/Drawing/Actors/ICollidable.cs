using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing.Actors
{
    public interface ICollidable
    {
        /// <summary>
        /// Position of the collidable
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Velocity of the collidable
        /// </summary>
        Vector2 Velocity { get; set; }
        
        /// <summary>
        /// Size of the collidable
        /// </summary>
        Vector2 Size { get; set; }

        /// <summary>
        /// Is the collidable solid
        /// </summary>
        bool IsSolid { get; set; }

        /// <summary>
        /// Is the collidable a platform
        /// </summary>
        bool IsPlatform { get; set; }

        /// <summary>
        /// Starts touching a collider
        /// </summary>
        /// <param name="collidee"></param>
        void StartTouch(ICollidable collidee);

        /// <summary>
        /// Processes touches
        /// </summary>
        /// <param name="collidees"></param>
        void ProcessTouches(List<ICollidable> collidees);

        /// <summary>
        /// Stops touching a collider
        /// </summary>
        /// <param name="collidee"></param>
        void EndTouch(ICollidable collidee);
    }
}
