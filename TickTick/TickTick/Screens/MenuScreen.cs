using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickTick.Services;
using TickTick.Drawing;
using Microsoft.Xna.Framework;

namespace TickTick.Screens
{
    public abstract class MenuScreen : GameScreen
    {
        /// <summary>
        /// Background layer
        /// </summary>
        public Layer Background { get; protected set; }

        /// <summary>
        /// Foreground layer
        /// </summary>
        public Layer Foreground { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuScreen(Game game) : base(game)
        {
            this.Game = game;
            this.Background = new Layer(this.Game);
            this.Foreground = new Layer(this.Game);
        }

        /// <summary>
        /// Initializes the screen
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.Background.Initialize();
            this.Foreground.Initialize();
        }

        /// <summary>
        /// Frame renewal
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        /// <param name="otherScreenHasFocus">!Game.IsActive</param>
        /// <param name="coveredByOtherScreen">Is other screen visible</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            this.Background.Update(gameTime);
            this.Foreground.Update(gameTime);
        }

        /// <summary>
        /// Draw Frame
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.ScreenState != Services.ScreenState.Active)
                return;

            base.Draw(gameTime);

            //this.ScreenManager.SpriteBatch.Begin();
            this.Background.Draw(gameTime);
            this.Foreground.Draw(gameTime);
            //this.ScreenManager.SpriteBatch.End();
        }
    }
}
