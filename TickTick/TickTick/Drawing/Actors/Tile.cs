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
        public bool IsSolid { get; set; }
        public bool IsPlatform { get; set; }
        public Vector2 Velocity { get; set; }

        public bool IsHot { get; set; }
        public bool IsIce { get; set; }

        public Tile(Game game, Layer layer, string assetname)
            : base(game, layer, assetname)
        {
            // Add itself to collision manager
            ((CollisionManager)Game.Services.GetService(typeof(CollisionManager))).Add(this);
        }

        public virtual void StartTouch(ICollidable collider)
        {

        }

        public virtual void EndTouch(ICollidable collider)
        {

        }

        public virtual void ProcessTouches(List<ICollidable> colliders)
        {

        }
    }
}
