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
    public class HelpScreen : MenuScreen
    {
        protected Sprite _background;
        protected Button _backButton;
        protected Int32 _menuIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public HelpScreen(Game game) : base(game) { }

        /// <summary>
        /// Initializes the screen
        /// </summary>
        public override void Initialize()
        {
            // Create sprites
            _background = new Sprite(this.Game, "Graphics/Backgrounds/spr_help");
            this.Background.Add(_background);

            _backButton = new Button(this.Game, this.InputManager, "Graphics/Sprites/spr_button_back");
            this.Foreground.Add(_backButton);

            // Initalize sprites and load their textures
            base.Initialize();

            // Set the positions
            _background.Position = Vector2.UnitY * -50 + Vector2.UnitX * (this.ScreenManager.ScreenWidth - _background.Texture.Width) / 2;
            _backButton.Position = new Vector2((this.ScreenManager.ScreenWidth - _backButton.Texture.Width) / 2, 650);

            _backButton.OnClicked += new ButtonClickDelegate(_backButton_OnClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="relativePosition"></param>
        protected void _backButton_OnClicked(Button button, Vector2 relativePosition)
        {
            this.ScreenManager.AddScreen(new TitleScreen(this.Game));
            this.ExitScreen();
        }

        /// <summary>
        /// Processes input
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public override void HandleInput(GameTime gameTime)
        {
            base.HandleInput(gameTime);

            if (this.InputManager.Keyboard.IsKeyReleased(Keys.Escape))
                _backButton_OnClicked(_backButton, Vector2.Zero);

        }
    }
}
