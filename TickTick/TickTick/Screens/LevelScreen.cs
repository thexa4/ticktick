using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickTick.Data;
using Microsoft.Xna.Framework;
using TickTick.Services;
using TickTick.Drawing;
using TickTick.Drawing.Actors;
using Microsoft.Xna.Framework.Input;

namespace TickTick.Screens
{
    public class LevelScreen : GameScreen
    {
        protected Level _levelData;
        protected LevelState _levelState;
        protected Layer _background, _foreground, _overlay;
        protected PlayerController _playerController;
        protected KeyboardController _keyboardController;
        protected Player _player;

        /// <summary>
        /// Creates a new level screen
        /// </summary>
        /// <param name="game">Game to bind to</param>
        /// <param name="data">Level data</param>
        public LevelScreen(Game game, Level data)
            : base(game)
        {
            _levelData = data;
            _background = new Layer(game);
            _foreground = new Layer(game);
            _overlay = new Layer(game);

            this.Exited += new EventHandler(LevelScreen_Exited);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LevelScreen_Exited(object sender, EventArgs e)
        {
            ((CollisionManager)this.Game.Services.GetService(typeof(CollisionManager))).Clear();
        }

        /// <summary>
        /// Initializes the screen
        /// </summary>
        public override void Initialize()
        {
            _keyboardController = new KeyboardController(this.Game, Keys.W, Keys.A, Keys.D);
            _keyboardController.Initialize();

            /////////////////////////////////////
            // Build the layers
            /////////////////////////////////////

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

            // Get the tiles
            var tiles = _levelData.GenerateComponents(this.Game, _foreground, out _levelState);
            foreach (var tile in tiles)
            {
                _foreground.Add(tile);
                if (tile.GetType() == typeof(Player))
                {
                    _player = tile as Player;
                    _playerController = new PlayerController(Game, _keyboardController, _player);
                    _playerController.Initialize();
                }
            }

            // Add the time
            _overlay.Add(new Sprite(this.Game, "Graphics/Sprites/spr_timer") { Position = Vector2.One * 10 });
            _overlay.Add(new TimerDisplay(this.Game, _levelState) { Position = new Vector2(25, 30) });

            // Add the quit button
            var quitButton = new Button(this.Game, this.InputManager, "Graphics/Sprites/spr_button_quit");
            _overlay.Add(quitButton);

            /////////////////////////////////////
            // Initialize the screen components
            /////////////////////////////////////

            base.Initialize();
            _background.Initialize();
            _foreground.Initialize();
            _overlay.Initialize();

            _backgroundSky.Position = Vector2.UnitY * (this.ScreenManager.ScreenHeight - _backgroundSky.Texture.Height);
            foreach (var mountain in _mountains)
                mountain.Position = Vector2.UnitX * (Single)(this.ScreenManager.Random.NextDouble() * this.ScreenManager.ScreenWidth - mountain.Texture.Width / 2) +
                    Vector2.UnitY * (this.ScreenManager.ScreenHeight - mountain.Texture.Height);

            quitButton.Position = new Vector2(this.ScreenManager.ScreenWidth - quitButton.Texture.Width - 10, 10);

            /////////////////////////////////////
            // Action handlers
            /////////////////////////////////////

            quitButton.OnClicked += new ButtonClickDelegate(quitButton_OnClicked);
            _levelState.OnCompleted += new EventHandler(_levelState_OnCompleted);
            _levelState.OnLost += new EventHandler(_levelState_OnLost);

            System.Diagnostics.Debug.WriteLine("Called init");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _levelState_OnLost(object sender, EventArgs e)
        {
            _player.Explode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _levelState_OnCompleted(object sender, EventArgs e)
        {
            _player.Celebrate();
        }

        /// <summary>
        /// On quit button clicked
        /// </summary>
        /// <param name="button"></param>
        /// <param name="relativePosition"></param>
        protected void quitButton_OnClicked(Button button, Vector2 relativePosition)
        {
            this.ExitScreen();
            this.ScreenManager.AddScreen(new LevelSelectScreen(this.Game));

            System.Diagnostics.Debug.WriteLine("Call exit");
        }

        /// <summary>
        /// Updates the level
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing Values</param>
        /// <param name="otherScreenHasFocus">Other Screen has Focus flag (Game.Active == false)</param>
        /// <param name="coveredByOtherScreen">Other GameScreen has been drawn</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            _background.Update(gameTime);
            _foreground.Update(gameTime);
            _overlay.Update(gameTime);
            
            _levelState.Update(gameTime);
            _keyboardController.Update(gameTime);
        }

        /// <summary>
        /// Draws the level
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing Values</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _background.Draw(gameTime);
            _foreground.Draw(gameTime);
            _overlay.Draw(gameTime);
        }

        /// <summary>
        /// Handles the input
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing Values</param>
        public override void HandleInput(GameTime gameTime)
        {
            base.HandleInput(gameTime);
            _playerController.UpdateInput(gameTime);
        }
    }
}
