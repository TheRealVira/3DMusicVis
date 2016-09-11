#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: MediaPlayerManager.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.11 - 17:35

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

#endregion

namespace _3DMusicVis2.Manager
{
    internal static class MediaPlayerManager
    {
        private static List<SpecialSong> SongList;

        private static int SongPointer;

        public static bool IsPlaying { get; private set; }

        private static int SongPointerProb
        {
            get { return SongPointer; }
            set
            {
                if (SongList.Count > 0)
                {
                    value = Math.Abs(value);
                    SongPointer = (value %= SongList.Count) == 0 ? 0 : value;
                }
            }
        }

        public static void Initialise()
        {
            SongList = new List<SpecialSong>();
        }

        public static void LoadContent(ContentManager content, string directory, bool allContainingDirs,
            GraphicsDevice device)
        {
            MediaPlayer.Stop();
            SongPointerProb = 1;
            var temp = new List<SpecialSong>();

            temp.AddRange(from item in Directory.GetFiles(directory, "*.wma")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), device));

            temp.AddRange(from item in Directory.GetFiles(directory, "*.aif")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), device));

            /*foreach (var item in Directory.GetFiles(@"3DMusicVis2\Music", "*.m4p"))
            {
                //this.Songs.Add(Song.FromUri(item, new Uri(item, UriKind.RelativeOrAbsolute)));
                string songName = item.Substring(18, item.Length - 22);
                this.Songs.Add(new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GraphicsDevice));
            }*/

            temp.AddRange(from item in Directory.GetFiles(directory, "*.mp3")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), device));

            temp.AddRange(from item in Directory.GetFiles(directory, "*.wav")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), device));

            SongList = temp.OrderBy(x => x.MySong.Name).ToList();

            if (SongList.Count > 0)
            {
                //MediaPlayer.Play(SongList[SongPointerProb].MySong);
                //SongPointerProb++;
                //IsPlaying = true;
                //IsStopped = false;
            }
            else
            {
                Console.Write("No music found...");
            }
        }

        public static void Play()
        {
            IsPlaying = true;
            SongPointerProb++;
            SongPointerProb++;
            MediaPlayer.Play(SongList[SongPointerProb].MySong);
            SongPointerProb++;
        }

        public static void Pause()
        {
            IsPlaying = false;
            MediaPlayer.Pause();
        }

        public static void Next()
        {
            IsPlaying = true;
            MediaPlayer.Stop();
            Play();
        }

        public static void Resume()
        {
            IsPlaying = true;
            MediaPlayer.Resume();
        }

        public static void Previous()
        {
            IsPlaying = true;
            MediaPlayer.Stop();
            SongPointerProb --;
            Play();
        }

        public static void Update()
        {
            IsPlaying = MediaPlayer.State != MediaState.Stopped;
            if (IsPlaying) return;

            Play();
        }
    }
}