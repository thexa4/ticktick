using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TickTick.Services;
using Microsoft.Xna.Framework.Graphics;

namespace TickTick.Drawing
{
    /// <summary>
    /// Represents a depth slice of the current scene
    /// Draws and updates all child components
    /// </summary>
    public class Layer : DrawableGameComponent
    {
        /// <summary>
        /// The subcomponents
        /// </summary>
        protected List<DrawableGameComponent> _children;

        /// <summary>
        /// Draws all components in this layer
        /// </summary>
        /// <param name="gameTime">The current gameTime</param>
        public override void Draw(GameTime gameTime)
        {
            var screenmanager = (ScreenManager)this.Game.Services.GetService(typeof(ScreenManager));
            screenmanager.SpriteBatch.Begin();
            foreach (var child in _children)
                if(child.Visible)
                    child.Draw(gameTime);
            screenmanager.SpriteBatch.End();
        }

        /// <summary>
        /// Updates all components in this layer
        /// </summary>
        /// <param name="gameTime">The current gameTime</param>
        public override void Update(GameTime gameTime)
        {
            foreach (var child in _children)
                if(child.Enabled)
                    child.Update(gameTime);
        }

        /// <summary>
        /// Adds a new component to this layer
        /// </summary>
        /// <param name="component">The component to add</param>
        public void Add(DrawableGameComponent component)
        {
            _children.Add(component);
            component.Initialize();
        }

        /// <summary>
        /// Removes a component from this layer
        /// </summary>
        /// <param name="component">The component to remove</param>
        public void Remove(DrawableGameComponent component)
        {
            _children.Remove(component);
        }

        /// <summary>
        /// Creates a new Layer
        /// </summary>
        /// <param name="game">The game the Layer belongs to</param>
        public Layer(Game game)
            : base(game)
        {
            _children = new List<DrawableGameComponent>();
        }
    }
}
