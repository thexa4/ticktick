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
    public class LevelSprite : Sprite
    {
        /// <summary>
        /// The layer the sprite is on
        /// </summary>
        public Layer Layer { get; protected set; }
      
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
        public int Frame
        {
            get { return _frame; }

            set
            {
                _frame = value % (Rows * Columns);

                if (this.Texture == null)
                    return;

                int framewidth = Texture.Width / Columns;
                int frameheight = Texture.Height / Rows;

                int curx = Frame % Columns;
                int cury = Frame / Columns;

                this.SourceRectangle = new Rectangle(curx * framewidth, cury * frameheight, framewidth, frameheight);
            }
        }
        private Int32 _frame;

        /// <summary>
        /// The number of seconds per frame
        /// </summary>
        public float AnimationSpeed { get; set; }

        /// <summary>
        /// The amount of time left till the next frame
        /// </summary>
        protected float _timeLeft;

        /// <summary>
        /// Creates a new sprite with default settings
        /// </summary>
        /// <param name="game">The game the sprite belongs to</param>
        /// <param name="assetname">The texture to display</param>
        public LevelSprite(Game game, Layer layer, string assetname)
            : base(game, assetname)
        {
            this.AnimationSpeed = 1f / 24f;
            this.Layer = layer;

            this.Rows = 1;
            this.Columns = 1;

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
        /// 
        /// </summary>
        /// <param name="contentManager"></param>
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            this.Frame = this.Frame; // calls set function of property;
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
                this.Frame += frames;
                this.Frame = Frame % (Rows * Columns);
                _timeLeft = AnimationSpeed;
            }


        }
    }
}
