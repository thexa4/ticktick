using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TickTick.Drawing
{
    public class TimerDisplay : DrawableGameComponent
    {
        protected Data.LevelState _levelState;

        /// <summary>
        /// 
        /// </summary>
        protected ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// Position of the display
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="p"></param>
        /// <param name="_levelState"></param>
        public TimerDisplay(Game game, Data.LevelState levelState)
            : base(game)
        {
            _levelState = levelState;
        }

        /// <summary>
        /// 
        /// </summary>
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
            this.ScreenManager.SpriteFonts.LoadFont("Hud", "Fonts/Hud");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var timeleft = Math.Round(_levelState.TimeLeft);
            this.ScreenManager.SpriteBatch.DrawString(this.ScreenManager.SpriteFonts["Hud"], 
                String.Format("{0}:{1}", (Int32)(timeleft / 60), timeleft % 60), Position, Color.White);
        }
    }
}
