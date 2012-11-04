using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Drawing.Actors;
using TickTick.Drawing;

namespace TickTick.Data
{
    partial class Level
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="layer"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<DrawableGameComponent> GenerateComponents(Game game, Layer layer, out LevelState state)
        {
            var l = new List<DrawableGameComponent>();
            state = new LevelState() { TimeLeft = 30 };

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    Vector2 position = new Vector2(x * TileWidth, y * TileHeight);
                    switch (this[x, y])
                    {
                        case '.':
                            // Background, draw nothing
                            break;
                        case '-':
                            // Normal platform
                            l.Add(new Tile(game, layer, "Graphics\\Tiles\\spr_platform")
                            {
                                IsSolid = false,
                                IsPlatform = true,
                                Position = position,
                            });
                            break;
                        case '+':
                            // Hot platform
                            l.Add(new Tile(game, layer, "Graphics\\Tiles\\spr_platform_hot")
                            {
                                IsSolid = false,
                                IsPlatform = true,
                                IsHot = true,
                                Position = position,
                            });
                            break;
                        case '@':
                            // Ice platform
                            l.Add(new Tile(game, layer, "Graphics\\Tiles\\spr_platform_ice")
                            {
                                IsSolid = false,
                                IsPlatform = true,
                                IsIce = true,
                                Position = position,
                            });
                            break;
                        case 'X':
                            // End tile
                            l.Add(new Finish(game, layer, "Graphics/Sprites/spr_goal", state)
                            {
                                Position = position - Vector2.UnitY * 23,
                            });
                            break;
                        case 'W':
                            // Water tile
                            l.Add(new Water(game, layer, "Graphics/Sprites/spr_water", state)
                            {
                                Position = position + Vector2.UnitX * 23,
                            });
                            break;
                        case '1':
                            // Player tile
                            l.Add(new Player(game, layer, state)
                            {
                                Position = position - Vector2.UnitY * TileHeight,
                            });
                            break;
                        case '#':
                            // Wall tile
                            l.Add(new Tile(game, layer, "Graphics\\Tiles\\spr_wall")
                            {
                                IsSolid = true,
                                Position = position,
                            });
                            break;
                        case '^':
                            // Wall tile hot
                            l.Add(new Tile(game, layer, "Graphics\\Tiles\\spr_wall_hot")
                            {
                                IsSolid = true,
                                IsHot = true,
                                Position = position,
                            });
                            break;
                        case '*':
                            // Wall tile ice
                            l.Add(new Tile(game, layer, "Graphics\\Tiles\\spr_wall_ice")
                            {
                                IsSolid = true,
                                IsIce = true,
                                Position = position,
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
