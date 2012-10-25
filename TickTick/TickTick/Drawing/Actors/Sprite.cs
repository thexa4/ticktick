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
        /// The layer the sprite is on
        /// </summary>
        public Layer Layer { get; protected set; }
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
        public Sprite(Game game, Layer layer, string assetname)
            : base(game)
        {
            AnimationSpeed = 0.125f;
            SpriteEffect = SpriteEffects.None;
            AssetName = assetname;
            Layer = layer;

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

        /// <summary>
        /// Updates the current frame
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
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
            var screenmanager = (ScreenManager)this.Game.Services.GetService(typeof(ScreenManager));
            DrawSprite(gameTime, screenmanager.SpriteBatch);
        }

        /// <summary>
        /// Enables you to draw a sprite using the current spritebatch
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
        /// <param name="spriteBatch">The current spritebatch</param>
        public void DrawSprite(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int framewidth = Texture.Width / Columns;
            int frameheight = Texture.Height / Rows;

            int curx = Frame % Columns;
            int cury = Frame / Rows;

            spriteBatch.Draw(Texture, Position, new Rectangle(curx * framewidth, cury * frameheight, framewidth, frameheight), Color.White, 0, Vector2.Zero, 1, SpriteEffect, 0);
        }
    }
}
