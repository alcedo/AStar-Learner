using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaHelpers.GameEngine
{
    public class GameTextureInstance
    {
        private Texture2D texture;
        private bool visible = true;

        private Vector2 origin = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private Vector2 scale = Vector2.One;
        private float rotation;

        private Rectangle? destinationRectangle;
        private Rectangle? sourceRectangle;

        private SpriteEffects spriteEffects = SpriteEffects.None;
        private Color color = Color.White;
        private float alpha = 1f;
        private float layerDepth;

        public static GameTextureInstance CreateBlank(GraphicsDevice graphicsDevice, int width, int height)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < colorData.Length; i++)
                colorData[i] = Color.White;

            texture.SetData(colorData);

            GameTextureInstance instance = new GameTextureInstance
            {
                texture = texture
            };

            return instance;
        }

        public Rectangle CalculateBoundingRectangle()
        {
            return new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null && visible)
            {
                if (!destinationRectangle.HasValue)
                    spriteBatch.Draw(texture, position, sourceRectangle, color * alpha, rotation, origin, scale, spriteEffects, layerDepth);
                else
                    spriteBatch.Draw(texture, destinationRectangle.Value, sourceRectangle, color * alpha, rotation, origin, spriteEffects, layerDepth);
            }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Rectangle? DestinationRectangle
        {
            get { return destinationRectangle; }
            set { destinationRectangle = value; }
        }

        public Rectangle? SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }
    }
}
