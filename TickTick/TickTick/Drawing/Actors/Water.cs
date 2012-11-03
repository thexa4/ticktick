using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickTick.Data;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing.Actors
{
    class Water : Tile
    {
        public LevelState State { get; protected set; }
        public bool IsCollected { get; protected set; }

        public Water(Game game, Layer layer, string assetname, LevelState state) : base(game, layer, assetname)
        {
            State = state;
            State.WaterDropsLeft++;
        }

        public override void StartTouch(ICollidable collider)
        {
            if (IsCollected)
                return;

            if (collider.GetType() != typeof(Player))
                return;

            State.WaterDropsLeft--;

            IsCollected = true;
            Visible = false;
            Enabled = false;
        }
    }
}
