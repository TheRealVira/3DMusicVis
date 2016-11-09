using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace _3DMusicVis2.Manager
{
    static class ImageManager
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
                    Directory.GetFiles(ImageManager.IMAGES_DIR, "*.*", SearchOption.AllDirectories).Where(s =>
                    {
                        var extension = Path.GetExtension(s);
                        return extension != null && ImageManager.IMAGES_EXT.Contains(extension.ToLower());
                    }))
            {
                using (FileStream fStream = new FileStream(file, FileMode.Open))
                {
                    Images.Add(Path.GetFileName(file), Texture2D.FromStream(Game1.Graphics.GraphicsDevice, fStream));
                }
            }
        }
    }
}
