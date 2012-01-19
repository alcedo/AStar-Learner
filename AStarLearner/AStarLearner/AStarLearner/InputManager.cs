using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace XnaHelpers
{
    /**
     * This class handles NON Kinect input from the user.
     * This class would have a reference to a list of 
     * textures that exist within the game, and by
     * checking the skeletal joints, check if there is any 
     * collisions, which means input. 
     */
    namespace InputFrameWork
    {
        public class InputComponent : Microsoft.Xna.Framework.GameComponent
        {
            bool keyboard;
            bool mouse;
            bool gamepad;
            bool chatpad;

            public InputComponent(Game game, bool UpdateKeyboard, bool UpdateMouse, bool UpdateGamePad, bool UpdateChatPad)
                : base(game)
            {
                keyboard = UpdateKeyboard;
                mouse = UpdateMouse;
                gamepad = UpdateGamePad;
                chatpad = UpdateChatPad;

                Input.keyboard = new Input_Keyboard();
                Input.mouse = new Input_Mouse();
                Input.gamepad = new Input_GamePad();
                Input.chatpad = new Input_ChatPad();
            }

            public override void Initialize()
            {
                base.Initialize();
            }

            public override void Update(GameTime gameTime)
            {
                if (keyboard)
                    Input.Keyboard.Update();
                if (mouse)
                    Input.Mouse.Update();
                if (gamepad)
                    Input.gamepad.Update();
                if (chatpad)
                    Input.chatpad.Update();

                base.Update(gameTime);
            }
        }

        public static class Input
        {
            public static Input_Keyboard keyboard;

            public static Input_Mouse mouse;

            public static Input_GamePad gamepad;

            public static Input_ChatPad chatpad;

            public static Input_Keyboard Keyboard
            { get { return keyboard; } }

            public static Input_Mouse Mouse
            { get { return mouse; } }

            public static Input_GamePad GamePad
            { get { return gamepad; } }

            public static Input_ChatPad ChatPad
            { get { return chatpad; } }
        }

        public class Input_Mouse
        {
            #region Variable Declarations

            public MouseState previousMouseState;

            public MouseState currentMouseState;

            #endregion

            public Input_Mouse()
            { }

            public void Update()
            {
                previousMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();
            }

            #region Accessors

            public MouseState PreviousMouseState
            { get { return previousMouseState; } }

            public MouseState CurrentMouseState
            { get { return currentMouseState; } }

            public Vector2 PositionFloat
            {
                get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
            }

            public Point PositionInt
            {
                get { return new Point(currentMouseState.X, currentMouseState.Y); }
            }

            public int X
            {
                get { return currentMouseState.X; }
            }

            public int Y
            {
                get { return currentMouseState.Y; }
            }

            public int ScrollWheel
            {
                get { return currentMouseState.ScrollWheelValue; }
            }

            #endregion

            #region Functions

            public bool LeftMouseButtonDown()
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed)
                    return true;
                else
                    return false;
            }

            public bool LeftMouseButtonPressed()
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    return true;
                else
                    return false;
            }

            public bool RightMouseButtonDown()
            {
                if (currentMouseState.RightButton == ButtonState.Pressed)
                    return true;
                else
                    return false;
            }

            public bool RightMouseButtonPressed()
            {
                if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released)
                    return true;
                else
                    return false;
            }

            public bool MiddleMouseButtonDown()
            {
                if (currentMouseState.MiddleButton == ButtonState.Pressed)
                    return true;
                else
                    return false;
            }

            public bool MiddleMouseButtonPressed()
            {
                if (currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released)
                    return true;
                else
                    return false;
            }

            public bool X1ButtonDown()
            {
                if (currentMouseState.XButton1 == ButtonState.Pressed)
                    return true;
                else
                    return false;
            }

            public bool X1ButtonPressed()
            {
                if (currentMouseState.XButton1 == ButtonState.Pressed && previousMouseState.XButton1 == ButtonState.Released)
                    return true;
                else
                    return false;
            }

            public bool X2ButtonDown()
            {
                if (currentMouseState.XButton2 == ButtonState.Pressed)
                    return true;
                else
                    return false;
            }

            public bool X2ButtonPressed()
            {
                if (currentMouseState.XButton2 == ButtonState.Pressed && previousMouseState.XButton2 == ButtonState.Released)
                    return true;
                else
                    return false;
            }

            #endregion
        }

        public class Input_Keyboard
        {
            #region Variable Declarations

            public KeyboardState previousKeyboardState;

            public KeyboardState currentKeyboardState;

            #endregion

            public Input_Keyboard()
            { }

            public void Update()
            {
                previousKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();
            }

            #region Functions

            public bool IsKeyPressed(Keys key)
            {
                if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                    return true;
                else
                    return false;
            }

            public bool IsKeyDown(Keys key)
            {
                if (currentKeyboardState.IsKeyDown(key))
                    return true;
                else
                    return false;
            }

            public bool IsKeyUp(Keys key)
            {
                if (currentKeyboardState.IsKeyUp(key))
                    return true;
                else
                    return false;
            }

            public Keys[] CurrentPressedKeys()
            {
                return currentKeyboardState.GetPressedKeys();
            }

            public Keys[] PreviousPressedKeys()
            {
                return previousKeyboardState.GetPressedKeys();
            }

            #endregion

        }

        public class Input_GamePad
        {
            #region Variable Declarations

            public GamePadState[] previousGamePadStates;

            public GamePadState[] currentGamePadStates;

            #endregion

            #region Accessors

            public GamePadState[] PreviousGamepadStates
            { get { return previousGamePadStates; } }

            public GamePadState[] CurrentGamepadStates
            { get { return currentGamePadStates; } }

            #endregion

            public Input_GamePad()
            {
                previousGamePadStates = new GamePadState[4];
                currentGamePadStates = new GamePadState[4];
            }

            public void Update()
            {
                for (int i = 0; i < 4; i++)
                {
                    previousGamePadStates[i] = currentGamePadStates[i];
                    currentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);
                }
            }

            #region Player One Functions

            public bool ButtonDown(Buttons button)
            {
                if (currentGamePadStates[0].IsButtonDown(button))
                    return true;
                else
                    return false;
            }

            public bool IsButtonPressed(Buttons button)
            {
                if (currentGamePadStates[0].IsButtonDown(button) && previousGamePadStates[0].IsButtonUp(button))
                    return true;
                else
                    return false;
            }

            public bool IsButtonUp(Buttons button)
            {
                if (currentGamePadStates[0].IsButtonUp(button))
                    return true;
                else
                    return false;
            }

            public Vector2 LeftThumbstick()
            {
                return currentGamePadStates[0].ThumbSticks.Left;
            }

            public Vector2 RightThumbstick()
            {
                return currentGamePadStates[0].ThumbSticks.Right;
            }

            public double LeftThumbstickDirection()
            {
                return Math.Atan2(-currentGamePadStates[0].ThumbSticks.Left.Y, currentGamePadStates[0].ThumbSticks.Left.X) + MathHelper.PiOver2;
            }

            public double RightThumbstickDirection()
            {
                return Math.Atan2(-currentGamePadStates[0].ThumbSticks.Right.Y, currentGamePadStates[0].ThumbSticks.Right.X) + MathHelper.PiOver2;
            }

            #endregion

            #region Specific PlayerIndex Functions

            public bool IsButtonDown(PlayerIndex index, Buttons button)
            {
                if (currentGamePadStates[(int)index].IsButtonDown(button))
                    return true;
                else
                    return false;
            }

            public bool IsButtonPressed(PlayerIndex index, Buttons button)
            {
                if (currentGamePadStates[(int)index].IsButtonDown(button) && previousGamePadStates[(int)index].IsButtonUp(button))
                    return true;
                else
                    return false;
            }

            public bool IsButtonUp(PlayerIndex index, Buttons button)
            {
                if (currentGamePadStates[(int)index].IsButtonUp(button))
                    return true;
                else
                    return false;
            }

            public Vector2 LeftThumbstick(PlayerIndex index)
            {
                return currentGamePadStates[(int)index].ThumbSticks.Left;
            }

            public Vector2 RightThumbstick(PlayerIndex index)
            {
                return currentGamePadStates[(int)index].ThumbSticks.Right;
            }

            public double LeftThumbstickDirection(PlayerIndex index)
            {
                return Math.Atan2(-currentGamePadStates[(int)index].ThumbSticks.Left.Y, currentGamePadStates[(int)index].ThumbSticks.Left.X) + MathHelper.PiOver2;
            }

            public double RightThumbstickDirection(PlayerIndex index)
            {
                return Math.Atan2(-currentGamePadStates[(int)index].ThumbSticks.Right.Y, currentGamePadStates[(int)index].ThumbSticks.Right.X) + MathHelper.PiOver2;
            }

            public GamePadButtons GetPressedButtons(PlayerIndex index)
            {
                return currentGamePadStates[(int)index].Buttons;
            }

            #endregion

            #region Ranged Player Indexes Functions

            public bool IsButtonDown(PlayerIndex startingIndex, PlayerIndex endingIndex, Buttons button)
            {
                bool[] temp = new bool[4];
                for (int i = 0; i < temp.Length; i++)
                { temp[i] = false; }
                for (int i = (int)startingIndex; i < (int)endingIndex + 1; i++)
                {
                    if (currentGamePadStates[i].IsButtonDown(button))
                        temp[i] = true;
                    else
                        temp[i] = false;
                }
                for (int i = (int)startingIndex; i < (int)endingIndex; i++)
                {
                    if (!temp[i])
                        return false;
                }
                return true;
            }

            public bool IsButtonPressed(PlayerIndex startingIndex, PlayerIndex endingIndex, Buttons button)
            {
                bool[] temp = new bool[4];
                for (int i = 0; i < temp.Length; i++)
                { temp[i] = false; }
                for (int i = (int)startingIndex; i < (int)endingIndex + 1; i++)
                {
                    if (currentGamePadStates[i].IsButtonDown(button) && previousGamePadStates[i].IsButtonUp(button))
                        temp[i] = true;
                    else
                        temp[i] = false;
                }
                for (int i = (int)startingIndex; i < (int)endingIndex; i++)
                {
                    if (!temp[i])
                        return false;
                }
                return true;
            }

            public bool IsButtonUp(PlayerIndex startingIndex, PlayerIndex endingIndex, Buttons button)
            {
                bool[] temp = new bool[4];
                for (int i = 0; i < temp.Length; i++)
                { temp[i] = false; }
                for (int i = (int)startingIndex; i < (int)endingIndex + 1; i++)
                {
                    if (currentGamePadStates[i].IsButtonUp(button))
                        temp[i] = true;
                    else
                        temp[i] = false;
                }
                for (int i = (int)startingIndex; i < (int)endingIndex; i++)
                {
                    if (!temp[i])
                        return false;
                }
                return true;
            }

            public List<Vector2> LeftThumbstick(PlayerIndex startingIndex, PlayerIndex endingIndex)
            {
                List<Vector2> thumbsticks = new List<Vector2>();
                for (int i = (int)startingIndex; i < (int)endingIndex + 1; i++)
                {
                    thumbsticks.Add(currentGamePadStates[i].ThumbSticks.Left);
                }
                return thumbsticks;
            }

            public List<Vector2> RightThumbstick(PlayerIndex startingIndex, PlayerIndex endingIndex)
            {
                List<Vector2> thumbsticks = new List<Vector2>();
                for (int i = (int)startingIndex; i < (int)endingIndex + 1; i++)
                {
                    thumbsticks.Add(currentGamePadStates[i].ThumbSticks.Right);
                }
                return thumbsticks;
            }

            public List<double> LeftThumbstickDirection(PlayerIndex startingIndex, PlayerIndex endingIndex)
            {
                List<double> directions = new List<double>();
                for (int i = (int)startingIndex; i < (int)endingIndex + 1; i++)
                {
                    directions.Add(Math.Atan2(-currentGamePadStates[i].ThumbSticks.Left.Y, currentGamePadStates[i].ThumbSticks.Left.X));
                }
                return directions;
            }

            public List<double> RightThumbstickDirection(PlayerIndex startingIndex, PlayerIndex endingIndex)
            {
                List<double> directions = new List<double>();
                for (int i = (int)startingIndex; i < (int)endingIndex; i++)
                {
                    directions.Add(Math.Atan2(-currentGamePadStates[i].ThumbSticks.Right.Y, currentGamePadStates[i].ThumbSticks.Right.X));
                }
                return directions;
            }

            #endregion

        }

        public class Input_ChatPad
        {
            public KeyboardState[] previousKeyboardStates;

            public KeyboardState[] currentKeyboardStates;

            public KeyboardState[] PreviousKeyboardStates
            { get { return previousKeyboardStates; } }

            public KeyboardState[] CurrentKeyboardStates
            { get { return currentKeyboardStates; } }


            public Input_ChatPad()
            {
                previousKeyboardStates = new KeyboardState[4];
                currentKeyboardStates = new KeyboardState[4];
            }

            public void Update()
            {
                for (int i = 0; i < 4; i++)
                {
                    previousKeyboardStates[i] = currentKeyboardStates[i];
                    currentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                }
            }


            public bool IsKeyDown(Keys key)
            {
                if (currentKeyboardStates[0].IsKeyDown(key))
                    return true;
                else
                    return false;
            }

            public bool IsKeyPressed(Keys key)
            {
                if (currentKeyboardStates[0].IsKeyDown(key) && previousKeyboardStates[0].IsKeyUp(key))
                    return true;
                else
                    return false;
            }

            public bool IsKeyUp(Keys key)
            {
                if (currentKeyboardStates[0].IsKeyUp(key))
                    return true;
                else
                    return false;
            }

            public Keys[] CurrentPressedKeys()
            {
                return currentKeyboardStates[0].GetPressedKeys();
            }

            public Keys[] PreviousPressedKeys()
            {
                return previousKeyboardStates[0].GetPressedKeys();
            }


            public bool IsKeyDown(PlayerIndex index, Keys key)
            {
                if (currentKeyboardStates[(int)index].IsKeyDown(key))
                    return true;
                else
                    return false;
            }

            public bool IsKeyPressed(PlayerIndex index, Keys key)
            {
                if (currentKeyboardStates[(int)index].IsKeyDown(key) && previousKeyboardStates[(int)index].IsKeyUp(key))
                    return true;
                else
                    return false;
            }

            public bool IsKeyUp(PlayerIndex index, Keys key)
            {
                if (currentKeyboardStates[(int)index].IsKeyUp(key))
                    return true;
                else
                    return false;
            }

            public Keys[] CurrentPressedKeys(PlayerIndex index)
            {
                return currentKeyboardStates[(int)index].GetPressedKeys();
            }

            public Keys[] PreviousPressedKeys(PlayerIndex index)
            {
                return previousKeyboardStates[(int)index].GetPressedKeys();
            }


        }
    }
}
