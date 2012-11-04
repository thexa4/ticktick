using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;
using TickTick.Drawing.Actors;

namespace TickTick.Data
{
    /// <summary>
    /// This controls the player
    /// </summary>
    public class PlayerController : GameComponent
    {
        protected KeyboardController _kc;
        public Player Player { get; protected set; }

        public const float MoveSpeed = 10;
        public const float JumpSpeed = 6;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="player"></param>
        public PlayerController(Game game, KeyboardController kc, Player player)
            : base(game)
        {
            Player = player;
            _kc = kc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateInput(GameTime gameTime)
        {
            var action = _kc.Action;

            if (action.HasFlag(ControllerAction.Left))
                Player.Velocity = Player.Velocity - Vector2.UnitX * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (action.HasFlag(ControllerAction.Right))
                Player.Velocity = Player.Velocity + Vector2.UnitX * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!Player.IsFalling && action.HasFlag(ControllerAction.Jump))
                Player.Velocity = Player.Velocity * Vector2.UnitX - Vector2.UnitY * JumpSpeed;

            if (!Player.IsFalling && !(action == ControllerAction.Left || action == ControllerAction.Right))
                if (!Player.IsOnIce)
                    Player.Velocity *= Vector2.UnitY;
        }
    }
}
