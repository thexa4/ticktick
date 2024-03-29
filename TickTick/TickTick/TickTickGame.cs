using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TickTick.Screens;
using TickTick.Services;
using Microsoft.Xna.Framework.Input;

namespace TickTick
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TickTickGame : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Graphics hook
        /// </summary>
        internal GraphicsDeviceManager Graphics
        {
            get;
            private set;
        }

        /// <summary>
        /// SpriteBatch hook
        /// </summary>
        internal SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        /// <summary>
        /// ScreenManager hook
        /// </summary>
        internal ScreenManager ScreenManager
        {
            get;
            private set;
        }

        /// <summary>
        /// InputManager hook
        /// </summary>
        internal InputManager InputManager
        {
            get;
            private set;
        }

        /// <summary>
        /// AudioManager hook
        /// </summary>
        internal AudioManager AudioManager
        {
            get;
            private set;
        }

        /// <summary>
        /// ColiisionManager hook
        /// </summary>
        internal CollisionManager CollisionManager
        {
            get;
            private set;
        }

        #region FRAMERATE
        private TimeSpan _elapsedTime;
        private Int32 _frameCount, _frameRate;
        #endregion

        /// <summary>
        /// Main Game Constructor
        /// </summary>
        public TickTickGame()
        {
            // Set Graphics profile
            this.Graphics = new GraphicsDeviceManager(this);

            this.Graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            this.Graphics.PreferredBackBufferWidth = 1440;//1280;
            this.Graphics.PreferredBackBufferHeight = 825;//720;
            this.Graphics.SupportedOrientations = DisplayOrientation.Default;
            this.IsMouseVisible = true;

#if DEBUG
            // Unlimted FPS
            this.IsFixedTimeStep = false;
            this.Graphics.SynchronizeWithVerticalRetrace = false;

            // Windowed
            this.Graphics.IsFullScreen = false;
#else
            // Capped FPS
            this.IsFixedTimeStep = true;
            this.Graphics.SynchronizeWithVerticalRetrace = true;        // screen bound
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1 / 60f);   // << 1 / number of frames per second

            // Fullscreen
            this.Graphics.IsFullScreen = false;
#endif

            // Apply Graphics
            this.Graphics.ApplyChanges();
            this.Content.RootDirectory = "Content";

            // Set components
            this.InputManager = new InputManager(this);
            this.ScreenManager = new ScreenManager(this);
            this.AudioManager = new AudioManager(this);
            this.CollisionManager = new CollisionManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.Components.Add(this.InputManager);
            this.Components.Add(this.ScreenManager);
            this.Components.Add(this.AudioManager);
            this.Components.Add(this.CollisionManager);

            // Initialize all components
            base.Initialize();

            this.ScreenManager.AddScreen(new TitleScreen(this));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Components loading
            base.LoadContent();

            // Loads font
            this.ScreenManager.SpriteFonts.LoadFont("Framerate", "Fonts/Default");
            
            // Loads audio
            this.AudioManager.Load("snd_music", "bgm");
            this.AudioManager.Loop("bgm");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.AudioManager.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // FRAMERATE AREA
            // This area is reserved to count the current frame rate.
            // Each second the framerate is updated (any more is not needed)

            // Add to ElapsedTime
            _elapsedTime += gameTime.ElapsedGameTime;
            // If More then one second passed
            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                // Set the FrameRate
                _frameRate = _frameCount;
                // Reset the FrameCount
                _frameCount = 0;
                // Remove this second
                _elapsedTime -= TimeSpan.FromSeconds(1);
            }

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            // FRAMERATE AREA
            // This area is reserved to count the current frame rate.
            // Each second the framerate is updated (more is not needed)
            _frameCount++;

            var framerateString = String.Format("Framerate: {0} f/s\n", _frameRate);

            this.SpriteBatch.Begin();
            this.SpriteBatch.DrawString(this.ScreenManager.SpriteFonts["Framerate"], framerateString,
                Vector2.One * 6 + Vector2.UnitX * (this.Graphics.PreferredBackBufferWidth - 10 - this.ScreenManager.SpriteFonts["Framerate"].MeasureString(framerateString).X), Color.Black);
            this.SpriteBatch.DrawString(this.ScreenManager.SpriteFonts["Framerate"], framerateString,
                Vector2.One * 5 + Vector2.UnitX * (this.Graphics.PreferredBackBufferWidth - 10 - this.ScreenManager.SpriteFonts["Framerate"].MeasureString(framerateString).X), Color.White);
            this.SpriteBatch.End();
        }
    }
}
