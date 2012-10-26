using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickTick.Data;
using Microsoft.Xna.Framework;
using TickTick.Services;
using TickTick.Drawing;

namespace TickTick.Screens
{
    public class LevelScreen : GameScreen
    {
        protected Level _levelData;
        protected Layer _background, _foreground, _overlay;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="data"></param>
        public LevelScreen(Game game, Level data)
            : base(game)
        {
            _levelData = data;
            _background = new Layer(game);
            _foreground = new Layer(game);
            _overlay = new Layer(game);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            // Add the sky
            var _backgroundSky = new Sprite(this.Game, "Graphics/Backgrounds/spr_sky");
            _background.Add(_backgroundSky);

            // Add a few random mountains
            var _mountains = new Sprite[5];
            for (int i = 0; i < 5; i++)
            {
                _mountains[i] = new Sprite(this.Game, String.Format("Graphics/Backgrounds/spr_mountain_{0}", (this.ScreenManager.Random.Next(2) + 1)));
                _background.Add(_mountains[i]);
            }

            // Add the clouds
            _background.Add(new Clouds(this.Game, 2));

            // Add the time
            _overlay.Add(new Sprite(this.Game, "Graphics/Sprites/spr_timer") { Position = Vector2.One * 10 });
            _overlay.Add(new TimerDisplay(this.Game, "Graphics/Sprites/spr_timer")); //{ Position = new Vector2(25, 30) });

            // Add the quit button
            var quitButton = new Button(this.Game, this.InputManager, "Graphics/Sprites/spr_button_quit");
            _overlay.Add(quitButton);

            // Get the tiles
            var tiles = _levelData.GenerateComponents(this.Game, _foreground);
            foreach (var tile in tiles)
                _foreground.Add(tile);

            base.Initialize();
            _background.Initialize();
            _foreground.Initialize();
            _overlay.Initialize();


            _backgroundSky.Position = Vector2.UnitY * (this.ScreenManager.ScreenHeight - _backgroundSky.Texture.Height);
            foreach (var mountain in _mountains)
                mountain.Position = Vector2.UnitX * (Single)(this.ScreenManager.Random.NextDouble() * this.ScreenManager.ScreenWidth - mountain.Texture.Width / 2) +
                    Vector2.UnitY * (this.ScreenManager.ScreenHeight - mountain.Texture.Height);

            quitButton.Position = new Vector2(this.ScreenManager.ScreenWidth - quitButton.Texture.Width - 10, 10);
            quitButton.OnClicked += new ButtonClickDelegate(quitButton_OnClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="relativePosition"></param>
        protected void quitButton_OnClicked(Button button, Vector2 relativePosition)
        {
            this.ScreenManager.AddScreen(new LevelSelectScreen(this.Game));
            this.ExitScreen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            _background.Update(gameTime);
            _foreground.Update(gameTime);
            _overlay.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _background.Draw(gameTime);
            _foreground.Draw(gameTime);
            _overlay.Draw(gameTime);
        }
    }
}
