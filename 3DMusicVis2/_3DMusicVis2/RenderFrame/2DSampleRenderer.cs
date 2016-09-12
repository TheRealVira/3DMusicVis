#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 2DSampleRenderer.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.12 - 21:23

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    internal static class _2DSampleRenderer
    {
        private static _2DMusicVisRenderFrame _renderer;
        private static ReadOnlyCollection<float> _samples;

        public static bool PunctionalDrawing;

        public static void Initialise(GraphicsDevice device)
        {
            var pp = device.PresentationParameters;
            _renderer =
                new _2DMusicVisRenderFrame
                {
                    Render = Target,
                    UpdateRenderer = UpdateRenderer,
                    ClearColor = Color.Transparent,
                    ColorMode = ColorMode.SideEqualsCenter,
                    FadeOutColor = Color.Black,
                    ForeGroundColor = Color.Red,
                    HightMultiplier = 1.5f
                };

            _samples = new ReadOnlyCollection<float>(new List<float>());
        }

        public static void UpdateRenderer(ReadOnlyCollection<float> samples)
        {
            lock (_samples)
            {
                _samples = samples;
            }
        }

        public static Texture2D Target(GraphicsDevice device, GameTime gameTime, Camera cam)
        {
            if (_samples == null) return Game1.FamouseOnePixel;

            var pp = device.PresentationParameters;
            var MyRenderTarget = new RenderTarget2D(device, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height, true,
                device.DisplayMode.Format, DepthFormat.Depth24);
            device.SetRenderTarget(MyRenderTarget);
            device.Clear(_renderer.ClearColor);
            using (var sprite = new SpriteBatch(device))
            {
                sprite.Begin();
                Game1.BasicEffect.Projection = cam.Projektion;
                Game1.BasicEffect.View = cam.View;

                lock (_samples)
                {
                    if (PunctionalDrawing)
                    {
                        for (var s = 0; s < _samples.Count; s++)
                        {
                            var x = Game1.VIRTUAL_RESOLUTION.Width*s/_samples.Count;
                            var width = 1;
                            var y =
                                (int)
                                    (Game1.VIRTUAL_RESOLUTION.Height/2f -
                                     _samples[s]*(Game1.VIRTUAL_RESOLUTION.Height/4f));
                            var height = 2;

                            sprite.Draw(Game1.FamouseOnePixel, new Rectangle(x, y, width, height),
                                _renderer.ForeGroundColor.Negate());
                        }
                    }
                    else
                    {
                        for (var s = 0; s < _samples.Count; s++)
                        {
                            var x = Game1.VIRTUAL_RESOLUTION.Width*s/_samples.Count;
                            var width = 8;
                            var y =
                                (int)
                                    (Game1.VIRTUAL_RESOLUTION.Height/2f -
                                     _samples[s]*(Game1.VIRTUAL_RESOLUTION.Height/4f));
                            var height = (int)
                                (Math.Abs(_samples[s])*
                                 Game1.VIRTUAL_RESOLUTION.Height/4f);

                            sprite.Draw(Game1.FamouseOnePixel, new Rectangle(x, y, width, height),
                                _renderer.ForeGroundColor);
                        }
                    }
                }

                sprite.End();
            }

            device.SetRenderTarget(null);

            return MyRenderTarget;
        }
    }
}