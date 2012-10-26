using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing
{
    public class Clouds : DrawableGameComponent
    {
        private int p;

        public Clouds(Game game, int p) : base(game)
        {

            this.p = p;
        }
    }
}
