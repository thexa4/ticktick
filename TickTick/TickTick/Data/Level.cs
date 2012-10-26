﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using TickTick.Drawing.Actors;
using TickTick.Drawing;

namespace TickTick.Data
{
    /// <summary>
    /// Represents a level
    /// </summary>
    public class Level
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
        public static Level ReadLevel(string path)
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

        public List<DrawableGameComponent> GenerateComponents(Game game, Layer layer)
        {
            var l = new List<DrawableGameComponent>();

            for(int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    switch (this[x, y])
                    {
                        case '.':
                            // Background, draw nothing
                            break;
                        case '-':
                            // Normal platform
                            l.Add(new Tile(game, layer, "Graphics\\Sprites\\spr_platform")
                            {
                                IsSolid = true,
                                IsPlatform = true,
                            });
                            break;
                        case '+':
                            // Hot platform
                            l.Add(new Tile(game, layer, "Graphics\\Sprites\\spr_platform_hot")
                            {
                                IsSolid = true,
                                IsPlatform = true,
                                IsHot = true,
                            });
                            break;
                        case '@':
                            // Hot platform
                            l.Add(new Tile(game, layer, "Graphics\\Sprites\\spr_platform_ice")
                            {
                                IsSolid = true,
                                IsPlatform = true,
                                IsIce = true,
                            });
                            break;
                        case 'X':
                            // End tile
                            //TODO: Add endtile loader
                            break;
                        case 'W':
                            // Water tile
                            //TODO: Add watertile loader
                        case '1':
                            // Player tile
                            //TODO: Add player loader
                            break;
                        case '#':
                            // Wall tile
                            l.Add(new Tile(game, layer, "Graphics\\Sprites\\spr_wall")
                            {
                                IsSolid = true,
                            });
                            break;
                        case '^':
                            // Wall tile hot
                            l.Add(new Tile(game, layer, "Graphics\\Sprites\\spr_wall_hot")
                            {
                                IsSolid = true,
                                IsHot = true,
                            });
                            break;
                        case '*':
                            // Wall tile ice
                            l.Add(new Tile(game, layer, "Graphics\\Sprites\\spr_wall_ice")
                            {
                                IsSolid = true,
                                IsIce = true,
                            });
                            break;
                        default:
                            throw new NotImplementedException("Error loading: " + this[x, y]);
                    }
                }

            return l;
        }
    }
}
