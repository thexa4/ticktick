using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickTick.Drawing.Actors;
using Microsoft.Xna.Framework;

namespace TickTick.Services
{
    /// <summary>
    /// Detects collisions and notifies objects of them
    /// </summary>
    class CollisionManager : GameComponent
    {
        /// <summary>
        /// The list of objects that can collide
        /// </summary>
        protected List<ICollidable> Colliders { get; set; }
        /// <summary>
        /// The list of objects currently touching
        /// </summary>
        protected List<Tuple<ICollidable, ICollidable>> CollisionPairs { get; set; }

        /// <summary>
        /// Creates a new CollisionManger
        /// </summary>
        public CollisionManager(Game game) : base(game)
        {
            Colliders = new List<ICollidable>();
            CollisionPairs = new List<Tuple<ICollidable, ICollidable>>();

            Game.Services.AddService(typeof(CollisionManager), this);
            // Update last to avoid drawing sprites in eachother
            this.UpdateOrder = Int32.MaxValue;
        }

        /// <summary>
        /// Calculates collisions and notifies objects
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            var cp = new List<Tuple<ICollidable, ICollidable>>();
            foreach (var t in CollisionPairs)
            {
                if (Collides(t.Item1, t.Item2))
                {
                    cp.Add(t);
                }
                else
                {
                    t.Item1.EndTouch(t.Item2);
                    t.Item2.EndTouch(t.Item1);
                }
            }
            CollisionPairs = cp;

            for(int i = 0; i < Colliders.Count; i++)
                for (int j = i + 1; j < Colliders.Count; j++)
                {
                    var a = Colliders[i];
                    var b = Colliders[j];
                    if (Collides(a, b))
                    {
                        var t = new Tuple<ICollidable, ICollidable>(a,b);
                        if(!CollisionPairs.Contains(t))
                        {
                            CollisionPairs.Add(t);
                            a.StartTouch(b);
                            b.StartTouch(a);
                        }
                    }
                }
        }

        /// <summary>
        /// Calculates wether two objects collide
        /// </summary>
        /// <param name="a">The first object</param>
        /// <param name="b">The second object</param>
        /// <returns>Wether a collision is taking place</returns>
        public bool Collides(ICollidable a, ICollidable b)
        {
            Rectangle ra = new Rectangle((int)a.Position.X, (int)a.Position.Y, (int)a.Size.X, (int)a.Size.Y);
            Rectangle rb = new Rectangle((int)b.Position.X, (int)b.Position.Y, (int)b.Size.X, (int)b.Size.Y);
            return ra.Intersects(rb);
        }

        /// <summary>
        /// Adds a new collision object
        /// </summary>
        /// <param name="c"></param>
        public void Add(ICollidable c)
        {
            Colliders.Add(c);
        }

        /// <summary>
        /// Removes a collision object
        /// </summary>
        /// <param name="c"></param>
        public void Remove(ICollidable c)
        {
            Colliders.Remove(c);
            var cp = new List<Tuple<ICollidable, ICollidable>>();
            foreach(var t in CollisionPairs)
                if (t.Item1 == c || t.Item2 == c)
                {
                    t.Item1.EndTouch(t.Item2);
                    t.Item2.EndTouch(t.Item1);
                }
                else
                {
                    cp.Add(t);
                }

            CollisionPairs = cp;
        }
    }
}
