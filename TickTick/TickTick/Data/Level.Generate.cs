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
        public List<DrawableGameComponent> GenerateComponents(Game game, Layer layer)
        {
            var l = new List<DrawableGameComponent>();
            var state = new LevelState();

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
                            //TODO: Add endtile loader
                            break;
                        case 'W':
                            // Water tile
                            l.Add(new Water(game, layer, "Graphics/Sprites/spr_water", state)
                            {
                                Position = position,
                            });
                            break;
                        case '1':
                            // Player tile
                            l.Add(new Player(game, layer)
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
