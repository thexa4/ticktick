using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TickTick.Drawing.Actors
{
    class Sprite : Actor
    {
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
            Rows = 1;
            Columns = 1;
            SpriteEffect = SpriteEffects.None;
            AssetName = assetname;

        }

        protected override void LoadContent()
        {
            
        }
    }
}
