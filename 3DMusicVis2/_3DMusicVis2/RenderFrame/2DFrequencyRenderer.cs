#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 2DFrequencyRenderer.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.10.17 - 20:43

#endregion

#region Usings

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    public static class _2DFrequencyRenderer
    {
        private static _2DMusicVisRenderFrame _renderer;
        private static ReadOnlyCollection<float> Frequencies;

        public static bool Dashed;

        public static void Initialise(GraphicsDevice device)
        {
            _renderer =
                new _2DMusicVisRenderFrame
                {
                    Render = Target,
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

        public static Texture2D Target(GraphicsDevice device, GameTime gameTime, Camera cam)
        {
            if (Frequencies == null) return Game1.FamouseOnePixel;

            var toRet = new RenderTarget2D(device, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height, true,
                device.DisplayMode.Format, DepthFormat.Depth24);
            device.SetRenderTarget(toRet);
            device.Clear(_renderer.ClearColor);
            using (var sprite = new SpriteBatch(device))
            {
                sprite.Begin();
                Game1.BasicEffect.Projection = cam.Projektion;
                Game1.BasicEffect.View = cam.View;

                if (Dashed)
                {
                    for (var f = 0; f < Frequencies.Count; f++)
                    {
                        var x = Game1.VIRTUAL_RESOLUTION.Width*f/(float) Frequencies.Count;
                        var y =
                            (int)
                                (Game1.VIRTUAL_RESOLUTION.Height -
                                 Frequencies[f]*Game1.VIRTUAL_RESOLUTION.Height/2);
                        var width = 1;
                        var height = 2;
                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());
                        sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) x, y, width*2, height),
                            _renderer.ForeGroundColor);
                    }
                }
                else
                {
                    for (var f = 0; f < Frequencies.Count; f++)
                    {
                        var x = Game1.VIRTUAL_RESOLUTION.Width*f/(float) Frequencies.Count;
                        var y =
                            (int)
                                (Game1.VIRTUAL_RESOLUTION.Height -
                                 Frequencies[f]*Game1.VIRTUAL_RESOLUTION.Height/2);
                        var width = 1;
                        var height = (int) (Frequencies[f]*Game1.VIRTUAL_RESOLUTION.Height/2);
                        //sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) (x + width*2), y, width, height),
                        //    _renderer.ForeGroundColor.Negate());
                        sprite.Draw(Game1.FamouseOnePixel, new Rectangle((int) x, y, width*2, height),
                            _renderer.ForeGroundColor);
                    }
                }

                sprite.End();
            }

            device.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);

            return toRet;
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