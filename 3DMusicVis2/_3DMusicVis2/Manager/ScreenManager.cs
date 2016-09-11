﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ScreenManager.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.11 - 17:35

#endregion

#region Usings

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.Manager
{
    internal static class ScreenManager
    {
        private static List<Screen.Screen> Screens;

        public static void Initialise(List<Screen.Screen> screens)
        {
            Screens = screens;
        }

        public static void LoadNextScreen(Screen.Screen screen)
        {
            Unload(screen);
            Screens[0]?.LoadedUp();
        }

        public static void Update(GameTime gameTime)
        {
            if (Screens.Count < 1) return;

            Screens[0]?.Update(gameTime);
        }

        public static void Draw(SpriteBatch sB, GameTime gameTime)
        {
            if (Screens.Count < 1) return;

            Screens[0]?.Draw(sB, gameTime);
        }

        public static void Unload(Screen.Screen screen)
        {
            screen?.Unloade();
            Screens.Remove(screen);
        }

        public static void UnloadAll()
        {
            if (Screens.Count < 1) return;

            for (var i = Screens.Count - 1; i > -1; i--)
            {
                Unload(Screens[i]);
            }
        }
    }
}