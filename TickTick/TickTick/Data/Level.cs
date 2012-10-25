using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TickTick.Data
{
    /// <summary>
    /// Represents a level
    /// </summary>
    class Level
    {
        /// <summary>
        /// The Width of the level
        /// </summary>
        public int Width { get; protected set; }
        /// <summary>
        /// The Height of the level
        /// </summary>
        public int Height { get; protected set; }

        protected char[,] _data;

        /// <summary>
        /// Returns the value at a certain position
        /// </summary>
        /// <param name="x">The x position to lookup</param>
        /// <param name="y">The y position to lookup</param>
        /// <returns></returns>
        public char this[int x, int y] {
            get { return _data[y, x]; }
            set { _data[y, x] = value; }
        }

        /// <summary>
        /// Creates a new level with a specified width and height
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public Level(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new char[height, width];
        }
    }
}
