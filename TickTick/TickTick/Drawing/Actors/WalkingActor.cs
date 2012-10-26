﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;

namespace TickTick.Drawing.Actors
{
    public class WalkingActor : LevelSprite, ICollidable
    {
        const float Gravity = 1f;

        public bool IsSolid { get; set; }
        public bool IsPlatform { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 MaxVelocity { get; set; }
        public bool IsFalling { get; set; }

        public WalkingActor(Game game, Layer layer, string assetname) : base(game, layer, assetname)
        {
            // Add itself to collision manager
            ((CollisionManager)Game.Services.GetService(typeof(CollisionManager))).Add(this);

            MaxVelocity = new Vector2(10, 10);
            IsFalling = true;
        }

        public override void Update(GameTime gameTime)
        {
            if(IsFalling)
                Velocity = Velocity + Vector2.UnitY * Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity = new Vector2(
                MathHelper.Clamp(Velocity.X, -MaxVelocity.X, MaxVelocity.X),
                MathHelper.Clamp(Velocity.Y, -MaxVelocity.Y, MaxVelocity.Y)
            );

            Position = Position + Velocity;
            base.Update(gameTime);
        }

        public void StartTouch(ICollidable collider)
        {
            if (!collider.IsSolid)
                return;

            if(IsBelow(collider))
                IsFalling = false;

            MoveOut(collider);
            if (IsAbove(collider) && Velocity.Y < 0 && !collider.IsPlatform)
                Velocity = new Vector2(Velocity.X, 0);

            if (IsLeft(collider) && Velocity.X < 0)
                Velocity = new Vector2(0, Velocity.Y);

            if (IsRight(collider) && Velocity.X > 0)
                Velocity = new Vector2(0, Velocity.Y);

            if (IsBelow(collider) && Velocity.Y > 0)
                Velocity = new Vector2(Velocity.X, 0);
        }

        public void EndTouch(ICollidable collider)
        {
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
            //if (IsAbove(collider) || IsBelow(collider))
                MoveYOut(collider);
            //if (IsLeft(collider) || IsRight(collider))
                MoveXOut(collider);
        }

        protected void MoveYOut(ICollidable collider)
        {
            if (Velocity.Y == 0 || !CollisionManager.Collides(this, collider))
                return;

            float dy = 0;
            if (Position.Y < collider.Position.Y && (Position.Y + Size.Y) > collider.Position.Y)
                dy = collider.Position.Y - Position.Y - Size.Y;
            else if(Position.Y < (collider.Position.Y + collider.Size.Y) && (Position.Y + Size.Y) > (collider.Position.Y + collider.Size.Y))
                dy = collider.Position.Y + collider.Size.Y - Position.Y;

            Vector2 movement = new Vector2((dy / Velocity.Y) * Velocity.X, dy);
            Position += movement;
        }

        protected void MoveXOut(ICollidable collider)
        {
            if(Velocity.X == 0 || !CollisionManager.Collides(this, collider))
                return;

            float dx = 0;
            if (Position.X < collider.Position.X && (Position.X + Size.X) > collider.Position.X)
                dx = collider.Position.X - Position.X - Size.X;
            else if(Position.X < (collider.Position.X + collider.Size.X) && (Position.X + Size.X) > (collider.Position.X + collider.Size.X))
                dx = collider.Position.X + collider.Size.X - Position.X;

            Vector2 movement = new Vector2(dx, (dx / Velocity.X) * Velocity.Y);
            Position += movement;
        }
    }
}
