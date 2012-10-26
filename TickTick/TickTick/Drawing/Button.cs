using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;

namespace TickTick.Drawing
{
    public delegate void ButtonClickDelegate(Button button, Vector2 relativePosition);

    public class Button : Sprite
    {
        public event ButtonClickDelegate OnClicked = delegate { };
        protected InputManager _inputManager;

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <param name="game">Game to bind to</param>
        /// <param name="inputManager">InputManager reference</param>
        /// <param name="assetName">Sprite to load</param>
        public Button(Game game, InputManager inputManager, String assetName) : base(game, assetName)
        {
            _inputManager = inputManager;
        }

        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleInput(gameTime);
        }

        /// <summary>
        /// Handles the input
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public virtual void HandleInput(GameTime gameTime) 
        {
            if (_inputManager.Mouse.IsButtonReleased(MouseButton.Left) &&
                _inputManager.Mouse.IsOverObj(this.Position, this.SourceRectangle))
            {
                var relPos = _inputManager.Mouse.Position - this.Position + new Vector2(this.SourceRectangle.X, this.SourceRectangle.Y);
                this.OnClicked.Invoke(this, relPos);
            }
        }
    }
}
