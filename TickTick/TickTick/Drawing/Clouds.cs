﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing
{
    public class Clouds : DrawableGameComponent
    {
        protected Sprite[] _cloudSprites;
        protected Dictionary<Sprite, Single> _velocities;
        protected Int32 visible;

        static Random Random = new Random();
        
        /// <summary>
        /// Creates new Clouds SpriteSet
        /// </summary>
        /// <param name="game">Game to bind to</param>
        /// <param name="n">Number of clouds</param>
        public Clouds(Game game, Int32 n) : base(game)
        {
            _velocities = new Dictionary<Sprite, float>();
            _cloudSprites = new Sprite[5];
            for (Int32 i = 0; i < _cloudSprites.Length; i++)
            {
                _cloudSprites[i] = new Sprite(game, String.Format("Graphics/Backgrounds/spr_cloud_{0}", i + 1));
                _cloudSprites[i].Enabled = false;
                _cloudSprites[i].Visible = false;
                _velocities.Add(_cloudSprites[i], 0);
            }

            // We only want different sprites to be visible. We have this number of 
            // sprites (length) and we want that number of sprites (n). So get the
            // minimum and all will be fine.
            visible = Math.Min(n, _cloudSprites.Length);
        }

        /// <summary>
        /// Initializes clouds
        /// </summary>
        public override void Initialize()
        {
            foreach (var cloud in _cloudSprites)
                cloud.Initialize();

            base.Initialize();

            for (Int32 i = 0; i < visible; i++)
                PickAndPlace(true);
        }

        /// <summary>
        /// Picks and places a invisible sprites and starts showing it
        /// </summary>
        /// <param name="start">If start is true, can spawn on screen</param>
        public void PickAndPlace(Boolean start = false)
        {
            var candidates = _cloudSprites.Where(sprite => !sprite.Visible).ToArray();
            var picked = candidates[Random.Next(0, candidates.Length)];

            picked.Visible = true;
            picked.Enabled = true;
            picked.Position = Vector2.UnitX * (Random.Next(start ? 0 : picked.ScreenManager.ScreenWidth, picked.ScreenManager.ScreenWidth * 2)) +
                Vector2.UnitY * Random.Next(40, picked.ScreenManager.ScreenHeight / 3 * 2);
            _velocities[picked] = -Random.Next(30, 30 * 8);
        }

        /// <summary>
        /// Update frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var cloud in _cloudSprites)
                if (cloud.Enabled)
                {
                    cloud.Update(gameTime);
                    cloud.Position += _velocities[cloud] * (Single)gameTime.ElapsedGameTime.TotalSeconds * Vector2.UnitX;
                    if (cloud.Position.X < -cloud.Size.X)
                    {
                        cloud.Visible = false;
                        cloud.Enabled = false;
                        PickAndPlace();
                    }
                }
        }

        /// <summary>
        /// Draws frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (var cloud in _cloudSprites)
                if (cloud.Visible)
                    cloud.Draw(gameTime);
        }
    }
}
