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
    public class TitleScreen : MenuScreen
    {
        protected Sprite _background;
        protected Button _playButton, _helpButton, _quitButton;
        protected Int32 _menuIndex;

        public TitleScreen(Game game) : base(game) { }

        /// <summary>
        /// Initializes the screen
        /// </summary>
        public override void Initialize()
        {
            // Add background
            _background = new Sprite(this.Game, "Graphics/Backgrounds/spr_title");
            this.Background.Add(_background);

            // Add buttons
            _playButton = new Button(this.Game, this.InputManager, "Graphics/Sprites/spr_button_play");
            _helpButton = new Button(this.Game, this.InputManager, "Graphics/Sprites/spr_button_help");
            _quitButton = new Button(this.Game, this.InputManager, "Graphics/Sprites/spr_button_quit");
            this.Foreground.Add(_playButton);
            this.Foreground.Add(_helpButton);
            this.Foreground.Add(_quitButton);

            // Initialize sprites and load their textures
            base.Initialize();

            // Set the positions
            _background.Position = Vector2.UnitY * 0/*-80*/ + Vector2.UnitX * (this.ScreenManager.ScreenWidth - _background.Texture.Width) / 2;
            _playButton.Position = new Vector2((this.ScreenManager.ScreenWidth - _playButton.Texture.Width) / 2, 500/*440*/);
            _helpButton.Position = new Vector2((this.ScreenManager.ScreenWidth - _helpButton.Texture.Width) / 2, 560/*500*/);
            _quitButton.Position = new Vector2((this.ScreenManager.ScreenWidth - _quitButton.Texture.Width) / 2, 620/*560*/);

            // Set on click actions
            _playButton.OnClicked += new ButtonClickDelegate(_playButton_OnClicked);
            _helpButton.OnClicked += new ButtonClickDelegate(_helpButton_OnClicked);
            _quitButton.OnClicked += new ButtonClickDelegate(_quitButton_OnClicked);
        }

        /// <summary>
        /// Is called when the quit button is clicked
        /// </summary>
        /// <param name="button">Reference to the clicked button</param>
        /// <param name="relativePosition">Position on the button clicked</param>
        protected void _quitButton_OnClicked(Button button, Vector2 relativePosition)
        {
            this.ExitScreen();
        }

        /// <summary>
        /// Is called when the help button is clicked
        /// </summary>
        /// <param name="button">Reference to the clicked button</param>
        /// <param name="relativePosition">Position on the button clicked</param>
        protected void _helpButton_OnClicked(Button button, Vector2 relativePosition)
        {
            this.ScreenManager.AddScreen(new HelpScreen(this.Game));
            this.ExitScreen();
        }

        /// <summary>
        /// Is called when the play button is clicked
        /// </summary>
        /// <param name="button">Reference to the clicked button</param>
        /// <param name="relativePosition">Position on the button clicked</param>
        protected void _playButton_OnClicked(Button button, Vector2 relativePosition)
        {
            this.ScreenManager.AddScreen(new LevelSelectScreen(this.Game));
            this.ExitScreen();
        }

        /// <summary>
        /// Processes input
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public override void HandleInput(GameTime gameTime)
        {
            base.HandleInput(gameTime);

            // Keyboard (simulate clicking them)
            if (this.InputManager.Keyboard.IsKeyReleased(Keys.Enter))
            {
                switch (_menuIndex)
                {
                    case 0:
                        _playButton_OnClicked(_playButton, Vector2.Zero);
                        break;
                    case 1:
                        _helpButton_OnClicked(_helpButton, Vector2.Zero);
                        break;
                    case 2:
                        _quitButton_OnClicked(_quitButton, Vector2.Zero);
                        break;
                }
            }
            else if (this.InputManager.Keyboard.IsKeyReleased(Keys.Escape))
            {
                // Exit game
                this.ExitScreen();
            }
        }
    }
}
