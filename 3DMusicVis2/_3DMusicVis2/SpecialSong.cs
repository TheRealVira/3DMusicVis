#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: SpecialSong.cs
// Date - created:2016.07.02 - 17:04
// Date - current: 2016.10.19 - 19:59

#endregion

#region Usings

using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using File = TagLib.File;

#endregion

namespace _3DMusicVis2
{
    public class SpecialSong
    {
        public Song MySong;
        public File TagLibFile;
        public Texture2D Thumbnail;
        public Uri Uri;

        public SpecialSong(string songName, Uri songUri, GraphicsDevice graphics)
        {
            MySong = Song.FromUri(songName, songUri);
            Uri = songUri;

            var absolutePath = Path.GetFullPath(songUri.OriginalString);
            TagLibFile = File.Create(absolutePath);

            if (TagLibFile.Tag.Pictures.Length > 0)
            {
                var bin = TagLibFile.Tag.Pictures[0].Data.Data;
                Thumbnail = Texture2D.FromStream(graphics, new MemoryStream(bin));
            }
        }
    }
}