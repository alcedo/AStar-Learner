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
    /// Contains math methods and values as well as additional conversion methods
    /// </summary>
    public class GameMath
    {
        #region Int <-> Bool Conversion
        /// <summary>
        /// Converts a boolean value to an integer
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns></returns>
        public static int BoolToInt(bool value)
        {
            //Returns true if 1 and false if anything else
            if (value == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Converts an integer to a boolean value
        /// </summary>
        /// <param name="value">The integer to convert</param>
        /// <returns></returns>
        public static bool IntToBool(int value)
        {
            //Returns true if 1 and false if anything else
            if (value != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Point Methods
        /// <summary>
        /// Gets the distance between two points
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns></returns>
        public static double GetDistance(Vector2 point1, Vector2 point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.X - point2.X, 2));
        }
        #endregion
    }
}
