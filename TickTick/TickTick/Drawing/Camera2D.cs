using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;

namespace TickTick.Drawing
{
    public class Camera2D : GameComponent
    {
        public Vector2 Position { get; protected set; }
        public IFocusable Focus { get; protected set; }

        public Single Rotation;
        public Single Zoom;
        public Vector2 ViewPort; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public Camera2D(Game game) : base (game)
        {
            Rotation = 0;
            Zoom = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            var screenManager = (ScreenManager)this.Game.Services.GetService(typeof(ScreenManager));
            ViewPort = Vector2.UnitX * screenManager.ScreenWidth + Vector2.UnitY * screenManager.ScreenHeight;
            Focus = Focus ?? new StaticFocus(ViewPort / 2);

            base.Initialize();
        }

        /// <summary>
        /// Focusses on this object
        /// </summary>
        /// <param name="focusable"></param>
        public void FocusOn(IFocusable focusable)
        {
            this.Focus = focusable;
        }

        /// <summary>
        /// Gets the transformation matrix for the current camera
        /// </summary>
        /// <returns></returns>
        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(Vector3.UnitX * -Position.X + Vector3.UnitY * -Position.Y) * // move focus to origin
                Matrix.CreateRotationZ(Rotation) * // Rotate
                Matrix.CreateScale((Vector3.UnitX + Vector3.UnitY) * Zoom + Vector3.UnitZ) * // Zoom
                Matrix.CreateTranslation((Vector3.UnitX * ViewPort.X + Vector3.UnitY * ViewPort.Y) * 0.5f + Vector3.UnitZ); // move origin to center screen;
        }

        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var target = Focus.Position + Focus.Size / 2;
            if (target != this.Position)
                this.Position = Vector2.Lerp(this.Position, target, (Single)gameTime.ElapsedGameTime.TotalSeconds * 1.5f);
        }

        /// <summary>
        /// 
        /// </summary>
        public class StaticFocus : IFocusable
        {
            public Vector2 Size { get { return Vector2.Zero; } }
            public Vector2 Position { get; protected set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Position"></param>
            public StaticFocus(Vector2 position)
            {
                Position = position;
            }
        }
    }
}
