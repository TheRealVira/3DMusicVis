#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 2DFrequencyRenderer.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System.Collections.ObjectModel;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Setting.Visualizer;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    public static class _2DFrequencyRenderer
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

        public static Texture2D Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings)
        {
            if (Frequencies == null || Frequencies.Count < 2) return Game1.FamouseOnePixel;

            var toRet = new RenderTarget2D(device, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height, true,
                device.DisplayMode.Format, DepthFormat.Depth24);
            device.SetRenderTarget(toRet);
            device.Clear(_renderer.ClearColor);

            Game1.SpriteBatch.Begin();

            switch (settings)
            {
                case DrawMode.Blocked:
                    for (var f = 0; f < Frequencies.Count; f++)
                    {
                        var x = Game1.VIRTUAL_RESOLUTION.Width*f/(float) Frequencies.Count;
                        var y =
                            (int) (Frequencies[f]*Game1.VIRTUAL_RESOLUTION.Height/2);
                        //y = y > Game1.VIRTUAL_RESOLUTION.Height / 2 ? Game1.VIRTUAL_RESOLUTION.Height / 2 : y;

                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());
                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle((int) x, 0, WIDTH*2, y),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Dashed:
                    for (var f = 0; f < Frequencies.Count; f++)
                    {
                        var x = Game1.VIRTUAL_RESOLUTION.Width*f/(float) Frequencies.Count;
                        var y =
                            (int) (Frequencies[f]*Game1.VIRTUAL_RESOLUTION.Height/2);
                        //y = y > Game1.VIRTUAL_RESOLUTION.Height / 2 ? Game1.VIRTUAL_RESOLUTION.Height / 2 : y;

                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());
                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle((int) x, y, WIDTH, HEIGHT),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Connected:
                    var last = new Vector2(0, Frequencies[0]*Game1.VIRTUAL_RESOLUTION.Height/2);
                    for (var f = 1; f < Frequencies.Count; f++)
                    {
                        var y =
                            (int) (Frequencies[f]*Game1.VIRTUAL_RESOLUTION.Height/2);
                        //y = y > Game1.VIRTUAL_RESOLUTION.Height / 2 ? Game1.VIRTUAL_RESOLUTION.Height / 2 : y;
                        var c = new Vector2(Game1.VIRTUAL_RESOLUTION.Width*f/(float) Frequencies.Count, y);

                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());

                        Primitives2D.DrawLine(Game1.SpriteBatch, last, c, _renderer.ForeGroundColor);
                        last = c;
                    }
                    break;
                default:
                    break;
            }

            Game1.SpriteBatch.End();

            device.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);

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