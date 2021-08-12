using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EngineHelper;
using EngineHelper.EHGraphics;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
#endif

namespace EngineHelper.EHInputs
{
    public sealed class EHInputManager
    {
#if WINDOWS_PHONE
        public EHTouch Touch = new EHTouch();
        public EHAccelerometer Accelerometer = new EHAccelerometer();

        ButtonState backButton;
#endif

#if WINDOWS
        public EHMouse Mouse = new EHMouse();
#endif

#if !WINDOWS_PHONE
        public EHKeyboard Keyboard = new EHKeyboard();
        public EHGamePad[] GamePads = { new EHGamePad(), new EHGamePad(), new EHGamePad(), new EHGamePad() };
        private int numConnectedGamePads; //currently connected game pads
#endif

#if WINDOWS_PHONE
        public ButtonState BackButton
        {
            get { return backButton; }
        }
#endif
        //Reads the states of attached input devices. NOTE: Should usually only be called ONCE per frame.
        public void ReadDeviceStates()
        {
#if WINDOWS_PHONE
            Touch.ReadState();
            Accelerometer.ReadState();
            backButton = GamePad.GetState(PlayerIndex.One).Buttons.Back;
#endif

#if WINDOWS
            Mouse.ReadState();
#endif

#if !WINDOWS_PHONE
            Keyboard.ReadState();

            numConnectedGamePads = 0;
            for (int i = 0; i < 4; i++)
            {
                GamePads[i].ReadState();

                if (GamePads[i].IsConnected)
                {
                    numConnectedGamePads++;
                }
            }
#endif
        }

#if !WINDOWS_PHONE
        //Returns the number of currently connected gamepads.
        public int ConnectedGamePads
        {
            get { return numConnectedGamePads; }
        }
    }
#endif

#if WINDOWS_PHONE
        ///////////////////////////////////////////////////////////////
        //Touch input
        public sealed class EHTouch
        {
            TouchCollection touchLocations;
            TouchPanelCapabilities touchCaps;

            List<GestureSample> gestureSamples;

            public int MaximumTouchCount
            {
                get { return touchCaps.MaximumTouchCount; }
            }

            //Returns the touch locations for this frame.
            public TouchCollection TouchLocations
            {
                get { return touchLocations; }
            }

            public EHTouch()
            {
                touchLocations = new TouchCollection();
                touchCaps = TouchPanel.GetCapabilities();

                TouchPanel.EnabledGestures = GestureType.None;

                gestureSamples = new List<GestureSample>();
            }

            //Updates the current touch locations and gestures. MUST be called once a frame.
            public void ReadState()
            {
                touchLocations = TouchPanel.GetState();
                gestureSamples.Clear();

                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gestureSample = TouchPanel.ReadGesture();
                    gestureSamples.Add(gestureSample);
                }
            }

            //Enables all gestures in ONE CALL.
            //Entries must be LOGICALLY OR'ed together (e.g. GestureType.DoubleTap | GestureType.Flick)
            //Disable gestures by passing in GestureType.None.

            //<param name="enabledGestures"></param>
            public void EnableGestures(GestureType enabledGestures)
            {
                TouchPanel.EnabledGestures = enabledGestures;
            }

            //Returns the gestures for this frame.
            public List<GestureSample> GestureSamples
            {
                get { return gestureSamples; }
            }
        }

        public class YAccelerometer
        {
            private Microsoft.Devices.Sensors.Accelerometer accelerometer = new Microsoft.Devices.Sensors.Accelerometer();

            private Vector3 rawAcceleration; // what the phone returns

            //translated according to phone display orientation
            private Vector3 acceleration3D;
            private Vector2 acceleration2D; // translated into 2D screen coords

            /// Returns the pull of gravity on the device as a normalized 3D vector (1.0 = 1g force).
            /// Returns <0.0, 0.0, -1.0> when device lies on a flat surface, screen up.
            /// Returns <0.0, -1.0, 0.0> when the device is perpendicular to a flat surface.
            public Vector3 Acceleration
            {
                get { return rawAcceleration; }
            }

            /// Returns the pull of gravity on the device as a normalized 3D vector
            /// relative to the display orientation of the phone.
            /// Returns <0.0, -1.0, 0.0> when device display orientation is considered straight "up"
            /// Returns <0.0, 1.0, 0.0> when the device is tilted all the way to the display's "right".
            public Vector3 RelativeAcceleration3D
            {
                get { return acceleration3D; }
            }

            /// Returns the pull of gravity on the device as a normalized 2D vector
            /// relative to the display orientation of the phone.
            /// Returns <0.0, 1.0> when device display orientation is considered straight "up"
            /// Returns <0.0, 1.0> when the device is tilted all the way to the display's "right".
            public Vector2 RelativeAcceleration2D
            {
                get { return acceleration2D; }
            }


            /// Updates the current acceleration readings.
            /// MUST be called once a frame.
            public void ReadState()
            {
                //Get the REAL acceleration
                rawAcceleration = accelerometer.CurrentValue.Acceleration;

                //translates accel vector so that +Y is up for that orientation (-Y in 2D)
                acceleration3D = rawAcceleration;

                DisplayOrientation orientation = YEngine.Graphics.Device.PresentationParameters.DisplayOrientation;     //Adjust to the correct path. //-CJM

                switch (orientation)
                {
                    case DisplayOrientation.Default:
                    case DisplayOrientation.Portrait:
                        break;
                    case DisplayOrientation.LandscapeLeft:
                        {
                            Matrix rotZ = Matrix.CreateRotationZ(MathHelper.PiOver2);
                            acceleration3D = Vector3.TransformNormal(acceleration3D, rotZ);
                        }
                        break;
                    case DisplayOrientation.LandscapeRight:
                        {
                            Matrix rotZ = Matrix.CreateRotationZ(-MathHelper.PiOver2);
                            acceleration3D = Vector3.TransformNormal(acceleration3D, rotZ);
                        }
                        break;
                }

                acceleration2D.X = acceleration3D.X;
                acceleration2D.Y = -acceleration3D.Y; // flipped for 2D space
            }

            //Start reading from the accelerometer. NOTE: Uses more battery life to read acceleration data. Only activate when using it!
            public void Start()
            {
                try
                {
                    //accelerometer.CurrentValueChanged += AccelerometerReadChanged;
                    accelerometer.Start();
                }
                catch (AccelerometerFailedException e)
                {
                }
            }

            //void AccelerometerReadChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
            //{
            //    e.SensorReading.Acceleration;
            //}

            /// Stop reading from the accelerometer. NOTE: Uses more battery life to read acceleration data. Turn off when not using it!
            public void Stop()
            {
                accelerometer.Stop();
            }
        };
#endif

#if WINDOWS
    //Mouse
    public enum MouseButtons
    {
        LeftButton = 0,
        MiddleButton,
        RightButton,
        XButton1,
        XButton2
    };

    public sealed class EHMouse
    {
        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private Vector2 moveDelta;
        private int wheelDelta;

        //Updates the current and previous states of the mouse. MUST be called once a frame.
        public void ReadState()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            moveDelta = new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
            wheelDelta = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
        }

        //Reports if a button is down in the passed in state.
        private bool IsButtonDown(MouseState state, MouseButtons btn)
        {
            bool buttonDown = false;

            switch (btn)
            {
                case MouseButtons.LeftButton:
                    buttonDown = (state.LeftButton == ButtonState.Pressed);
                    break;
                case MouseButtons.MiddleButton:
                    buttonDown = (state.MiddleButton == ButtonState.Pressed);
                    break;
                case MouseButtons.RightButton:
                    buttonDown = (state.RightButton == ButtonState.Pressed);
                    break;
                case MouseButtons.XButton1:
                    buttonDown = (state.XButton1 == ButtonState.Pressed);
                    break;
                case MouseButtons.XButton2:
                    buttonDown = (state.XButton2 == ButtonState.Pressed);
                    break;
            }

            return buttonDown;
        }

        //Returns the mouse position as a Point for ease of use.
        public Point PositionPoint
        {
            get { return new Point(currentMouseState.X, currentMouseState.Y); }
        }

        //Returns the mouse position as a Vector2 for ease of use.
        public Vector2 Position
        {
            get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
        }

        //Returns the amount the mouse position has changed from the previous frame.
        public Vector2 MoveDelta
        {
            get { return moveDelta; }
        }

        //Reports how much the mouse wheel has changed this frame.
        public int WheelDelta
        {
            get { return wheelDelta; }
        }

        //Checks if a mouse button is currently down.
        public bool ButtonDown(MouseButtons btn)
        {
            return IsButtonDown(currentMouseState, btn);
        }

        //Checks if a mouse button is currently up.
        public bool ButtonUp(MouseButtons btn)
        {
            return !IsButtonDown(currentMouseState, btn);
        }

        //Checks if a mouse button was pressed this frame.
        public bool ButtonPressed(MouseButtons btn)
        {
            return IsButtonDown(currentMouseState, btn) && !IsButtonDown(previousMouseState, btn);
        }

        //Checks if a mouse button was released this frame.
        public bool ButtonReleased(MouseButtons btn)
        {
            return !IsButtonDown(currentMouseState, btn) && IsButtonDown(previousMouseState, btn);
        }
    }
#endif

#if !WINDOWS_PHONE
    //Keyboard
    public sealed class EHKeyboard
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        //Updates the current and previous states of the Keyboard. MUST be called once a frame.
        public void ReadState()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        //Checks if a key is currently down.
        public bool KeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        //Checks if a key is currently up.
        public bool KeyUp(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key);
        }

        //Checks if a key was pressed this frame.
        public bool KeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        //Checks if a key was released this frame.
        public bool KeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
        }

        public Keys[] GetPressedKeys()
        {
            return currentKeyboardState.GetPressedKeys();
        }
    }

    //GamePad
    public sealed class EHGamePad
    {
        private static int numCreated = 0;

        private PlayerIndex playerIndex;

        private GamePadState currentGamePadState;
        private GamePadState previousGamePadState;

        private static IEnumerable<Buttons> individualButtons =
                from x in typeof(Buttons).GetFields(BindingFlags.Static | BindingFlags.Public)
                select (Buttons)x.GetValue(null);

        public EHGamePad()
        {
            playerIndex = (PlayerIndex)numCreated;
            numCreated++;
        }

        //Is the gamepad connected?
        public bool IsConnected
        {
            get { return currentGamePadState.IsConnected; }
        }

        //Updates the current and previous states of the GamePad. MUST be called once a frame.
        public void ReadState()
        {
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(playerIndex);
        }

        //Checks if a button is down. (includes DPad and thumbstick buttons)
        public bool ButtonDown(Buttons button)
        {
            //If gamepad is unplugged, return false
            if (IsConnected == false)
                return false;

            return currentGamePadState.IsButtonDown(button);
        }

        //Checks if a button is up. (includes DPad and thumbstick buttons)
        public bool ButtonUp(Buttons button)
        {
            //If gamepad is unplugged, return false
            if (IsConnected == false)
                return false;

            return currentGamePadState.IsButtonUp(button);
        }

        //Checks if a button was pressed this frame. (includes DPad and thumbstick buttons)
        public bool ButtonPressed(Buttons button)
        {
            //If gamepad is unplugged, return false
            if (IsConnected == false)
                return false;

            return currentGamePadState.IsButtonDown(button) &&
                   previousGamePadState.IsButtonUp(button);
        }

        //Checks if a button was released this frame. (includes DPad and thumbstick buttons)
        public bool ButtonReleased(Buttons button)
        {
            //If gamepad is unplugged, return false
            if (IsConnected == false)
                return false;

            return currentGamePadState.IsButtonUp(button) &&
                   previousGamePadState.IsButtonDown(button);
        }

        //Determines if any of the buttons specified are currently pressed.
        public bool AnyButtonDown(Buttons buttons)
        {
            //If gamepad is unplugged, return false
            if (IsConnected == false)
                return false;

            //iterate through all the possible buttons
            foreach (var button in individualButtons)
            {
                // if the parameter contains the button and the state has the button as pressed, return true
                if ((buttons & button) != 0 && currentGamePadState.IsButtonDown(button))
                    return true;
            }

            //if we're here, none of the requested buttons were pressed
            return false;
        }

        //Triggers

        //Gets the current state of the trigger.
        public float LeftTrigger
        {
            get
            {
                //If gamepad is unplugged, return 0.0f
                if (IsConnected == false)
                    return 0.0f;

                return currentGamePadState.Triggers.Left;
            }
        }

        //Gets the current state of the trigger.
        public float RightTrigger
        {
            get
            {
                //If gamepad is unplugged, return 0.0f
                if (IsConnected == false)
                    return 0.0f;

                return currentGamePadState.Triggers.Right;
            }
        }

        //Thumbsticks

        /// Gets the current state of the thumbstick.
        public Vector2 LeftThumbstick
        {
            get
            {
                //If gamepad is unplugged, return Vector2.Zero
                if (IsConnected == false)
                    return Vector2.Zero;

                return currentGamePadState.ThumbSticks.Left;
            }
        }

        //Gets the current state of the thumbstick.
        public Vector2 RightThumbstick
        {
            get
            {
                //If gamepad is unplugged, return Vector2.Zero
                if (IsConnected == false)
                    return Vector2.Zero;

                return currentGamePadState.ThumbSticks.Right;
            }
        }
    }
#endif
}

