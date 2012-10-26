using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;

namespace TickTick.Drawing
{
    public class LevelButton : Button
    {

        public LevelButton(Game game, InputManager inputManager, string assetName) : base(game, inputManager, assetName)
        {
        }

    }
}
