﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing.Actors
{
    class Actor : DrawableGameComponent
    {
        public Vector2 Position { get; set; }

        public Actor(Game game)
            : base(game)
        {
            
        }
    }
}
