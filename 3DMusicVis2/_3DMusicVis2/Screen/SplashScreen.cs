﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: SplashScreen.cs
// Date - created: 2016.05.19 - 18:04
// Date - current: 2016.05.19 - 20:03

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Manager;

#endregion

namespace _3DMusicVis2.Screen
{
    class SplashScreen : Screen
    {
        private float DurationTillDetimination;
        private readonly Texture2D Texture;

        public SplashScreen(GraphicsDeviceManager gdm, Texture2D texture, float duration = Game1.SplashMaxCount)
            : base(gdm)
        {
            DurationTillDetimination = duration;
            Texture = texture;
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            GDM.GraphicsDevice.Clear(Color.Black);
            var screenCenter = new Vector2(
                GDM.GraphicsDevice.Viewport.Bounds.Width/2,
                GDM.GraphicsDevice.Viewport.Bounds.Height/2);
            var textureCenter = new Vector2(
                Texture.Width/2,
                Texture.Height/2);
            sB.Draw(Texture, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
        }

        public override void Update(GameTime gameTime)
        {
            DurationTillDetimination -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (DurationTillDetimination < 1)
            {
                ScreenManager.Delete(this);
            }
        }
    }
}