using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TickTick.Services;
using Microsoft.Xna.Framework.Content;

namespace TickTick.Drawing.Actors
{
    class Sprite : DrawableGameComponent
    {
        /// <summary>
        /// The position of the sprite
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The asset name of this sprite
        /// </summary>
        public string AssetName { get; protected set; }
        /// <summary>
        /// The texture of this sprite
        /// </summary>
        public Texture2D Texture { get; protected set; }
        /// <summary>
        /// The amount of rows in the spritesheet
        /// </summary>
        public int Rows { get; protected set; }
        /// <summary>
        /// The amount of columns in the spritesheet
        /// </summary>
        public int Columns { get; protected set; }
        /// <summary>
        /// The frame that's currently being displayed
        /// </summary>
        public int Frame { get; set; }
        /// <summary>
        /// The number of seconds per frame
        /// </summary>
        public float AnimationSpeed { get; set; }
        /// <summary>
        /// The sprite effect to be added (mirroring)
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// The amount of time left till the next frame
        /// </summary>
        protected float _timeLeft;

        /// <summary>
        /// Creates a new sprite with default settings
        /// </summary>
        /// <param name="game">The game the sprite belongs to</param>
        /// <param name="assetname">The texture to display</param>
        public Sprite(Game game, string assetname)
            : base(game)
        {
            AnimationSpeed = 0.125f;
            SpriteEffect = SpriteEffects.None;
            AssetName = assetname;

            Rows = 1;
            Columns = 1;

            // Try guessing spritesheet width from assetname
            string[] parameters = assetname.Split('@');
            if (parameters.Length > 1)
            {
                string[] arguments = parameters[1].Split('x');
                Columns = Int32.Parse(arguments[0]);
                if (arguments.Length > 1)
                    Rows = Int32.Parse(arguments[1]);
            }
        }

        /// <summary>
        /// Loads the screenmanager and calls LoadContent(ContentManager)
        /// </summary>
        protected override void LoadContent()
        {
            var screenmanager = (ScreenManager)this.Game.Services.GetService(typeof(ScreenManager));
            LoadContent(screenmanager.ContentManager);
        }

        /// <summary>
        /// Enables a sprite to load data
        /// </summary>
        /// <param name="contentManager"></param>
        public void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>(AssetName);
        }

        public override void Update(GameTime gameTime)
        {
            _timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            int frames = (int)(_timeLeft / -AnimationSpeed);
            if (frames > 0)
            {
                Frame += frames;
                Frame = Frame % (Rows * Columns);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            
        }
    }
}
