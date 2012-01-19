using System;
using System.Collections.Generic;
using Microsoft.Research.Kinect.Nui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using XnaHelpers.GameEngine;
using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;
using XNATweener;

namespace AStarLearner
{
    public class MainGame : Game
    {
        
        public const int gameWidth = 640;
        public const int gameHeight = 480;

        //public const int gameWidth = 1280;
        //public const int gameHeight = 1024;

        public const bool isFullScreen = false;

        public Random rand = new Random((int)DateTime.Now.Ticks);

        private const int EdgeOffset = 10;
        private const int HotSpotSizes = 100;
        private const float HotSpotAlpha = 0.5f;
        private const int JointIntersectionSize = 80;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Runtime kinectRuntime;

        GameSFX correct_snd;

        ParticleEffect particleEffect;
        Renderer particleRenderer;

        private readonly GameTextureInstance kinectRGBVideo = new GameTextureInstance();
        private readonly List<GameTextureInstance> hotSpots = new List<GameTextureInstance>();
        private readonly List<GameTextureInstance> skeletonSpots = new List<GameTextureInstance>();
        private readonly List<GameTextureInstance> debugSpots = new List<GameTextureInstance>();

        // Each gameSet would have 1 Correct Solution Object and multiple selection Object
        // The correct solution object should be placed in the first index.
        private List<GameObject> currentGameSet = new List<GameObject>();
        
        //Stores a list of gameSet.
        private List<String> gameSetList = new List<String>();

        // We would have 6 possible MCQ choices, to be displayed. ie: 3 on the left and 3 on the right. 
        // this list stores the coordinates to determine where the selection object should be placed. 
        private List<Vector2> gameObjectPosition = new List<Vector2>();

        private GameObject solutionObjectReplica;

        /// <summary>
        /// This function would randomly select and set for us the correct solution. and 
        /// pick random "non correct" red - herrings. 
        /// </summary>
        public void initGameSetList()
        {
            gameSetList.Add("Set1"); gameSetList.Add("Set6");
            gameSetList.Add("Set2"); gameSetList.Add("Set7");
            gameSetList.Add("Set3"); gameSetList.Add("Set8");
            gameSetList.Add("Set4"); gameSetList.Add("Set9");
            gameSetList.Add("Set5"); gameSetList.Add("Set10");

        }

        public string getRandomGameSet()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            // Rand.Next picks lower bound(Inclusive) and Upper Bound Exclusive;
            int choice = rand.Next(0, gameSetList.Count);
            return gameSetList[choice];
        }

        /// <summary>
        /// Return an list of random numbers, that is guaranteed to have no repetition within the list
        /// itself 
        /// </summary>
        /// <param name="min">Min Number Range</param>
        /// <param name="max">Max Number Range</param>
        /// <returns>a list of ordered list of numbers</returns>
        public List<int> genRandomNumberList(int min, int max)
        {
          
            // Fill this list of random numbers, w/o repetitions
            List<int> choice = new List<int>();
            while (choice.Count != max)
            {
                int num = rand.Next(min, max);
                
                if (!choice.Contains(num))
                    choice.Add(num);
            }

            return choice;
        }

        // Remember to draw objects
        public List<GameObject> generateGameSet()
        {
            solutionObjectReplica = null;
            currentGameSet.Clear();
            gameObjectPosition.Clear();
            int xSpacing = 5;

            var set = Content.LoadContent<Texture2D>("Level1\\" + getRandomGameSet());
            
            List<string> contentName = new List<string>();
            foreach (string s in set.Keys)
                contentName.Add(s);

            List<int> contentRand = genRandomNumberList(0, contentName.Count);

            // Create solution object
            GameSprite solutionSprite = new GameSprite(set[contentName[contentRand[0]]], 1, 1, 0, 0);

            // Create selection objects
            GameSprite selectionSprite1 = new GameSprite(set[contentName[contentRand[1]]], 1, 1, 0, 0);
            GameSprite selectionSprite2 = new GameSprite(set[contentName[contentRand[2]]], 1, 1, 0, 0);
            GameSprite selectionSprite3 = new GameSprite(set[contentName[contentRand[3]]], 1, 1, 0, 0);
            // GameSprite selectionSprite4 = new GameSprite(set1["yellow_ball"]  , 1, 1, 0, 0);

            // Store positions. These are namely 2 on the left, 2 on the right
            gameObjectPosition.Add(new Vector2(0, xSpacing)); //top left 
            gameObjectPosition.Add(new Vector2(0, solutionSprite.Height * 4)); // bottom left 
            gameObjectPosition.Add(new Vector2(Window.ClientBounds.Width - solutionSprite.Width, 0)); // top right 
            gameObjectPosition.Add(new Vector2(Window.ClientBounds.Width - solutionSprite.Width, solutionSprite.Height * 4)); //btm right

            // Generate position placement choices randomly
            List<int> randPosn = new List<int>();
            
         
            while (randPosn.Count != gameObjectPosition.Count)
            {
                int choice = rand.Next(0, gameObjectPosition.Count);
                if (!randPosn.Contains(choice))
                    randPosn.Add(choice);
            }

            // Create solution object
            GameObject solutionObj = new GameObject(solutionSprite, gameObjectPosition[randPosn[0]]);
            solutionObj.isSolutionObject = true;
            
            // Create a replica of the solution object for display purposes only 
            solutionObjectReplica = new GameObject(solutionSprite, Window.ClientBounds.Width / 2, 0);

            // left hand side
            GameObject selectionObject1 = new GameObject(selectionSprite1, gameObjectPosition[randPosn[1]]);
            GameObject selectionObject2 = new GameObject(selectionSprite2, gameObjectPosition[randPosn[2]]);

            // right hand side 
            GameObject selectionObject3 = new GameObject(selectionSprite3, gameObjectPosition[randPosn[3]]);

            // Add to current game set 
            currentGameSet.Add(solutionObj); //Solution is always the first
            currentGameSet.Add(selectionObject1);
            currentGameSet.Add(selectionObject2);
            currentGameSet.Add(selectionObject3);             

            return currentGameSet;
        }


        // place this in the main game loop
        public void gameLogic(JointsCollection joints)
        {
   
            
            foreach (Joint joint in joints)
            {
               
                if (joint.ID == JointID.HandRight)
                {
                    // Place solution object replica on the person's hand
                    Vector2 jointPosition = joint.GetScreenPosition(kinectRuntime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    //solutionObjectReplica.Position = jointPosition;
                    
                    if (checkSolutionIntersection(jointPosition))
                    {
                        correctChoice(jointPosition);
                    }
                    else
                    {
                        wrongChoice();
                    }

                }

                if (joint.ID == JointID.HandLeft)
                {
                    // Place solution object replica on the person's hand
                    Vector2 jointPosition = joint.GetScreenPosition(kinectRuntime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    //solutionObjectReplica.Position = jointPosition;

                    if (checkSolutionIntersection(jointPosition))
                    {
                        particleEffect.Trigger(jointPosition);
                        correctChoice(jointPosition);
                    }
                    else
                    {
                        wrongChoice();
                    }

                }

                
            }

        }

        private bool checkSolutionIntersection(Vector2 jointPosition)
        {
          
            Rectangle rectangle = new Rectangle((int)jointPosition.X - (JointIntersectionSize / 2),
                                   (int)jointPosition.Y - (JointIntersectionSize / 2), JointIntersectionSize, JointIntersectionSize);

            return currentGameSet[0].Collision(rectangle);            
  
        }

        private void correctChoice(Vector2 pos)
        {
            generateGameSet();
            particleEffect.Trigger(pos);
            correct_snd.MultiPlay();           
        }

        private void wrongChoice()
        {
            // play sound. 
        }

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;

            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            this.graphics.IsFullScreen = isFullScreen;

            Content.RootDirectory = "Content";

           /* particleEffect = new ParticleEffect
            {

                new Emitter 
                {
                    Budget = 1000,
                    Term = 3f,

                    Name = "FirstEmitter",
                    BlendMode = EmitterBlendMode.Alpha,
                    ReleaseQuantity = 100,
                    ReleaseRotation = new VariableFloat {Value = 0f, Variation = MathHelper.Pi},
                    ReleaseScale = 64f,
                    ReleaseSpeed = new VariableFloat { Value = 64f , Variation = 32f },
                    ParticleTextureAssetName = "particle1",

                    Modifiers = new ModifierCollection
                    {
                        new OpacityModifier
                        {

                            Initial= 1f,
                            Ultimate  = 0f, 
                        },

                         new ColourModifier
                         {
                             InitialColour = Color.Tomato.ToVector3(),
                             UltimateColour = Color.LimeGreen.ToVector3(),
                         },
        
                    },


                },
            }; */

            particleEffect = new ParticleEffect();
            particleRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };
            
        }

        protected override void Initialize()
        {
            kinectRuntime = new Runtime();
            kinectRuntime.Initialize(RuntimeOptions.UseColor | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseDepthAndPlayerIndex);
            kinectRuntime.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);

            kinectRuntime.VideoFrameReady += VideoFrameReady;
            kinectRuntime.SkeletonFrameReady += SkeletonFrameReady;
            
            particleEffect.Initialise();

            base.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            kinectRuntime.Uninitialize();
            base.OnExiting(sender, args);
        }

        protected override void LoadContent()
        {
            // Rendering inits. 
            spriteBatch = new SpriteBatch(GraphicsDevice);
            kinectRGBVideo.Texture = new Texture2D(GraphicsDevice, gameWidth, gameHeight, false, SurfaceFormat.Color);
            
            // Game Set inits.
            initGameSetList();
            generateGameSet();
            
            // Load sounds 
            correct_snd = new GameSFX(Content.Load<SoundEffect>("kling"));

            //Loads the background music and plays it
            GameMusic background = new GameMusic(Content.Load<Song>("background_music3"));
            background.PlayLooping();

            particleRenderer.LoadContent(Content);

            particleEffect = Content.Load<ParticleEffect>(("BasicExplosion"));
            particleEffect.LoadContent(Content);
            particleEffect.Initialise();

            /*
            //skeleton right hand
            GameTextureInstance texture = GameTextureInstance.CreateBlank(GraphicsDevice, 20, 20);
            texture.Position = new Vector2(0, 0);
            texture.Alpha = 1;
            texture.Color = Color.Orange;
            skeletonSpots.Add(texture);

            //Left hand
            texture = GameTextureInstance.CreateBlank(GraphicsDevice, 20, 20);
            texture.Position = new Vector2(0, 0);
            texture.Alpha = 1;
            texture.Color = Color.BlueViolet;
            skeletonSpots.Add(texture);

            //hotspots
            texture = GameTextureInstance.CreateBlank(GraphicsDevice, HotSpotSizes, HotSpotSizes);
            texture.Position = new Vector2(EdgeOffset);
            texture.Alpha = HotSpotAlpha;
            hotSpots.Add(texture);

            texture = GameTextureInstance.CreateBlank(GraphicsDevice, HotSpotSizes, HotSpotSizes);
            texture.Position = new Vector2(GraphicsDevice.Viewport.Width - texture.Texture.Width - EdgeOffset, EdgeOffset);
            texture.Alpha = HotSpotAlpha;
            hotSpots.Add(texture);

            texture = GameTextureInstance.CreateBlank(GraphicsDevice, HotSpotSizes, HotSpotSizes);
            texture.Position = new Vector2(10, GraphicsDevice.Viewport.Height - texture.Texture.Height - EdgeOffset);
            texture.Alpha = HotSpotAlpha;
            hotSpots.Add(texture);

            texture = GameTextureInstance.CreateBlank(GraphicsDevice, HotSpotSizes, HotSpotSizes);
            texture.Position = new Vector2(GraphicsDevice.Viewport.Width - texture.Texture.Width - EdgeOffset, GraphicsDevice.Viewport.Height - texture.Texture.Height - EdgeOffset);
            texture.Alpha = HotSpotAlpha;
            hotSpots.Add(texture);

            //debug textures
            GameTextureInstance rectTexture = GameTextureInstance.CreateBlank(GraphicsDevice, 80, 80);
            rectTexture.Alpha = 0.3f;
            rectTexture.Color = Color.HotPink;
            debugSpots.Add(rectTexture);

            rectTexture = GameTextureInstance.CreateBlank(GraphicsDevice, 80, 80);
            rectTexture.Alpha = 0.3f;
            rectTexture.Color = Color.HotPink;
            debugSpots.Add(rectTexture);
 

            ResetSquareColors();
*/
        }

        private void ResetSquareColors()
        {
            foreach (GameTextureInstance texture in hotSpots)
                texture.Color = Color.Red;
        }

        private void VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            PlanarImage image = e.ImageFrame.Image;
            kinectRGBVideo.Texture = image.ToTexture2D(GraphicsDevice);
        }

        private void SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {

            SkeletonFrame skeletonFrame = e.SkeletonFrame;
            ResetSquareColors();
      
            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (data.TrackingState == SkeletonTrackingState.Tracked)
                {
                    gameLogic(data.Joints);
                }
            }
        }
        protected override void Update(GameTime gameTime)
        {

            float SecondsPassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            particleEffect.Update(SecondsPassed);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            kinectRGBVideo.Draw(spriteBatch);
            spriteBatch.End();

            particleRenderer.RenderEffect(particleEffect);

            foreach (GameObject g in currentGameSet)
                g.Draw(spriteBatch);

            solutionObjectReplica.Draw(spriteBatch);

            /*
                        foreach (GameTextureInstance texture in hotSpots)
                            texture.Draw(spriteBatch);

                        foreach (GameTextureInstance texture in skeletonSpots)
                            texture.Draw(spriteBatch);

                        foreach (GameTextureInstance texture in debugSpots)
                            texture.Draw(spriteBatch);


                        spriteBatch.End(); */

            base.Draw(gameTime);
        }
    }
}
