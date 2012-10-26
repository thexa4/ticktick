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
    public class LevelSelectScreen : MenuScreen
    {
        protected Sprite _background;
        protected Button _backButton;
        protected Button[] _levelButtons;
        protected Int32 _menuIndex;

        protected const Int32 Levels = 10;

        public LevelSelectScreen(Game game) : base(game) { }

        /// <summary>
        /// Initialiazes the screen
        /// </summary>
        public override void Initialize()
        {
            _background = new Sprite(this.Game, "Graphics/Backgrounds/spr_levelselect");
            this.Background.Add(_background);

            // add the level buttons
            _levelButtons = new Button[Levels];
            for (Int32 i = 0; i < Levels; i++)
            {
                var assetName = String.Format("Graphics/Sprites/spr_level_{0}", "unsolved");
                _levelButtons[i] = new LevelButton(this.Game, this.InputManager, assetName);
                _levelButtons[i].Enabled = true;
                this.Foreground.Add(_levelButtons[i]);
            }

            _backButton = new Button(this.Game, this.InputManager, "Graphics/Sprites/spr_button_back");
            this.Foreground.Add(_backButton);

            // Initialize and load all the sprites
            base.Initialize();

            // Set positions and actions for the levelbuttons
            for (Int32 i = 0; i < Levels; i++)
            {
                int row = i / 4;
                int column = i % 4;

                _levelButtons[i].Position = new Vector2(column * (_levelButtons[i].Texture.Width + 20),
                    row * (_levelButtons[i].Texture.Height + 20)) + new Vector2(310, 120);
                _levelButtons[i].OnClicked += new ButtonClickDelegate(LevelSelectScreen_OnClicked);
            }

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
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="relativePosition"></param>
        protected void  LevelSelectScreen_OnClicked(Button button, Vector2 relativePosition)
        {
 	        
        }

    }
}
