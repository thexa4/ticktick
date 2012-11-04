using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TickTick.Drawing.Actors
{
    public class Player : WalkingActor
    {
        Dictionary<string, Texture2D> _textures { get; set; }
        Dictionary<string, string> _assets { get; set; }

        /// <summary>
        /// Collidedwith 
        /// </summary>
        public bool IsDead { get; set; }
        public bool IsFinished { get; set; }
        public bool IsExploded { get; set; }

        protected int TouchingIceBlocks { get; set; }
        protected int TouchingHotBlocks { get; set; }

        /// <summary>
        /// Currently on an ice block
        /// </summary>
        public bool IsOnIce { get { return TouchingIceBlocks > 0; } }

        /// <summary>
        /// Currently on a hot block
        /// </summary>
        public bool IsOnHot { get { return TouchingHotBlocks > 0; } }

        /// <summary>
        /// Creates a player
        /// </summary>
        /// <param name="game">Game to bind to</param>
        /// <param name="layer">Layer to draw to</param>
        public Player(Game game, Layer layer)
            : base(game, layer, "Graphics/Sprites/Player/spr_idle")
        {
            _textures = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing Values</param>
        public override void Update(GameTime gameTime)
        {
            if (!IsFinished && !IsDead)
            {
                if (IsDead)
                    return;

                if (!IsFalling)
                {
                    if (Velocity.X == 0)
                        SetTexture("idle");
                    else
                        SetTexture("run");
                }
                else if (Velocity.Y < 0)
                    SetTexture("jump");
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Explode
        /// </summary>
        public void Explode()
        {
            if (IsDead || IsFinished)
                return;
            IsDead = true;
            IsExploded = true;
            Velocity = Vector2.Zero;
            Position += Vector2.UnitY * 15;
            SetTexture("explode");
        }

        /// <summary>
        /// Die
        /// </summary>
        public void Die()
        {
            if (IsDead || IsFinished)
                return;
            IsDead = true;
            Velocity *= Vector2.UnitY;
            //TODO: Add
            SetTexture("die");
        }

        /// <summary>
        /// Celebrate
        /// </summary>
        public void Celebrate()
        {
            if (IsDead || IsFinished)
                return;
            IsFinished = true;
            Velocity *= Vector2.UnitY;
            Velocity += Vector2.UnitY * 23;
            SetTexture("celebrate");
        }

        /// <summary>
        /// Start touch handling
        /// </summary>
        /// <param name="collider"></param>
        public override void StartTouch(ICollidable collider)
        {
            base.StartTouch(collider);

            var tile = collider as Tile;
            if (tile == null)
                return;

            if (tile.IsHot)
                TouchingHotBlocks++;

            if (tile.IsIce)
                TouchingIceBlocks++;
        }

        /// <summary>
        /// End touch handling
        /// </summary>
        /// <param name="collider"></param>
        public override void EndTouch(ICollidable collider)
        {
            base.EndTouch(collider);

            var tile = collider as Tile;
            if (tile == null)
                return;

            if (tile.IsHot)
                TouchingHotBlocks--;

            if (tile.IsIce)
                TouchingIceBlocks--;
        }
        
        /// <summary>
        /// Loads all content
        /// </summary>
        /// <param name="contentManager"></param>
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            _assets = new Dictionary<string, string> {
                {"celebrate", "Graphics/Sprites/Player/spr_celebrate@14"},
                {"die", "Graphics/Sprites/Player/spr_die@5"},
                {"explode", "Graphics/Sprites/Player/spr_explode@5x5"},
                {"idle", "Graphics/Sprites/Player/spr_idle"},
                {"jump", "Graphics/Sprites/Player/spr_jump@14"},
                {"run", "Graphics/Sprites/Player/spr_run@13"},
            };

            foreach (var asset in _assets.Keys)
                _textures.Add(asset, contentManager.Load<Texture2D>(_assets[asset]));

            base.LoadContent(contentManager);
        }

        /// <summary>
        /// Sets the new texture
        /// </summary>
        /// <param name="asset">Asset name</param>
        public void SetTexture(String asset)
        {
            if (Texture == _textures[asset])
                return;
            Texture = _textures[asset];
            SetSprite(_assets[asset]);
            this.Size = new Vector2(Texture.Width / Columns, Texture.Height / Rows) * Scale;
        }
    }
}
