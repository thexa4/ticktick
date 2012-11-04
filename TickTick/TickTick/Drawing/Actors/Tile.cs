using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;

namespace TickTick.Drawing.Actors
{
    class Tile : LevelSprite, ICollidable
    {
        /// <summary>
        /// Tile is solid
        /// </summary>
        public bool IsSolid { get; set; }

        /// <summary>
        /// Tile is a platform
        /// </summary>
        public bool IsPlatform { get; set; }

        /// <summary>
        /// Velocity of the tile
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Is a hot tile
        /// </summary>
        public bool IsHot { get; set; }

        /// <summary>
        /// Is an ice tile
        /// </summary>
        public bool IsIce { get; set; }

        /// <summary>
        /// Creates a new tile
        /// </summary>
        /// <param name="game"></param>
        /// <param name="layer"></param>
        /// <param name="assetname"></param>
        public Tile(Game game, Layer layer, string assetname)
            : base(game, layer, assetname)
        {
            // Add itself to collision manager
            ((CollisionManager)Game.Services.GetService(typeof(CollisionManager))).Add(this);
        }

        /// <summary>
        /// Starts touching event
        /// </summary>
        /// <param name="collider"></param>
        public virtual void StartTouch(ICollidable collider)
        {

        }

        /// <summary>
        /// Ends touching event
        /// </summary>
        /// <param name="collider"></param>
        public virtual void EndTouch(ICollidable collider)
        {

        }

        /// <summary>
        /// Processes collisions
        /// </summary>
        /// <param name="colliders"></param>
        public virtual void ProcessTouches(List<ICollidable> colliders)
        {

        }
    }
}
