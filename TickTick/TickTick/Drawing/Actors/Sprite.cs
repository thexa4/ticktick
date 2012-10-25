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
        public string AssetName { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }
        public int Frame { get; set; }
        public float Speed { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        protected float _timeLeft;

        public Sprite(Game game, string assetname)
            : base(game)
        {
            Speed = 0.125f;
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
