using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TickTick.Services
{
    /// <summary>
    /// The GameScreen provides basic functionality to be used with the ScreenManager.
    /// </summary>
    public class GameScreen
    {
        #region Fields

        private Boolean _otherScreenHasFocus;
        private ScreenState _screenState;

        protected Game _gameReference;
        protected ScreenManager _screenManager;
        protected InputManager _inputManager;
        protected AudioManager _audioManager;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            set { _screenManager = value; }
            get { return _screenManager; }
        }

        /// <summary>
        /// Exposes InputService
        /// </summary>
        public InputManager InputManager
        {
            set { _inputManager = value; }
            get { return _inputManager; }
        }

        /// <summary>
        /// Exposes AudioService
        /// </summary>
        public AudioManager AudioManager
        {
            get { return _audioManager; }
            set { _audioManager = value; }
        }

        /// <summary>
        /// Game reference
        /// </summary>
        public Game Game
        {
            get { return _gameReference; }
            set { _gameReference = value; }
        }

        /// <summary>
        /// Exposes ContentManager
        /// </summary>
        protected ContentManager ContentManager { get; set; }

        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public Boolean IsActive
        {
            get
            {
                return !_otherScreenHasFocus && _screenState == ScreenState.Active;
            }
        }


        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return _screenState; }
            protected set { _screenState = value; }
        }

        /// <summary>
        /// Currently visible (calls draw)
        /// </summary>
        public Boolean IsVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Currently enabled (calls update)in
        /// </summary>
        public Boolean IsEnabled
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public GameScreen(Game game)
        {
            _gameReference = game;
        }

        /// <summary>
        /// Initializes Screen, sets Variables
        /// </summary>
        public virtual void Initialize()
        {
            if (this.InputManager == null)
                this.InputManager = (InputManager)this.Game.Services.GetService(typeof(InputManager));

            if (this.InputManager == null)
                throw new InvalidOperationException("No Input service found.");

            if (this.AudioManager == null)
                this.AudioManager = (AudioManager)this.Game.Services.GetService(typeof(AudioManager));

            this.IsVisible = true;
            this.IsEnabled = true;
        }

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent(ContentManager contentManager)
        {
            this.ContentManager = contentManager;
        }

        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent()
        {
            if (this.ContentManager != null)
            {
                this.ContentManager.Unload();
                this.ContentManager.Dispose();

                this.ContentManager = null;
            }
        }

        /// <summary>
        /// Post process after adding screen to the list
        /// </summary>
        public virtual void AfterScreenIsAdded()
        {

        }

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike <see cref="HandleInput"/>, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing values</param>
        /// <param name="otherScreenHasFocus">Game is blurred</param>
        /// <param name="coveredByOtherScreen">Other GameScreen is active</param>
        public virtual void Update(GameTime gameTime, Boolean otherScreenHasFocus, Boolean coveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;

            if (coveredByOtherScreen)
            {
                _screenState = ScreenState.Hidden;
            }
            else
            {
                _screenState = ScreenState.Active;
            }
        }

        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        /// <param name="gameTime">Snapshot of timing Values</param>
        public virtual void HandleInput(GameTime gameTime)
        {

        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime">Snapshot of timing Values</param>
        public virtual void Draw(GameTime gameTime)
        {

        }

        /// <summary>
        /// Tells the screen to go away. Unlike <see cref="ScreenManager">RemoveScreen</see>, which
        /// instantly kills the screen,
        /// </summary>
        public void ExitScreen()
        {
            // When the transition finishes, remove the screen.
            _screenManager.RemoveScreen(this);

            if (Exited != null)
                Exited(this, EventArgs.Empty);

        }

        /// <summary>
        /// OnExited Event
        /// </summary>
        public event EventHandler Exited;
    }
}
