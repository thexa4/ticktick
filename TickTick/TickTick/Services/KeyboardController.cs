using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TickTick.Services
{
    public class KeyboardController : GameComponent, IController
    {
        /// <summary>
        /// Current Action
        /// </summary>
        public ControllerAction Action
        {
            get;
            protected set;
        }

        private Keys _jump, _left, _right;
        private InputManager _inputManager;

        /// <summary>
        /// Creates a new Paddle Controller
        /// </summary>
        /// <param name="game">Game to bind to</param>
        /// <param name="down">Soft down button</param>

        /// <param name="left">Move left button</param>
        /// <param name="right">Move right button</param>
        public KeyboardController(Game game, Keys jump, Keys left, Keys right) : base(game)
        {
            _right = right;
            _left = left;
            _jump = jump;
        }

        /// <summary>
        /// Get or set the key used to control an action by action
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Keys this[ControllerAction index] {
            get
            {
                switch (index)
                {
                    case ControllerAction.Left:
                        return _left;
                    case ControllerAction.Right:
                        return _right;
                    case ControllerAction.Jump:
                        return _jump;
                    
                    default:
                        return Keys.None;
                }
            }

            set
            {
                switch (index)
                {
                    case ControllerAction.Left:
                        _left = value;
                        break;
                    case ControllerAction.Right:
                        _right = value;
                        break;
                    case ControllerAction.Jump:
                        _jump = value;
                        break;
                    
                    default:
                        return;
                }
            }
            
        }

        /// <summary>
        /// Initializes controller
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _inputManager = (InputManager)this.Game.Services.GetService(typeof(InputManager));
        }

        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Action = ControllerAction.None;

            if (!this.Enabled)
                return;

            if (_inputManager.Keyboard.IsKeyDown(_left))
                Action |= ControllerAction.Left;
            if (_inputManager.Keyboard.IsKeyDown(_right))
                Action |= ControllerAction.Right;
            if (_inputManager.Keyboard.IsKeyDown(_jump))
                Action |= ControllerAction.Jump;
        }
    }
}
