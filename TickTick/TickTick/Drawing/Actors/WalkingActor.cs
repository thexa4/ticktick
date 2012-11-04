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

        /// <summary>
        /// Is a solid actor
        /// </summary>
        public bool IsSolid { get; set; }

        /// <summary>
        /// Is a platform
        /// </summary>
        public bool IsPlatform { get; set; }

        /// <summary>
        /// Current velocity
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Maximum velocity
        /// </summary>
        public Vector2 MaxVelocity { get; set; }

        /// <summary>
        /// Is falling flag
        /// </summary>
        public bool IsFalling { get; set; }

        /// <summary>
        /// List of touching collider platforms
        /// </summary>
        public List<ICollidable> TouchingPlatforms { get; set; }

        /// <summary>
        /// Creates a new walking actor
        /// </summary>
        /// <param name="game">Game to Bind to</param>
        /// <param name="layer">Layer</param>
        /// <param name="assetname">Assetname</param>
        public WalkingActor(Game game, Layer layer, string assetname) : base(game, layer, assetname)
        {
            // Add itself to collision manager
            ((CollisionManager)Game.Services.GetService(typeof(CollisionManager))).Add(this);

            TouchingPlatforms = new List<ICollidable>();
            MaxVelocity = new Vector2(2, 5);
            IsFalling = true;
        }

        /// <summary>
        /// Updates the actor
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Increase the speed
            Velocity = Velocity + Vector2.UnitY * Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity = new Vector2(
                MathHelper.Clamp(Velocity.X, -MaxVelocity.X, MaxVelocity.X),
                MathHelper.Clamp(Velocity.Y, -MaxVelocity.Y, MaxVelocity.Y)
            );

            // Update the position
            Position = Position + Velocity * 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Velocity.X < 0)
                SpriteEffect = SpriteEffects.FlipHorizontally;
            else if (Velocity.X > 0)
                SpriteEffect = SpriteEffects.None;

            base.Update(gameTime);
        }

        /// <summary>
        /// Process the touches
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

        /// <summary>
        /// Start touch event
        /// </summary>
        /// <param name="collider"></param>
        public virtual void StartTouch(ICollidable collider)
        {
            if (collider.IsPlatform && !IsBelow(collider))
                TouchingPlatforms.Add(collider);
        }

        /// <summary>
        /// End touch event
        /// </summary>
        /// <param name="collider"></param>
        public virtual void EndTouch(ICollidable collider)
        {
            if (collider.IsPlatform)
                if (TouchingPlatforms.Contains(collider))
                    TouchingPlatforms.Remove(collider);

            IsFalling = true;
        }

        /// <summary>
        /// Check if is below
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        protected bool IsBelow(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if (dir.Y > 0 && Math.Abs(dir.Y) > Math.Abs(dir.X))
                return true;
            return false;
        }

        /// <summary>
        /// Check if is above
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        protected bool IsAbove(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if(dir.Y < 0 && Math.Abs(dir.Y) > Math.Abs(dir.X))
                return true;
            return false;
        }

        /// <summary>
        /// Gets the direction
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public Vector2 GetDirection(ICollidable collider)
        {
            Vector2 mycenter = Position + Size / 2;
            Vector2 othercenter = collider.Position + collider.Size / 2;
            return othercenter - mycenter;
        }

        /// <summary>
        /// Check if is left
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        protected bool IsLeft(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if (dir.X < 0 && Math.Abs(dir.X) > Math.Abs(dir.Y))
                return true;
            return false;
        }

        /// <summary>
        /// Check if is right
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        protected bool IsRight(ICollidable collider)
        {
            Vector2 dir = GetDirection(collider);
            if (dir.X > 0 && Math.Abs(dir.X) > Math.Abs(dir.Y))
                return true;
            return false;
        }

        /// <summary>
        /// Moves out of the collider
        /// </summary>
        /// <param name="collider"></param>
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
