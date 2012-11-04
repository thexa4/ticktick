using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickTick.Data;
using Microsoft.Xna.Framework;
using TickTick.Screens;

namespace TickTick.Drawing.Actors
{
    class Finish : Tile
    {
        public LevelState State { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="layer"></param>
        /// <param name="assetname"></param>
        /// <param name="state"></param>
        public Finish(Game game, Layer layer, string assetname, LevelState state)
            : base(game, layer, assetname)
        {
            State = state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collider"></param>
        public override void StartTouch(ICollidable collider)
        {
            if (collider.GetType() != typeof(Player))
                return;

            if (State.WaterDropsLeft == 0)
                State.End();
        }
    }
}
