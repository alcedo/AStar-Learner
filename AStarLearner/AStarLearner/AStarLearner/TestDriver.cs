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
using XnaHelpers.InputFrameWork;
using XnaHelpers.GameEngine;

namespace AStarLearner
{
    class VictorTestDriver : Microsoft.Xna.Framework.Game
    {
        // Core XNA Components 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputComponent inputManager;

        // In Game variables. 
        //The sprites for all the Game objects for the game. 
        GameSprite squareSprite;
        GameSprite planeSprite;
        GameSprite bulletSprite;
        
        //Game Objects
        GameObject planeObject;
        GameObjectSpawner bullets;
        GameObjectSpawner enemies;

       
        public VictorTestDriver()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {




            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {   // Note: use this.Content to load your game contents here

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create needed input components 
            inputManager = new InputComponent(this, true, true, false, false);

            //Loads all the sprites
            planeSprite = new GameSprite(Content.Load<Texture2D>("plane"), 1, 3, 0, 0.5);

            //Create Game objects
             planeObject = new GameObject(planeSprite, Window.ClientBounds.Width / 2 - planeSprite.Width / 2,
                 Window.ClientBounds.Height - planeSprite.Height);

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Moves the plane based on keyboard input
            planeObject.Velocity = GameInput.Windows.GetDirectionalVector() * 2;

            //Updates the plane
            planeObject.Update(true, Window);
           
            // Console.WriteLine("X is: " + Input.mouse.X);
            inputManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            planeObject.Draw(spriteBatch);


            base.Draw(gameTime);
        }
    }
}
