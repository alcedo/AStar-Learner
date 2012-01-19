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
    /// Used in game Object. Once GameSprite animation ends, it would tell GameObject
    /// that the animation has ended. GameObject can then choose to decide whether to kill 
    /// itself or do whatever it needs.
    public delegate void AnimationEndedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// A basic animated image
    /// </summary>
    public class GameSprite
    {
        #region Variable Declarations
        //The sprite sheet
        Texture2D image;
        //Sprite sheet information
        int rows, columns, totalFrames;
        //Animation information
        /// <summary>
        /// The current frame of the animation
        /// </summary>
        public int CurrentFrame;
        /// <summary>
        /// The width of the GameSprite
        /// </summary>
        public readonly int Width;
        /// <summary>
        /// The height of the GameSprite
        /// </summary>
        public readonly int Height;
        /// <summary>
        /// How much to scale the sprite
        /// </summary>
        public Vector2 Scale;
        //Animation playback information
        bool paused;
        /// <summary>
        /// The speed of the animation
        /// </summary>
        public double AnimationSpeed;
        /// <summary>
        /// Whether to loop the animation
        /// </summary>
        public bool Looping = true;
        /// <summary>
        /// Raised when the animation ends
        /// </summary>
        double currentSpeedVar = 0;
        
        /// <summary>
        /// This event is raised when the sprite animation ends
        /// </summary>
        public event AnimationEndedEventHandler AnimationEnded;

        #endregion

        #region Constructors

                
        /// <summary>
        /// Initializes a new GameSprite
        /// </summary>
        /// <param name="GameSpriteSheet">The sprite sheet containing the frames of the animation</param>
        /// <param name="sheetRows">The number of rows the sprite sheet has</param>
        /// <param name="sheetColumns">The number of columns the sprite sheet has</param>
        public GameSprite(Texture2D GameSpriteSheet, int sheetRows, int sheetColumns)
        {
            //Sets the sprite information
            image = GameSpriteSheet;
            rows = sheetRows;
            columns = sheetColumns;
            totalFrames = rows * columns;
            Width = image.Width / columns;
            Height = image.Height / rows;
            paused = false;
            AnimationSpeed = 1;
            Scale = new Vector2(1, 1);
        }

        /// <summary>
        /// Initializes a new GameSprite
        /// </summary>
        /// <param name="GameSpriteSheet">The sprite sheet containing the frames of the animation</param>
        /// <param name="sheetRows">The number of rows the sprite sheet has</param>
        /// <param name="sheetColumns">The number of columns the sprite sheet has</param>
        /// <param name="startingFrame">The frame of the animation to start with</param>
        /// <param name="imageSpeed">The speed of the animation</param>
        public GameSprite(Texture2D GameSpriteSheet, int sheetRows, int sheetColumns, int startingFrame, double imageSpeed)
        {
            //Sets the sprite animation
            image = GameSpriteSheet;
            rows = sheetRows;
            columns = sheetColumns;
            totalFrames = rows * columns;
            Width = image.Width / columns;
            Height = image.Height / rows;
            paused = false;
            CurrentFrame = startingFrame;
            AnimationSpeed = (double)MathHelper.Clamp((float)imageSpeed, 0, 1);
            Scale = new Vector2(1, 1);
        }

        /// <summary>
        /// Initializes a new GameSprite
        /// </summary>
        /// <param name="GameSpriteSheet">The sprite sheet containing the frames of the animation</param>
        /// <param name="sheetRows">The number of rows the sprite sheet has</param>
        /// <param name="sheetColumns">The number of columns the sprite sheet has</param>
        /// <param name="startingFrame">The frame of the animation to start with</param>
        /// <param name="imageSpeed">The speed of the animation</param>
        /// <param name="scaleX">The initial X scaling</param>
        /// <param name="scaleY">The initial Y scaling</param>
        public GameSprite(Texture2D GameSpriteSheet, int sheetRows, int sheetColumns, int startingFrame, double imageSpeed, int scaleX, int scaleY)
        {
            //Sets the sprite animation
            image = GameSpriteSheet;
            rows = sheetRows;
            columns = sheetColumns;
            totalFrames = rows * columns;
            Width = image.Width / columns;
            Height = image.Height / rows;
            paused = false;
            CurrentFrame = startingFrame;
            AnimationSpeed = (double)MathHelper.Clamp((float)imageSpeed, 0, 1);
            Scale = new Vector2(scaleX, scaleY);
        }

        /// <summary>
        /// Initializes a new GameSprite
        /// </summary>
        /// <param name="gameSprite">The GameSprite to clone from</param>
        public GameSprite(GameSprite gameSprite)
        {
            //Sets the sprite animation
            image = gameSprite.image;
            rows = gameSprite.rows;
            columns = gameSprite.columns;
            totalFrames = rows * columns;
            Width = image.Width / columns;
            Height = image.Height / rows;
            paused = gameSprite.paused;
            AnimationSpeed = gameSprite.AnimationSpeed;
            Scale = gameSprite.Scale;
            CurrentFrame = 0;
            Looping = gameSprite.Looping;
        }
        #endregion

        #region Playback Methods
        /// <summary>
        /// Updates the GameSprite animation
        /// </summary>
        public void Update()
        {
            //If the sprite animation is unpaused, updates the current frame based on the speed
            if (paused == false)
            {
                if (currentSpeedVar >= 1)
                {
                    CurrentFrame++;
                    currentSpeedVar = (1 - currentSpeedVar) + AnimationSpeed;
                }
                else
                {
                    currentSpeedVar += AnimationSpeed;
                }

                //Raises AnimationEnded if the animation has finished
                if (CurrentFrame == totalFrames)
                {
                    if (Looping == true)
                    {
                        CurrentFrame = 0;
                    }
                    AnimationEnded(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Pauses the GameSprite animation if it is playing
        /// </summary>
        public void Pause()
        {
            paused = true;
        }

        /// <summary>
        /// Plays the GameSprite animation if it is paused
        /// </summary>
        public void Play()
        {
            paused = false;
        }
        #endregion

        #region Draw Methods
        /// <summary>
        /// Draws the GameSprite at the desired location
        /// </summary>
        /// <param name="spriteBatch">The GameSprite batch to draw the GameSprite with</param>
        /// <param name="position">The position to draw the GameSprite</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, (int)(Width * Scale.X), (int)(Height * Scale.Y)), new Rectangle(
                (CurrentFrame % columns) * Width,
                (CurrentFrame / columns) * Height,
                Width, Height), Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the GameSprite at the desired location
        /// </summary>
        /// <param name="spriteBatch">The GameSprite batch to draw the GameSprite with</param>
        /// <param name="position">The position to draw the GameSprite</param>
        /// <param name="blendColor">The color to blend with the GameSprite</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color blendColor)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, (int)(Width * Scale.X), (int)(Height * Scale.Y)), new Rectangle(
                (CurrentFrame % columns) * Width,
                (CurrentFrame / columns) * Height,
                Width, Height), blendColor);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the GameSprite at the desired location
        /// </summary>
        /// <param name="spriteBatch">The GameSprite batch to draw the GameSprite with</param>
        /// <param name="x">The X position at which to put the GameSprite</param>
        /// <param name="y">The Y position at which to put the GameSprite</param>
        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, new Rectangle(x, y, (int)(Width * Scale.X), (int)(Height * Scale.Y)), new Rectangle(
                (CurrentFrame % columns) * Width,
                (CurrentFrame / columns) * Height,
                Width, Height), Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the GameSprite at the desired location
        /// </summary>
        /// <param name="spriteBatch">The GameSprite batch to draw the GameSprite with</param>
        /// <param name="x">The X position at which to put the GameSprite</param>
        /// <param name="y">The Y position at which to put the GameSprite</param>
        /// <param name="blendColor">The color to blend with the GameSprite</param>
        public void Draw(SpriteBatch spriteBatch, int x, int y, Color blendColor)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, new Rectangle(x, y, (int)(Width * Scale.X), (int)(Height * Scale.Y)), new Rectangle(
                (CurrentFrame % columns) * Width,
                (CurrentFrame / columns) * Height,
                Width, Height), blendColor);
            spriteBatch.End();
        }
        #endregion
    }


}
