using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        /// <summary>
        /// The Description of the level
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The internal representation of the level file
        /// </summary>
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

        /// <summary>
        /// Reads a level from a file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>The level</returns>
        static Level ReadLevel(string path)
        {
            var f = new FileInfo(path);
            FileStream stream = f.OpenRead();
            StreamReader reader = new StreamReader(stream);

            int width = 20;
            int height = 15;

            var level = new Level(width, height);

            for (int y = 0; y < height; y++)
            {
                string line = reader.ReadLine();
                for (int x = 0; x < width; x++)
                    level[x, y] = line[x];
            }

            level.Description = reader.ReadLine();

            return level;
        }
    }
}
