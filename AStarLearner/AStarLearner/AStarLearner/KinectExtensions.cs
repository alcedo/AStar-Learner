using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Research.Kinect.Nui
{
    public static class KinectExtensions
    {
        public static Texture2D ToTexture2D(this PlanarImage image, GraphicsDevice graphicsDevice)
        {
            Texture2D texture = new Texture2D(graphicsDevice, image.Width, image.Height, false, SurfaceFormat.Color);
            Color[] colorData = new Color[image.Width * image.Height];

            int index = 0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++, index += image.BytesPerPixel)
                    colorData[y * image.Width + x] = new Color(image.Bits[index + 2], image.Bits[index + 1], image.Bits[index + 0]);
            }

            texture.SetData(colorData);
            return texture;
        }

        public static Vector2 GetScreenPosition(this Joint joint, Runtime kinectRuntime, int screenWidth, int screenHeight)
        {
            float depthX;
            float depthY;

            kinectRuntime.SkeletonEngine.SkeletonToDepthImage(joint.Position, out depthX, out depthY);
            depthX = Math.Max(0, Math.Min(depthX * 320, 320));  //convert to 320, 240 space
            depthY = Math.Max(0, Math.Min(depthY * 240, 240));  //convert to 320, 240 space

            int colorX;
            int colorY;
            // only ImageResolution.Resolution640x480 is supported at this point
            kinectRuntime.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution640x480, new ImageViewArea(), (int)depthX, (int)depthY, (short)0, out colorX, out colorY);

            // map back to skeleton.Width & skeleton.Height
            return new Vector2((int)screenWidth * colorX / 640.0f, (int)screenHeight * colorY / 480.0f);
        }
    }
}
