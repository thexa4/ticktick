using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing.Actors
{
    public class Player : WalkingActor
    {
        public Player(Game game, Layer layer)
            : base(game, layer, "Graphics/Sprites/Player/spr_idle")
        {
        }
    }
}
