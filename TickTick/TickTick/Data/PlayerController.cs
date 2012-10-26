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
        protected InputManager _input;
        public Player Player { get; protected set; }

        public const float MoveSpeed = 10;

        public PlayerController(Game game, Player player)
            : base(game)
        {
            Player = player;
        }

        public override void Initialize()
        {
            _input = (InputManager)Game.Services.GetService(typeof(InputManager));
        }

        public void UpdateInput(GameTime gameTime)
        {
            if (_input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                Player.Velocity = Player.Velocity - Vector2.UnitX * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                Player.Velocity = Player.Velocity + Vector2.UnitX * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
