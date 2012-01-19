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
    public class GameObject
    {
        #region Variable Declarations
        /// <summary>
        /// The GameSprite being used with this GameObject
        /// </summary>
        public GameSprite objectSprite;
        /// <summary>
        /// The current position
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// The current velocity
        /// </summary>
        public Vector2 Velocity;
        /// <summary>
        /// The current gravity towards the bottom of the screen
        /// </summary>
        public float Gravity = 0;
        /// <summary>
        /// The Rectangle that is used for collisions
        /// </summary>
        public Rectangle boundingRect;

        public bool isSolutionObject = false;

        /// <summary>
        /// Whether to destroy the object when the animation ends
        /// </summary>
        public bool DestroyOnAnimationEnded = false;
        /*Whether the object is dead. If this is set, the object is most likely at the position
        (-objectSprite.Width, -objectSprite.Height).*/
        bool dead = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new GameObject
        /// </summary>
        /// <param name="gameSprite">The GameSprite to use</param>
        /// <param name="startingPosition">The initial position for the GameObject</param>
        public GameObject(GameSprite gameSprite, Vector2 startingPosition)
        {
            //Sets the required GameObject values
            objectSprite = new GameSprite(gameSprite);
            Position = startingPosition;
            boundingRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(objectSprite.Width * objectSprite.Scale.X), (int)(objectSprite.Height * objectSprite.Scale.Y));
            objectSprite.AnimationEnded += new AnimationEndedEventHandler(objectSprite_AnimationEnded);
        }

        //The method to call when the sprite animation has ended
        void objectSprite_AnimationEnded(object sender, EventArgs e)
        {
            //Kill the object if it is set to destroy when the animation ends
            if (DestroyOnAnimationEnded == true)
            {
                this.Kill();
            }
        }

        /// <summary>
        /// Initializes a new GameObject
        /// </summary>
        /// <param name="gameSprite">The GameSprite to use</param>
        /// <param name="startingPosition">The initial position for the GameObject</param>
        /// <param name="startingVelocity">The initial Velocity for the GameObject</param>
        public GameObject(GameSprite gameSprite, Vector2 startingPosition, Vector2 startingVelocity)
        {
            //Sets the required GameObject values
            objectSprite = new GameSprite(gameSprite);
            Position = startingPosition;
            Velocity = startingVelocity;
            boundingRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(objectSprite.Width * objectSprite.Scale.X), (int)(objectSprite.Height * objectSprite.Scale.Y));
            objectSprite.AnimationEnded += new AnimationEndedEventHandler(objectSprite_AnimationEnded);
        }

        /// <summary>
        /// Initializes a new GameObject
        /// </summary>
        /// <param name="gameSprite">The GameSprite to use</param>
        /// <param name="startingPositionX">The initial X position for the GameObject</param>
        /// <param name="startingPositionY">The initial Y position for the GameObject</param>
        public GameObject(GameSprite gameSprite, int startingPositionX, int startingPositionY)
        {
            //Sets the required GameObject values
            objectSprite = new GameSprite(gameSprite);
            Position.X = startingPositionX;
            Position.Y = startingPositionY;
            boundingRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(objectSprite.Width * objectSprite.Scale.X), (int)(objectSprite.Height * objectSprite.Scale.Y));
            objectSprite.AnimationEnded += new AnimationEndedEventHandler(objectSprite_AnimationEnded);
        }

        /// <summary>
        /// Initializes a new GameObject
        /// </summary>
        /// <param name="gameSprite">The GameSprite to use</param>
        /// <param name="startingPositionX">The initial X position for the GameObject</param>
        /// <param name="startingPositionY">The initial Y position for the GameObject</param>
        /// <param name="startingVelocityX">The initial X Velocity for the GameObject</param>
        /// <param name="startingVelocityY">The initial Y Velocity for the GameObject</param>
        public GameObject(GameSprite gameSprite, int startingPositionX, int startingPositionY, int startingVelocityX, int startingVelocityY)
        {
            //Sets the required GameObject values
            objectSprite = new GameSprite(gameSprite);
            Position.X = startingPositionX;
            Position.Y = startingPositionY;
            Velocity.X = startingVelocityX;
            Velocity.Y = startingVelocityY;
            boundingRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(objectSprite.Width * objectSprite.Scale.X), (int)(objectSprite.Height * objectSprite.Scale.Y));
            objectSprite.AnimationEnded += new AnimationEndedEventHandler(objectSprite_AnimationEnded);
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the GameObject as well as the GameSprite associated with it
        /// </summary>
        public void Update()
        {
            //Updates all object components if the object isn't dead
            if (IsDead() == false)
            {
                objectSprite.Update();
                Velocity.Y += Gravity;
                Position += Velocity;
                boundingRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(objectSprite.Width * objectSprite.Scale.X), (int)(objectSprite.Height * objectSprite.Scale.Y));
            }
        }

        /// <summary>
        /// Updates the GameObject as well as the GameSprite associated with it
        /// </summary>
        public void Update(bool keepOnScreen, GameWindow window)
        {
            //Updates all object components and keeps the object onscreen
            if (IsDead() == false)
            {
                objectSprite.Update();
                Velocity.Y += Gravity;
                Position += Velocity;
                if (keepOnScreen == true)
                {
                    /*Makes sure that no part of the object goes outside the view.
                     * If they do, move the objects back inside the view*/
                    if (Position.X < 0)
                    {
                        Position.X = 0;
                    }
                    if (Position.Y < 0)
                    {
                        Position.Y = 0;
                    }
                    if (Position.X + objectSprite.Width * objectSprite.Scale.X > window.ClientBounds.Width)
                    {
                        Position.X = window.ClientBounds.Width - objectSprite.Width * objectSprite.Scale.X;
                    }
                    if (Position.Y + objectSprite.Height * objectSprite.Scale.Y > window.ClientBounds.Height)
                    {
                        Position.Y = window.ClientBounds.Height - objectSprite.Height * objectSprite.Scale.Y;
                    }
                }
                boundingRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(objectSprite.Width * objectSprite.Scale.X), (int)(objectSprite.Height * objectSprite.Scale.Y));
            }
        }
        #endregion

        #region Drawing Methods
        /// <summary>
        /// Draws the associated GameSprite at the current position
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw the sprite with</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the sprite if the object isn't dead
            if (IsDead() == false)
            {
                objectSprite.Draw(spriteBatch, Position);
            }
        }

        /// <summary>
        /// Draws the associated GameSprite at the current position with the specified blending color
        /// </summary>
        /// <param name="spriteBatch">The sprite batch the draw the sprite with</param>
        /// <param name="blendColor">The color to blend with the sprite</param>
        public void Draw(SpriteBatch spriteBatch, Color blendColor)
        {
            //Draws the sprite if the object isn't dead with the specified blend color
            if (IsDead() == false)
            {
                objectSprite.Draw(spriteBatch, Position, blendColor);
            }
        }
        #endregion

        #region Collision Method
        /// <summary>
        /// Checks for a collision with the specified GameObject
        /// </summary>
        /// <param name="otherObject">The object to check</param>
        /// <returns>Returns null if he object is dead</returns>
        public bool Collision(GameObject otherObject)
        {
            //Checks for a collision between this object and the other specified object
            if (IsDead() == false && otherObject.IsDead() == false)
            {
                return boundingRect.Intersects(otherObject.boundingRect);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks for a collision with another Rectangle Object
        /// </summary>
        /// <param name="rect">Rectangle object to check collision with</param>
        /// <returns>Returns true if collides</returns>
        public bool Collision(Rectangle rect)
        {
            return boundingRect.Intersects(rect);
        }

        #endregion

        #region Kill and Revive Methods
        /// <summary>
        /// Kills the GameObject
        /// </summary>
        public void Kill()
        {
            //Sets the default "dead position" and kills the object
            Position = new Vector2(-objectSprite.Width, -objectSprite.Height);
            dead = true;
        }

        /// <summary>
        /// Revives the GameObject
        /// </summary>
        public void Revive()
        {
            //Revives the object(this doesn't move the object)
            dead = false;
        }

        /// <summary>
        /// Checks if the object is dead
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            //Checks whether the object is dead
            return dead;
        }
        #endregion
    }

    public class GameObjectSpawner
    {
        #region Variable Declarations
        /// <summary>
        /// All the GameObjects spawned by this spawner
        /// </summary>
        public List<GameObject> Objects;

        /// <summary>
        /// The spawn delay in frames
        /// </summary>
        public int Delay = 0;
        /// <summary>
        /// The maximum number of GameObjects that can be spawned. Use null for an infinate number of GameObjects.
        /// </summary>
        public int? MaxObjects = null;
        /// <summary>
        /// The GameSprite to use for every new spawned GameObject
        /// </summary>
        public GameSprite SpawnerSprite;
        public bool DestroyOnAnimationEnded = false;
        //The timer that cleans the Objects list
        GameTimer cleanTimer = new GameTimer(30);
        //The delay timing value that updates every frame
        int currentDelay = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new GameObjectSpawner
        /// </summary>
        /// <param name="sprite">The sprite to use for each GameObject</param>
        public GameObjectSpawner(GameSprite sprite)
        {
            SpawnerSprite = sprite;
            cleanTimer.TimerFinished += new TimerFinishedEventHandler(cleanTimer_TimerFinished);
            Objects = new List<GameObject>();
        }

        void cleanTimer_TimerFinished(object sender, EventArgs e)
        {
            CleanObjectList();
            cleanTimer.Paused = false;
        }

        /// <summary>
        /// Initializes a new GameObjectSpawner
        /// </summary>
        /// <param name="sprite">The sprite to use for each GameObject</param>
        /// <param name="maxObjects">The maximum number of objects that can be spawned</param>
        public GameObjectSpawner(GameSprite sprite, int maxObjects)
        {
            //Sets the required components
            MaxObjects = maxObjects;
            SpawnerSprite = sprite;
            cleanTimer.TimerFinished += new TimerFinishedEventHandler(cleanTimer_TimerFinished);
            Objects = new List<GameObject>();
        }
        #endregion

        #region Draw Method
        /// <summary>
        /// Draws each GameObject at it's position
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw the sprite with</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Calls the Draw method for each of the child objects
            foreach (GameObject gameObject in Objects)
            {
                gameObject.Draw(spriteBatch);
            }
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Updates each GameObject and it's associated GameSprite
        /// </summary>
        public void Update()
        {
            //Calls the Update method for each of the child objects
            foreach (GameObject gameObject in Objects)
            {
                gameObject.Update();
            }
            cleanTimer.Update();
        }

        /// <summary>
        /// Updates each GameObject and it's associated GameSprite and keeps them onscreen
        /// </summary>
        /// <param name="killIfOffscreen">Whether to automatically kill the object if it's offscreen</param>
        /// <param name="window">The GameWindow associated with the game</param>
        public void Update(bool killIfOffscreen, GameWindow window)
        {
            //Calls the Update method for each of the child objects with the keep on screen option set to true
            foreach (GameObject gameObject in Objects)
            {
                gameObject.Update();

                if (!gameObject.boundingRect.Intersects(new Rectangle(0, 0, window.ClientBounds.Width, window.ClientBounds.Height)))
                {
                    gameObject.Kill();
                }
            }
            if (currentDelay < Delay)
            {
                currentDelay++;
            }
            cleanTimer.Update();
        }
        #endregion

        #region Spawning Methods
        /// <summary>
        /// Spawns a new object
        /// </summary>
        /// <param name="position">The position to spawn at</param>
        public void SpawnObject(Vector2 position)
        {
            int numberAlive = 0;
            foreach (GameObject gameObject in Objects)
            {
                if (!gameObject.IsDead())
                {
                    numberAlive++;
                }
            }
            if (MaxObjects != null)
            {
                if (numberAlive < MaxObjects)
                {
                    Objects.Add(new GameObject(SpawnerSprite, position));
                    Objects.Last().DestroyOnAnimationEnded = DestroyOnAnimationEnded;
                }
            }
        }

        /// <summary>
        /// Spawns a new object
        /// </summary>
        /// <param name="positionX">The x position to spawn at</param>
        /// <param name="positionY">The y position to spawn at</param>
        public void SpawnObject(int positionX, int positionY)
        {
            int numberAlive = 0;
            foreach (GameObject gameObject in Objects)
            {
                if (!gameObject.IsDead())
                {
                    numberAlive++;
                }
            }
            if (MaxObjects != null)
            {
                if (numberAlive < MaxObjects)
                {
                    Objects.Add(new GameObject(SpawnerSprite, positionX, positionY));
                    Objects.Last().DestroyOnAnimationEnded = DestroyOnAnimationEnded;
                }
            }
        }

        /// <summary>
        /// Spawns a new object
        /// </summary>
        /// <param name="position">The position to spawn at</param>
        /// <param name="velocity">The initial velocity to spawn with</param>
        public void SpawnObject(Vector2 position, Vector2 velocity)
        {
            int numberAlive = 0;
            foreach (GameObject gameObject in Objects)
            {
                if (!gameObject.IsDead())
                {
                    numberAlive++;
                }
            }
            if (MaxObjects != null)
            {
                if (numberAlive < MaxObjects)
                {
                    Objects.Add(new GameObject(SpawnerSprite, position, velocity));
                    Objects.Last().DestroyOnAnimationEnded = DestroyOnAnimationEnded;
                }
            }
        }

        /// <summary>
        /// Spawns a new object
        /// </summary>
        /// <param name="positionX">The x position to spawn at</param>
        /// <param name="positionY">The y position to spawn at</param>
        /// <param name="velocityX">The initial x velocity to spawn with</param>
        /// <param name="velocityY">The initial y velocity to spawn with</param>
        public void SpawnObject(int positionX, int positionY, int velocityX, int velocityY)
        {
            int numberAlive = 0;
            foreach (GameObject gameObject in Objects)
            {
                if (!gameObject.IsDead())
                {
                    numberAlive++;
                }
            }
            if (MaxObjects != null)
            {
                if (numberAlive < MaxObjects)
                {
                    Objects.Add(new GameObject(SpawnerSprite, positionX, positionY, velocityX, velocityY));
                    Objects.Last().DestroyOnAnimationEnded = DestroyOnAnimationEnded;
                }
            }
        }

        /// <summary>
        /// Cleans the Objects list of any dead GameObjects
        /// </summary>
        public void CleanObjectList()
        {
            for (int i = 1; i < Objects.Count; i++)
            {
                if (Objects[i].IsDead())
                {
                    Objects.RemoveAt(i);
                }
            }
        }
        #endregion
    }
}
