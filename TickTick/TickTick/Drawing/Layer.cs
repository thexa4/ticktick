using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TickTick.Drawing
{
    /// <summary>
    /// Represents a depth slice of the current scene
    /// Draws and updates all child components
    /// </summary>
    class Layer : DrawableGameComponent
    {
        /// <summary>
        /// The subcomponents
        /// </summary>
        public List<DrawableGameComponent> Children { get; protected set; }

        /// <summary>
        /// Draws all components in this layer
        /// </summary>
        /// <param name="gameTime">The current gameTime</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var child in Children)
                child.Draw(gameTime);
        }

        /// <summary>
        /// Updates all components in this layer
        /// </summary>
        /// <param name="gameTime">The current gameTime</param>
        public override void Update(GameTime gameTime)
        {
            foreach (var child in Children)
                child.Update(gameTime);
        }

        /// <summary>
        /// Creates a new Layer
        /// </summary>
        /// <param name="game">The game the Layer belongs to</param>
        public Layer(Game game)
            : base(game)
        {
            Children = new List<DrawableGameComponent>();
        }
    }
}
