using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Data
{
    public class LevelState 
    {
        /// <summary>
        /// 
        /// </summary>
        public Int32 WaterDropsLeft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Double TimeLeft { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double TimeSpeed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCompleted = delegate { }, OnLost = delegate { };

        /// <summary>
        /// 
        /// </summary>
        public LevelState()
        {
            TimeSpeed = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            TimeLeft -= TimeSpeed * gameTime.ElapsedGameTime.TotalSeconds;

            if (TimeLeft < 0)
                End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void End()
        {
            if (TimeLeft < 0)
                OnLost.Invoke(this, EventArgs.Empty);
            else if (WaterDropsLeft == 0)
                OnCompleted.Invoke(this, EventArgs.Empty);
        }
    }
}
