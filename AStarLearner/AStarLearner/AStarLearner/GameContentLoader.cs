using System;
using System.Collections.Generic;
using Microsoft.Research.Kinect.Nui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace XnaHelpers.GameEngine
{
    static public class ContentLoader
    {
        /// <summary>
        /// Note: this is an extension method to the ContentManager class.
        /// ref: http://weblogs.asp.net/scottgu/archive/2007/03/13/new-orcas-language-feature-extension-methods.aspx
        /// Load all content within a certain folder. The function
        /// returns a dictionary where the file name, without type
        /// extension, is the key and the texture object is the value.
        ///
        /// The contentFolder parameter has to be relative to the
        /// game.Content.RootDirectory folder.
        /// </summary>
        /// <typeparam name="T">The content type.</typeparam>
        /// <param name="contentManager">The content manager for which content is to be loaded.</param>
        /// <param name="contentFolder">The game project root folder relative folder path.</param>
        /// <returns>A list of loaded content objects.</returns
        public static Dictionary<String, T> LoadContent<T>(this ContentManager contentManager, string contentFolder)
        {
            //Load directory info, abort if none. contentManager.RootDirectory => "Content"
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            //Init the resulting list
            Dictionary<String, T> result = new Dictionary<String, T>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                result[key] = contentManager.Load<T>(contentFolder + "\\" + key);
            }

            //Return the result
            return result;
        }

    }
}
