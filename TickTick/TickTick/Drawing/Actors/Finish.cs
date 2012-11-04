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

        public Finish(Game game, Layer layer, string assetname, LevelState state)
            : base(game, layer, assetname)
        {
            State = state;
        }

        public override void StartTouch(ICollidable collider)
        {
            if (collider.GetType() != typeof(Player))
                return;

            if (State.WaterDropsLeft == 0)
                ScreenManager.ExitAll();
        }
    }
}
