using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickTick.Drawing.Actors;

namespace TickTick.Services
{
    class CollisionManager
    {
        protected List<ICollidable> Colliders { get; set; }

        public void Add(ICollidable c)
        {
            Colliders.Add(c);
        }

        public void Remove(ICollidable c)
        {
            Colliders.Remove(c);
        }
    }
}
