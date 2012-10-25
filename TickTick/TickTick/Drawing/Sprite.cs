using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TickTick.Services;
using Microsoft.Xna.Framework.Content;

namespace TickTick.Drawing
{
    public class Sprite : DrawableGameComponent
    {
        /// <summary>
        /// The position of the sprite
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The position origin of the sprite
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The Color (spritebatch tint) of the sprite
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The asset name of this sprite
        /// </summary>
        public String AssetName { get; protected set; }

        /// <summary>
        /// The texture of this sprite
        /// </summary>
        public Texture2D Texture { get; protected set; }

        /// <summary>
        /// The sprite effect to be added (mirroring)
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// The sprites drawing scale
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// The source rectangle
        /// </summary>
        public Rectangle SourceRectangle { get; set; }

        /// <summary>
        /// The screenmanager
        /// </summary>
        public ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// The contentManager
        /// </summary>
        public ContentManager ContentManager { get; set; }

        /// <summary>
        /// Creates a new sprite with default settings
        /// </summary>
        /// <param name="game">The game the sprite belongs to</param>
        /// <param name="assetname">The texture to display</param>
        public Sprite(Game game, String assetname)
            : base(game)
        {
            this.SpriteEffect = SpriteEffects.None;
            this.Scale = Vector2.One;
            this.Color = Color.White;
            this.Origin = Vector2.Zero;
            this.AssetName = assetname;
        }

        public override void Initialize()
        {
            this.ScreenManager = (ScreenManager)this.Game.Services.GetService(typeof(ScreenManager));
            base.Initialize();
        }

        /// <summary>
        /// Loads the screenmanager and calls LoadContent(ContentManager)
        /// </summary>
        protected override void LoadContent()
        {
            LoadContent(this.ScreenManager.ContentManager);
        }

        /// <summary>
        /// Enables a sprite to load data
        /// </summary>
        /// <param name="contentManager"></param>
        public virtual void LoadContent(ContentManager contentManager)
        {
            this.ContentManager = contentManager;
            this.Texture = this.ContentManager.Load<Texture2D>(this.AssetName);

            if (this.SourceRectangle == Rectangle.Empty)
                this.SourceRectangle = new Rectangle(0, 0, this.Texture.Width, this.Texture.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!this.Visible)
                return;

            DrawSprite(gameTime, this.ScreenManager.SpriteBatch);
        }

        /// <summary>
        /// Enables you to draw a sprite using the current spritebatch
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
        /// <param name="spriteBatch">The current spritebatch</param>
        public virtual void DrawSprite(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, this.SourceRectangle, this.Color, 0, 
                this.Origin, this.Scale, this.SpriteEffect, 0);
        }
    }
}
