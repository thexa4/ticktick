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

        //
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
        }
    }
}
