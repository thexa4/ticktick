using Microsoft.Xna.Framework;
using TickTick.Services;

namespace TickTick.Services
{
    public interface IController
    {
        ControllerAction Action { get; }

        void Initialize();
        void Update(GameTime gameTime);
    }
}
