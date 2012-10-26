using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;
using TickTick.Data;

namespace TickTick.Drawing
{
    public class LevelButton : Button
    {
        public Level Level
        {
            get
            {
                return Level.ReadLevel(_levelAssetName);
            }
        }

        protected String _levelAssetName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="inputManager"></param>
        /// <param name="assetName"></param>
        /// <param name="levelAssetName"></param>
        public LevelButton(Game game, InputManager inputManager, string assetName, string levelAssetName) 
            : base(game, inputManager, assetName)
        {
            _levelAssetName = levelAssetName;   
        }

    }
}
