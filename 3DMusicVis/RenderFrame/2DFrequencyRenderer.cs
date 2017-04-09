#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: 2DFrequencyRenderer.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.RenderFrame
{
    internal static class _2DFrequencyRenderer
    {
        private const int WIDTH = 1, HEIGHT = 2;
        private static _2DMusicVisRenderFrame _renderer;
        private static ReadOnlyCollection<float> Frequencies;

        public static void Initialise(GraphicsDevice device)
        {
            _renderer =
                new _2DMusicVisRenderFrame
                {
                    Render = Draw,
                    UpdateRenderer = UpdateRenderer,
                    ClearColor = Color.Transparent,
                    ColorMode = ColorMode.SideEqualsCenter,
                    FadeOutColor = Color.Black,
                    ForeGroundColor = Color.White,
                    HightMultiplier = 1.5f
                };
        }

        public static void UpdateRenderer(ReadOnlyCollection<float> frequencies)
        {
            Frequencies = frequencies;
        }

        public static void Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings,
            ref RenderTarget2D tex)
        {
            if (Frequencies == null || Frequencies.Count < 2 || tex == null) return;

            device.SetRenderTarget(tex);
            device.Clear(_renderer.ClearColor);
            Game1.SpriteBatch.Begin();

            switch (settings)
            {
                case DrawMode.Blocked:
                    for (var f = 0; f < Frequencies.Count; f++)
                    {
                        var x = ResolutionManager.VIRTUAL_RESOLUTION.Width * f / (float) Frequencies.Count;
                        var y =
                            (int) (Frequencies[f] * ResolutionManager.VIRTUAL_RESOLUTION.Height / 2);
                        //y = y > Game1.VIRTUAL_RESOLUTION.Height / 2 ? Game1.VIRTUAL_RESOLUTION.Height / 2 : y;

                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());
                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle((int) x, 0, WIDTH * 2, y),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Dashed:
                    for (var f = 0; f < Frequencies.Count; f++)
                    {
                        var x = ResolutionManager.VIRTUAL_RESOLUTION.Width * f / (float) Frequencies.Count;
                        var y =
                            (int) (Frequencies[f] * ResolutionManager.VIRTUAL_RESOLUTION.Height / 2);
                        //y = y > Game1.VIRTUAL_RESOLUTION.Height / 2 ? Game1.VIRTUAL_RESOLUTION.Height / 2 : y;

                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());
                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle((int) x, y, WIDTH, HEIGHT),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Connected:
                    var last = new Vector2(0, Frequencies[0] * ResolutionManager.VIRTUAL_RESOLUTION.Height / 2);
                    for (var f = 1; f < Frequencies.Count; f++)
                    {
                        var y =
                            (int) (Frequencies[f] * ResolutionManager.VIRTUAL_RESOLUTION.Height / 2);
                        //y = y > Game1.VIRTUAL_RESOLUTION.Height / 2 ? Game1.VIRTUAL_RESOLUTION.Height / 2 : y;
                        var c = new Vector2(ResolutionManager.VIRTUAL_RESOLUTION.Width * f / (float) Frequencies.Count,
                            y);

                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());

                        Game1.SpriteBatch.DrawLine(last, c, _renderer.ForeGroundColor);
                        last = c;
                    }
                    break;
                default:
                    break;
            }

            Game1.SpriteBatch.End();

            device.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
        }

        public static Texture2D Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings)
        {
            var toRet = new RenderTarget2D(device, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height, true,
                device.DisplayMode.Format, DepthFormat.Depth24);

            Draw(device, gameTime, cam, settings, ref toRet);

            return toRet;
            //device.Clear(ClearOptions.Draw | ClearOptions.DepthBuffer, _renderer.ClearColor, 1.0f, 0);
            //using (SpriteBatch sprite = new SpriteBatch(device))
            //{
            //    sprite.Begin();
            //    sprite.Draw(shadowMap, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            //    sprite.End();
            //}
        }
    }
}