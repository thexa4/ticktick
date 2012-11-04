using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;
using Microsoft.Xna.Framework.Graphics;

namespace TickTick.Drawing.Actors
{
    public class WalkingActor : LevelSprite, ICollidable
    {
        const float Gravity = 12f;

        public bool IsSolid { get; set; }
        public bool IsPlatform { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 MaxVelocity { get; set; }
        public bool IsFalling { get; set; }
        public List<ICollidable> TouchingPlatforms { get; set; }

        public WalkingActor(Game game, Layer layer, string assetname) : base(game, layer, assetname)
        {
            // Add itself to collision manager
            ((CollisionManager)Game.Services.GetService(typeof(CollisionManager))).Add(this);

            TouchingPlatforms = new List<ICollidable>();
            MaxVelocity = new Vector2(2, 5);
            IsFalling = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Velocity = Velocity + Vector2.UnitY * Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity = new Vector2(
                MathHelper.Clamp(Velocity.X, -MaxVelocity.X, MaxVelocity.X),
                MathHelper.Clamp(Velocity.Y, -MaxVelocity.Y, MaxVelocity.Y)
            );

            Position = Position + Velocity;

            if (Velocity.X < 0)
                SpriteEffect = SpriteEffects.FlipHorizontally;
            else if (Velocity.X > 0)
                SpriteEffect = SpriteEffects.None;

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colliders"></param>
        public virtual void ProcessTouches(List<ICollidable> colliders)
        {
            Vector2 prev = Position;
            IsFalling = true;

            foreach (var col in colliders)
                if (col.IsSolid || (col.IsPlatform && !TouchingPlatforms.Contains(col)))
                {
                    if (IsBelow(col) || IsAbove(col))
                        MoveOut(col);

                    if (IsBelow(col))
                        IsFalling = false;
                }

            foreach (var col in colliders)
                if (col.IsSolid || (col.IsPlatform && !TouchingPlatforms.Contains(col)))
                    if (IsLeft(col) || IsRight(col))
                        MoveOut(col);

            Vector2 diff = Position - prev;

            if (diff.X > 0 && Velocity.X < 0)
                Velocity *= Vector2.UnitY;
            if (diff.X < 0 && Velocity.X > 0)
                Velocity *= Vector2.UnitY;

            if (diff.Y < 0 && Velocity.Y > 0)
                Velocity *= Vector2.UnitX;
            if (diff.Y > 0 && Velocity.Y < 0)
                Velocity *= Vector2.UnitX;
        }

        public virtual void StartTouch(ICollidable collider)
        {
            if (collider.IsPlatform && !IsBelow(collider))
                TouchingPlatforms.Add(collider);
        }

        public virtual void EndTouch(ICollidable collider)
        {
            if (collider.IsPlatform)
                if (TouchingPlatforms.Contains(collider))
                    TouchingPlatforms.Remove(collider);

            IsFalling = true;
        }

        protected bool IsBelow(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if (dir.Y > 0 && Math.Abs(dir.Y) > Math.Abs(dir.X))
                return true;
            return false;
        }

        protected bool IsAbove(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if(dir.Y < 0 && Math.Abs(dir.Y) > Math.Abs(dir.X))
                return true;
            return false;
        }

        public Vector2 GetDirection(ICollidable collider)
        {
            Vector2 mycenter = Position + Size / 2;
            Vector2 othercenter = collider.Position + collider.Size / 2;
            return othercenter - mycenter;
        }

        protected bool IsLeft(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if (dir.X < 0 && Math.Abs(dir.X) > Math.Abs(dir.Y))
                return true;
            return false;
        }

        protected bool IsRight(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if (dir.X > 0 && Math.Abs(dir.X) > Math.Abs(dir.Y))
                return true;
            return false;
        }

        public void MoveOut(ICollidable collider)
        {
            Vector2 mycenter = Position + Size / 2;
            Vector2 othercenter = collider.Position + collider.Size / 2;

            Vector2 minDistance = (Size + collider.Size) / 2;
            
            Vector2 distance = othercenter - mycenter;
            Vector2 depth = Vector2.Zero;
            if (distance.X > 0)
                depth.X = minDistance.X - distance.X;
            else
                depth.X = -minDistance.X - distance.X;
            if (distance.Y > 0)
                depth.Y = minDistance.Y - distance.Y;
            else
                depth.Y = -minDistance.Y - distance.Y;

            if (IsAbove(collider) || IsBelow(collider))
                Position = new Vector2(Position.X, Position.Y - depth.Y);
            if (CollisionManager.Collides(this, collider) && (IsLeft(collider) || IsRight(collider)))
                Position = new Vector2(Position.X - depth.X, Position.Y);
        }
    }
}
