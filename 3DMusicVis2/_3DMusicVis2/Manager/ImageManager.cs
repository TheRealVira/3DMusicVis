#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ImageManager.cs
// Date - created:2016.11.09 - 21:04
// Date - current: 2016.11.11 - 09:50

#endregion

#region Usings

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.Manager
{
    internal static class ImageManager
    {
        public const string IMAGES_EXT = "*.png,*.jpg";
        public const string IMAGES_DIR = "Images";

        public static Dictionary<string, Texture2D> Images { get; private set; }

        public static void Initialise()
        {
            if (Images != null) return;

            Directory.CreateDirectory(IMAGES_DIR);

            Images = new Dictionary<string, Texture2D>();

            foreach (
                var file in
                    Directory.GetFiles(IMAGES_DIR, "*.*", SearchOption.AllDirectories).Where(s =>
                    {
                        var extension = Path.GetExtension(s);
                        return extension != null && IMAGES_EXT.Contains(extension.ToLower());
                    }))
            {
                using (var fStream = new FileStream(file, FileMode.Open))
                {
                    Images.Add(Path.GetFileName(file), Texture2D.FromStream(Game1.Graphics.GraphicsDevice, fStream));
                }
            }
        }
    }
}