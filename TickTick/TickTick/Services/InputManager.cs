﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Remoting.Contexts;
using System.Linq;
using TickTick.Services;

namespace TickTick.Services
{
    /// <summary>
    /// The InputManager Class provides functions for serveral different types of input. Easily
    /// checkup on Keyboard or Mouse input and also use more advanced functions like IsTriggerd
    /// button state management.
    /// </summary>
    public partial class InputManager : GameComponent
    {
        /// <summary>
        /// Keyboard Input Reference
        /// </summary>
        public KeyboardInputComponent Keyboard
        {
            get;
            private set;
        }

        /// <summary>
        /// Mouse Input Reference
        /// </summary>
        public MouseInputComponent Mouse
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor for the Input Component
        /// </summary>
        /// <param name="game">Game to bind to</param>
        public InputManager(Game game)
            : base(game)
        {
            // Set this to Update before everything
            this.UpdateOrder = 0;

            this.Keyboard = new KeyboardInputComponent(game);
            this.Mouse = new MouseInputComponent(game);

            // Add as Service
            this.Game.Services.AddService(this.GetType(), this);
        }

        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing Values</param>
        public override void Update(GameTime gameTime)
        {
            // Update Input
            this.Keyboard.Update(gameTime);
            this.Mouse.Update(gameTime);

            // Update the Component
            base.Update(gameTime);
        }
    }

    /// <summary>
    /// The KeyboardComponent is a component that handles Keyboard input. It handles the actual
    /// keyboard states, whereas the manager only references. 
    /// </summary>
    public partial class KeyboardInputComponent : GameComponent
    {
        #region Fields
        private KeyboardState _currentState;
        private KeyboardState _previousState;
        private Boolean _isWatching;
        private Dictionary<Keys, TriggerKey> _triggers;

        /// <summary>
        /// The Default Watching Array defines what keys are always kept in the watch _triggers
        /// list. These keys are the arrows, wasd, space, enter, escape and backspace keys.
        /// </summary>
        private readonly Keys[] _DEFAULTWATCHING = new Keys[] { Keys.Enter, 
            Keys.Up, Keys.Down, Keys.Left, Keys.Right, 
            Keys.W, Keys.S, Keys.A, Keys.D, 
            Keys.Escape, Keys.Space, Keys.Back };

        private readonly Keys[] _LETTERS = new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, 
            Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, 
            Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z };

        private readonly Keys[] _DIGITS = new Keys[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, 
            Keys.D5, Keys.D6, Keys.D7,Keys.D8, Keys.D9 };

        internal Keys[] DEFAULTWATCHING { get { return _DEFAULTWATCHING; } }
        internal Keys[] LETTERS { get { return _LETTERS; } }
        internal Keys[] DIGITS { get { return _DIGITS; } }

        #endregion

        /// <summary>
        /// Gets/Sets the Watching Flag, which enables or disables watching keystrokes on trigger.
        /// </summary>
        internal Boolean IsWatching
        {
            get { return _isWatching; }
            set { _isWatching = value; }
        }
        /// <summary>
        /// Gets/Sets the current keyboard state
        /// </summary>
        protected KeyboardState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }
        /// <summary>
        /// Gets/Sets the previous keyboard state
        /// </summary>
        protected KeyboardState PreviousState
        {
            get { return _previousState; }
            set { _previousState = value; }
        }

        /// <summary>
        /// Gets the currently pressed keys
        /// </summary>
        public Keys[] PressedKeys
        {
            get {
                
                // If we are not in the correct context, we need to schedule updating the
                // get pressed key on the other context. We can only call void targets so
                // a private function will update the array. If we update on the wrong context
                // the array will simply be empty.

                if (Thread.CurrentContext != _executingThread)
                    _executingThread.DoCallBack(new CrossContextDelegate(UpdatePressedKeys));
                else
                    UpdatePressedKeys();

                return _pressedKeys; }
            set {
                _pressedKeys = value;
            }
        }

        private Context _executingThread;
        private Keys[] _pressedKeys;
        private void UpdatePressedKeys()
        {
            PressedKeys = _currentState.GetPressedKeys();
        }

        /// <summary>
        /// KeyboardComponent Constructor
        /// </summary>
        /// <param name="game">Game to bind to</param>
        internal KeyboardInputComponent(Game game)
            : base(game)
        {
            // Fill the states
            this.PreviousState = Keyboard.GetState();
            this.CurrentState = Keyboard.GetState();

            // Enable watching
            this.IsWatching = true;
            
            // Holds the data of the watched keys
            _triggers = new Dictionary<Keys, TriggerKey>();
            foreach (Keys key in this.DEFAULTWATCHING)
                Watch(key);

            // Save thread for multithreading support
            _executingThread = Thread.CurrentContext;
        }

        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing Values</param>
        public override void Update(GameTime gameTime)
        {
            // Move the old new state to the old slot
            // Set the new new state to the new slot
            this.PreviousState = this.CurrentState;
            this.CurrentState = Keyboard.GetState();

            // If Watching is Enabled
            if (this.IsWatching)
            {
                Int32 count = _triggers.Keys.Count;
                foreach (Keys key in _triggers.Keys)
                {
                    TriggerKey curobj;

                    if (_triggers.TryGetValue(key, out curobj))
                        curobj.Update(gameTime, CurrentState.IsKeyDown(key));
                }
            }

            // Update the Component
            base.Update(gameTime);
        }

        /// <summary>
        /// Returns whether a specific key is currently pressed down.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True on pressed down</returns>
        public Boolean IsKeyDown(Keys key)
        {
            return CurrentState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns whether a specific key is currently released up.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True on released up</returns>
        public Boolean IsKeyUp(Keys key)
        {
            return CurrentState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns whether a specific key was just pressed down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if newly pressed</returns>
        public Boolean IsKeyPressed(Keys key)
        {
            return IsKeyDown(key) && PreviousState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns whether a specific key is just released
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if newly released</returns>
        public Boolean IsKeyReleased(Keys key)
        {
            return IsKeyUp(key) && PreviousState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns whether a specific key is triggerd.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if triggerd</returns>
        public Boolean IsKeyTriggerd(Keys key)
        {
            TriggerKey curobj;

            if (_triggers.TryGetValue(key, out curobj))
            {
                return curobj.IsTriggerd;
            }
            else
            {
                Watch(key);
                return IsKeyTriggerd(key);
            }
        }

        /// <summary>
        /// Start watching a specific key.
        /// </summary>
        /// <param name="key">Key to Watch</param>
        public void Watch(Keys key)
        {
            if (_triggers.ContainsKey(key) == false)
                _triggers.Add(key, new TriggerKey());
        }

        /// <summary>
        /// Stop Watching a specific key.
        /// </summary>
        /// <param name="key">Key not to Watch</param>
        public void Unwatch(Keys key)
        {
            if (_triggers.ContainsKey(key) == true)
                _triggers.Remove(key);
        }

        /// <summary>
        /// Start Watching all alphanumeric keys
        /// </summary>
        public void WatchAlphaNumeric()
        {
            // Add Backspace
            Watch(Keys.Back);

            // Watch all Letters
            foreach (Keys key in LETTERS)
                Watch(key);

            // Watch all Digits
            foreach (Keys key in DIGITS)
                Watch(key);
        }

        /// <summary>
        /// Stop watching all alphanumeric keys
        /// </summary>
        public void UnwatchAlphaNumeric()
        {
            // UnWatch all Letters
            foreach (Keys key in LETTERS)
                if (!DEFAULTWATCHING.Contains(key))
                    Unwatch(key);

            // Unwatch all Digits
            foreach (Keys key in DIGITS)
                if (!DEFAULTWATCHING.Contains(key))
                    Unwatch(key);
            
            if (!DEFAULTWATCHING.Contains(Keys.Back))
                Unwatch(Keys.Back);
        }
    }

    /// <summary>
    /// The MouseInputComponent is a component that handles Mouse input. It handles
    /// the actual mouse states, whereas the manager only references. 
    /// </summary>
    public partial class MouseInputComponent : GameComponent
    {
        #region Fields
        private MouseState _currentState;
        private MouseState _previousState;
        private Boolean _isWatching;

        #endregion

        /// <summary>
        /// Gets/Sets the Watching Flag, which enables or 
        /// disables watching keystrokes on trigger.
        /// </summary>
        internal Boolean IsWatching
        {
            get { return _isWatching; }
            set { _isWatching = value; }
        }
        /// <summary>
        /// Gets/Sets the current mouse state
        /// </summary>
        protected MouseState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }
        /// <summary>
        /// Gets/Sets the previous mouse state
        /// </summary>
        protected MouseState PreviousState
        {
            get { return _previousState; }
            set { _previousState = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">Game to Bind to</param>
        public MouseInputComponent(Game game)
            : base(game)
        {
            // Fill the states
            this.PreviousState = Mouse.GetState();
            this.CurrentState = Mouse.GetState();
            // Enable watching
            this.IsWatching = true;
        }

        /// <summary>
        /// Button currently DOWN
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if button is down</returns>
        public Boolean IsButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return this.CurrentState.LeftButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return this.CurrentState.MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return this.CurrentState.RightButton == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Button currently UP
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if button is up</returns>
        public Boolean IsButtonUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return this.CurrentState.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return this.CurrentState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return this.CurrentState.RightButton == ButtonState.Released;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Button just Pressed
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if just Pressed</returns>
        public Boolean IsButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return IsButtonDown(button) && this.PreviousState.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return IsButtonDown(button) && this.PreviousState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return IsButtonDown(button) && this.PreviousState.RightButton == ButtonState.Released;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Button just Released Check
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if just Released</returns>
        public Boolean IsButtonReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return IsButtonUp(button) && this.PreviousState.LeftButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return IsButtonUp(button) && this.PreviousState.MiddleButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return IsButtonUp(button) && this.PreviousState.RightButton == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean IsMoved
        {
            get
            {
                return this.PreviousState.X != this.CurrentState.X || this.PreviousState.Y != this.CurrentState.Y;
            }
        }

        /// <summary>
        /// Gets/Sets the Mouse Position
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(this.CurrentState.X, this.CurrentState.Y); }
            set { Mouse.SetPosition((Int32)value.X, (Int32)value.Y); }
        }

        /// <summary>
        /// Gets/Sets Mouse X Position
        /// </summary>
        public Int32 X
        {
            get { return this.CurrentState.X; }
            set { Mouse.SetPosition(value, Mouse.GetState().Y); }
        }

        /// <summary>
        /// Gets/Sets Mouse Y Position
        /// </summary>
        public Int32 Y
        {
            get { return this.CurrentState.Y; }
            set { Mouse.SetPosition(Mouse.GetState().X, value); }
        }

        /// <summary>
        /// Gets the current ScrollWheelValue
        /// </summary>
        public Int32 ScrollWheelValue
        {
            get { return this.CurrentState.ScrollWheelValue; }
        }

        /// <summary>
        /// Gets the change in ScrollWheelValue since last frame
        /// </summary>
        public Int32 ScrollWheelChangeValue
        {
            get { return this.CurrentState.ScrollWheelValue - this.PreviousState.ScrollWheelValue; }
        }

        /// <summary>
        /// Check if mouse is over x
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <returns>True if over, false if not over</returns>
        public Boolean IsOverObj(Rectangle rect)
        {
            if (X < rect.X)
                return false;
            if (Y < rect.Y)
                return false;
            if (X > rect.X + rect.Width)
                return false;
            if (Y > rect.Y + rect.Height)
                return false;

            return true;
        }

        /// <summary>
        /// Check if mouse is over x
        /// </summary>
        /// <param name="rect">Vector4 rectangle</param>
        /// <returns>True if over, false if not over</returns>
        public Boolean IsOverObj(Vector4 rect)
        {
            if (X < rect.W)
                return false;
            if (Y < rect.X)
                return false;
            if (X > rect.W + rect.Y)
                return false;
            if (Y > rect.X + rect.Z)
                return false;

            return true;
        }

        /// <summary>
        /// Check if mouse is over x
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <param name="width">Width Parameter</param>
        /// <param name="height">Height Parameter</param>
        /// <returns>True if over, false if not over</returns>
        public Boolean IsOverObj(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            if (X < x)
                return false;
            if (Y < y)
                return false;
            if (X > x + width)
                return false;
            if (Y > y + height)
                return false;

            return true;
        }

        /// <summary>
        /// Check if mouse is over x
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="width">Width Parameter</param>
        /// <param name="height">Height Parameter</param>
        /// <returns>True if over, false if not over</returns>
        public Boolean IsOverObj(Vector2 pos, Int32 width, Int32 height)
        {
            if (X < pos.X)
                return false;
            if (Y < pos.Y)
                return false;
            if (X > pos.X + width)
                return false;
            if (Y > pos.Y + height)
                return false;

            return true;
        }

        /// <summary>
        /// Check if mouse is over x
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <param name="size">Size</param>
        /// <returns>True if over, false if not over</returns>
        public Boolean IsOverObj(Int32 x, Int32 y, Vector2 size)
        {
            if (X < x)
                return false;
            if (Y < y)
                return false;
            if (X > x + size.X)
                return false;
            if (Y > y + size.Y)
                return false;

            return true;
        }

        /// <summary>
        /// Check if mouse is over x
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="size">Size</param>
        /// <returns>True if over, false if not over</returns>
        public Boolean IsOverObj(Vector2 pos, Vector2 size)
        {
            if (X < pos.X)
                return false;
            if (Y < pos.Y)
                return false;
            if (X > pos.X + size.X)
                return false;
            if (Y > pos.Y + size.Y)
                return false;

            return true;
        }

        /// <summary>
        /// Check if mouse is over x
        /// </summary>
        /// <param name="_positionPortret"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        internal bool IsOverObj(Vector2 pos, Rectangle bounds)
        {
            if (X < pos.X + bounds.X)
                return false;
            if (Y < pos.Y + bounds.Y)
                return false;
            if (X > pos.X + bounds.X + bounds.Width)
                return false;
            if (Y > pos.Y + bounds.Y + bounds.Height)
                return false;

            return true;
        }


        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime">Snapshot of Timing Values</param>
        public override void Update(GameTime gameTime)
        {
            // Move the old new state to the old slot
            this.PreviousState = this.CurrentState;
            // Set the new new state to the new slot
            this.CurrentState = Mouse.GetState();
            // Update base
            base.Update(gameTime);
        }
    }

    /// <summary>
    /// The Triggerkey Helper class holds all the trigger data for a specific key.
    /// </summary>
    public class TriggerKey
    {
        #region Fields
        private Double _triggerTime;
        private TriggerState _triggerState;
        #endregion

        /// <summary>
        /// Gets Triggerd State
        /// </summary>
        internal Boolean IsTriggerd
        {
            get { return _triggerState == TriggerState.Active || _triggerState == TriggerState.PressedActive; }
        }

        /// <summary>
        /// Frame Renewal
        /// </summary>
        /// <param name="gameTime">Snapshot of timing values</param>
        /// <param name="state">Current key state</param>
        internal void Update(GameTime gameTime, Boolean state)
        {
            _triggerTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            switch (_triggerState)
            {
                // Inactive State (no candidate for triggering)
                case TriggerState.Inactive:
                    
                    if (state) // If pressed down
                    {
                        _triggerTime = 0;
                        _triggerState = TriggerState.Active;
                    }
                    break;

                // Pressed State (candidate for triggering)
                case TriggerState.Pressed:

                    if (!state) // If released up
                        _triggerState = TriggerState.Inactive;
                    else if (_triggerTime > 300) // If Repeatingly holding
                        _triggerState = TriggerState.PressedActive;
                    break;

                // Pressed Active State (triggerd, still pressed down)
                case TriggerState.PressedActive:

                    _triggerTime = 0;
                    _triggerState = state ? TriggerState.PressedInactive : TriggerState.Inactive;
                    break;

                // Pressed InActive State (candidate for triggering, still pressed down)
                case TriggerState.PressedInactive:

                    if (_triggerTime > 15) // (Re)-trigger this key
                        _triggerState = TriggerState.PressedActive;
                    break;

                // Active State (triggerd)
                case TriggerState.Active:
                    _triggerState = state ? TriggerState.Pressed : TriggerState.Inactive;
                    break;
            }
        }

        /// <summary>
        /// Triggerstate Enumeration
        /// </summary>
        private enum TriggerState : byte
        {
            Inactive = 0,
            JustPressed = 1,
            Pressed = 2,
            Active = 3,
            PressedActive = 4,
            PressedInactive = 5
        };
    }
}
