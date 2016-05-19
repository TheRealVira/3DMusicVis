#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ScreenManager.cs
// Date - created: 2016.05.19 - 18:44
// Date - current: 2016.05.19 - 20:03

#endregion

#region Usings

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.Manager
{
    static class ScreenManager
    {
        private static List<Screen.Screen> Screens;

        public static void Initialise(List<Screen.Screen> screens)
        {
            Screens = screens;
        }

        public static void Delete(Screen.Screen screen)
        {
            Screens.Remove(screen);
        }

        public static void Update(GameTime gameTime)
        {
            if (Screens.Count < 1) return;

            Screens[0].Update(gameTime);
        }

        public static void Draw(SpriteBatch sB, GameTime gameTime)
        {
            if (Screens.Count < 1) return;

            Screens[0].Draw(sB, gameTime);
        }
    }
}