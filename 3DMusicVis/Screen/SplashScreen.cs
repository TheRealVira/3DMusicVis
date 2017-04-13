#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: SplashScreen.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Screen
{
    internal class SplashScreen : Screen
    {
        private readonly Texture2D Texture;
        private float DurationTillDetimination;

        public SplashScreen(GraphicsDeviceManager gdm, Texture2D texture, float duration = Game1.SplashMaxCount)
            : base(gdm, "SplashScreen")
        {
            DurationTillDetimination = duration;
            Texture = texture;
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            GDM.GraphicsDevice.Clear(Color.Black);
            var screenCenter = new Vector2(
                GDM.GraphicsDevice.Viewport.Bounds.Width / 2f,
                GDM.GraphicsDevice.Viewport.Bounds.Height / 2f);
            var textureCenter = new Vector2(
                Texture.Width / 2f,
                Texture.Height / 2f);

            sB.Begin();
            sB.Draw(Texture, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f);
            sB.End();
        }

        public override void Update(GameTime gameTime)
        {
            DurationTillDetimination -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (DurationTillDetimination < 1)
                ScreenManager.LoadNextScreen(this);
        }
    }
}