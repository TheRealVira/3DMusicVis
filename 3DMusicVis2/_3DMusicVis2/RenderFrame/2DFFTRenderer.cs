#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 2DFFTRenderer.cs
// Date - created:2016.09.10 - 19:25
// Date - current: 2016.09.12 - 21:23

#endregion

#region Usings

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NAudio.Dsp;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    internal class _2DFFTRenderer
    {
        private static _2DMusicVisRenderFrame _renderer;
        private static ReadOnlyCollection<Complex> _frequencies;

        public static void Initialise(GraphicsDevice device)
        {
            var pp = device.PresentationParameters;
            _renderer =
                new _2DMusicVisRenderFrame
                {
                    Render = Target,
                    ClearColor = Color.Transparent,
                    ColorMode = ColorMode.SideEqualsCenter,
                    FadeOutColor = Color.Black,
                    ForeGroundColor = Color.Red,
                    HightMultiplier = 1.5f
                };
        }

        public static void UpdateRenderer(ReadOnlyCollection<Complex> frequencies)
        {
            _frequencies = frequencies;
        }

        public static Texture2D Target(GraphicsDevice device, GameTime gameTime, Camera cam)
        {
            if (_frequencies == null) return Game1.FamouseOnePixel;

            var pp = device.PresentationParameters;
            var MyRenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, true,
                device.DisplayMode.Format, DepthFormat.Depth24);

            device.SetRenderTarget(MyRenderTarget);
            device.Clear(_renderer.ClearColor);
            using (var sprite = new SpriteBatch(device))
            {
                sprite.Begin();
                Game1.BasicEffect.Projection = cam.Projektion;
                Game1.BasicEffect.View = cam.View;

                for (var f = 0; f < _frequencies.Count; f++)
                {
                    var x = Game1.VIRTUAL_RESOLUTION.Width*f/(float) _frequencies.Count;
                    sprite.DrawLine(new Vector2(x, Game1.Graphics.PreferredBackBufferHeight),
                        new Vector2(x, Game1.Graphics.PreferredBackBufferHeight - Math.Abs(_frequencies[f].Y)*1000 + 1f),
                        _renderer.ForeGroundColor);
                }

                sprite.End();
            }

            device.SetRenderTarget(null);

            return MyRenderTarget;
            //device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, _renderer.ClearColor, 1.0f, 0);
            //using (SpriteBatch sprite = new SpriteBatch(device))
            //{
            //    sprite.Begin();
            //    sprite.Draw(shadowMap, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            //    sprite.End();
            //}
        }
    }
}