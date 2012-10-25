using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TickTick.Services;
using Microsoft.Xna.Framework.Graphics;
using TickTick.Drawing;

namespace TickTick.Screens
{
    /// <summary>
    /// 
    /// </summary>
    public class TitleScreen : GameScreen
    {
        protected Sprite _background, _playButton, _helpButton;
        protected Int32 _menuIndex;

        /// <summary>
        /// Loads all content for this screen
        /// </summary>
        /// <param name="contentManager">ContentManager to load to</param>
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.ScreenManager.SpriteFonts.LoadFont("Title", "Fonts/Title");
            this.ScreenManager.SpriteFonts.LoadFont("Help", "Fonts/Default");

            _background = new Sprite(this.Game, "Backgrounds/spr_title")
            {
                Position = Vector2.Zero,

            };
        }

        /// <summary>
        /// Update logic
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        /// <param name="otherScreenHasFocus">Game is blurred</param>
        /// <param name="coveredByOtherScreen">Other GameScreen is active</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        /// <summary>
        /// Processes input
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public override void HandleInput(GameTime gameTime)
        {
            base.HandleInput(gameTime);

            if (this.InputManager.Keyboard.IsKeyReleased(Keys.Enter))
            {
                switch (_menuIndex)
                {
                    case 0:
                        this.ScreenManager.AddScreen(new TitleScreen());
                        break;
                    case 1:
                        this.ScreenManager.AddScreen(new TitleScreen());
                        break;
                }
                
                this.ExitScreen();
            }
            else if (this.InputManager.Keyboard.IsKeyReleased(Keys.Escape))
            {
                this.ExitScreen();
            }
        }

        /// <summary>
        /// Draws frame
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            if (this.ScreenState != Services.ScreenState.Active)
                return;

            base.Draw(gameTime);

            this.ScreenManager.SpriteBatch.Begin();
            _background.Draw(gameTime);
            this.ScreenManager.SpriteBatch.End();
        }
    }
}
