using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace XnaHelpers.GameEngine
{
    /// <summary>
    /// Gets input from various input devices
    /// </summary>
    public class GameInput
    {
        #region Zune Input
        /// <summary>
        /// Gets input from a Zune
        /// </summary>
        public class Zune
        {
            /// <summary>
            /// Represents the buttons on a Zune
            /// </summary>
            public enum ZuneButtons
            {
                Play = Buttons.B,
                Back = Buttons.Back,
                Center = Buttons.A,
                Up = Buttons.DPadUp,
                Down = Buttons.DPadDown,
                Left = Buttons.DPadLeft,
                Right = Buttons.DPadRight,
                Click = Buttons.LeftShoulder
            }

            #region Button Input Methods
            /// <summary>
            /// If the specified button is pressed
            /// </summary>
            /// <param name="button">The button to check</param>
            /// <returns></returns>
            public static bool IsButtonDown(ZuneButtons button)
            {
                return GamePad.GetState(PlayerIndex.One).IsButtonDown((Buttons)button);
            }

            /// <summary>
            /// If the specified button is not pressed
            /// </summary>
            /// <param name="button">The button to check</param>
            /// <returns></returns>
            public static bool IsButtonUp(ZuneButtons button)
            {
                return GamePad.GetState(PlayerIndex.One).IsButtonUp((Buttons)button);
            }

            /// <summary>
            /// Gets a Vector2 from the directional keys on the keyboard as if they were a thumbstick
            /// </summary>
            /// <returns></returns>
            public static Vector2 GetDirectionalVector()
            {
                //Uses the BoolToInt method to get a directional vector similar to thumbstick rotation
                return new Vector2(-GameMath.BoolToInt(IsButtonDown(ZuneButtons.Left)) + GameMath.BoolToInt(IsButtonDown(ZuneButtons.Right)),
                    -GameMath.BoolToInt(IsButtonDown(ZuneButtons.Up)) + GameMath.BoolToInt(IsButtonDown(ZuneButtons.Down)));
            }
            #endregion

            #region Zune Pad Methods
            /// <summary>
            /// Gets the finger position on the Zune Pad
            /// </summary>
            /// <returns></returns>
            public static Vector2 GetSlidePosition()
            {
                return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            }
            #endregion
        }
        #endregion

        #region Xbox 360 Input
        /// <summary>
        /// Gets input from an Xbox 360 controller
        /// </summary>
        public class Xbox360
        {
            /// <summary>
            /// Represents the two thumbsticks on an Xbox 360 controller
            /// </summary>
            public enum Thumbstick
            {
                Left,
                Right
            }

            #region Connection Methods
            /// <summary>
            /// If the controller for the specified player is connected
            /// </summary>
            /// <param name="playerIndex">The controller to check</param>
            /// <returns></returns>
            public static bool IsControllerConnected(PlayerIndex playerIndex)
            {
                return GamePad.GetState(playerIndex).IsConnected;
            }
            #endregion

            #region Button Input Muthods
            /// <summary>
            /// If the specified button is pressed
            /// </summary>
            /// <param name="button">The button to check</param>
            /// <param name="playerIndex">The player to check</param>
            /// <returns></returns>
            public static bool IsButtonDown(Buttons button, PlayerIndex playerIndex)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(button);
            }

            /// <summary>
            /// If the specified button is not pressed
            /// </summary>
            /// <param name="button">The button to check</param>
            /// <param name="playerIndex">The player to check</param>
            /// <returns></returns>
            public static bool IsButtonUp(Buttons button, PlayerIndex playerIndex)
            {
                return GamePad.GetState(playerIndex).IsButtonUp(button);
            }

            /// <summary>
            /// Gets a Vector2 from the d-pad on the controller as if it were a thumbstick
            /// </summary>
            /// <returns></returns>
            public static Vector2 GetDPadVector(PlayerIndex playerIndex)
            {
                GamePadState state = GamePad.GetState(playerIndex);
                return new Vector2(-GameMath.BoolToInt(state.IsButtonDown(Buttons.DPadLeft)) + GameMath.BoolToInt(state.IsButtonDown(Buttons.DPadRight)),
                    -GameMath.BoolToInt(state.IsButtonDown(Buttons.DPadUp)) + GameMath.BoolToInt(state.IsButtonDown(Buttons.DPadDown)));
            }
            #endregion

            #region Thumbstick Methods
            /// <summary>
            /// Gets the rotation of the specified thumbstick
            /// </summary>
            /// <param name="thumbstick">The thumbstick to use</param>
            /// <param name="playerIndex">The player to check</param>
            /// <returns></returns>
            public static Vector2 GetThumbstickRotation(Thumbstick thumbstick, PlayerIndex playerIndex)
            {
                //Gets the thumbstick rotation for the specified thumbstick
                if (thumbstick == Thumbstick.Left)
                {
                    return GamePad.GetState(playerIndex).ThumbSticks.Left;
                }
                else
                {
                    return GamePad.GetState(playerIndex).ThumbSticks.Right;
                }
            }
            #endregion
        }
        #endregion

        #region Windows Input
        /// <summary>
        /// Gets input from Windows input devices such as mice and keyboards
        /// </summary>
        public class Windows
        {
            #region Mouse Input
            /// <summary>
            /// Represents the buttons on a mouse
            /// </summary>
            public enum MouseButtons
            {
                Left,
                Right,
                Middle,
                ExtraButton1,
                ExtraButton2
            }

            #region Variable Declarations
            /// <summary>
            /// The mouse position. Setting mouse position is relative to the game window, but getting it is relative to the screen.
            /// </summary>
            public Vector2 MousePosition
            {
                get
                {
                    //Gets the mouse position as a Vector2
                    return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                }
                set
                {
                    //Uses the Mouse.SetPosition method to set the position
                    Mouse.SetPosition((int)value.X, (int)value.Y);
                }
            }
            #endregion

            #region Button Input Methods
            /// <summary>
            /// If the mouse button specified is pressed
            /// </summary>
            /// <param name="button">The button to check</param>
            /// <returns></returns>
            public static bool IsMouseButtonDown(MouseButtons button)
            {
                //Checks the mouse state of the specified button
                if (button == MouseButtons.Left)
                {
                    return (Mouse.GetState().LeftButton == ButtonState.Pressed);
                }
                else if (button == MouseButtons.Right)
                {
                    return (Mouse.GetState().RightButton == ButtonState.Pressed);
                }
                else if (button == MouseButtons.Middle)
                {
                    return (Mouse.GetState().MiddleButton == ButtonState.Pressed);
                }
                else if (button == MouseButtons.ExtraButton1)
                {
                    return (Mouse.GetState().XButton1 == ButtonState.Pressed);
                }
                else if (button == MouseButtons.ExtraButton2)
                {
                    return (Mouse.GetState().XButton2 == ButtonState.Pressed);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// If the mouse button specified is not pressed
            /// </summary>
            /// <param name="button">The button to check</param>
            /// <returns></returns>
            public static bool IsMouseButtonUp(MouseButtons button)
            {
                return !IsMouseButtonDown(button);
            }
            #endregion
            #endregion

            #region Keyboard Input
            /// <summary>
            /// If the key specified is pressed
            /// </summary>
            /// <param name="key">The key to check</param>
            /// <returns></returns>
            public static bool IsKeyDown(Keys key)
            {
                return Keyboard.GetState().IsKeyDown(key);
            }

            /// <summary>
            /// If the key specified is not pressed
            /// </summary>
            /// <param name="key">The key to check</param>
            /// <returns></returns>
            public static bool IsKeyUp(Keys key)
            {
                return Keyboard.GetState().IsKeyUp(key);
            }

            /// <summary>
            /// Gets a Vector2 from the directional keys on the keyboard as if they were a thumbstick
            /// </summary>
            /// <returns></returns>
            public static Vector2 GetDirectionalVector()
            {
                //Uses the BoolToInt method to get a directional vector similar to thumbstick rotation
                KeyboardState state = Keyboard.GetState();
                return new Vector2(-GameMath.BoolToInt(state.IsKeyDown(Keys.Left)) + GameMath.BoolToInt(state.IsKeyDown(Keys.Right)),
                    -GameMath.BoolToInt(state.IsKeyDown(Keys.Up)) + GameMath.BoolToInt(state.IsKeyDown(Keys.Down)));
            }
            #endregion
        }
        #endregion
    }
}
