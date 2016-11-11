#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 2DSampleRenderer.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Setting.Visualizer;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    internal static class _2DSampleRenderer
    {
        private static _2DMusicVisRenderFrame _renderer;
        private static ReadOnlyCollection<float> _samples;

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

            _samples = new ReadOnlyCollection<float>(new List<float>());
        }

        public static void UpdateRenderer(ReadOnlyCollection<float> samples)
        {
            lock (_samples)
            {
                _samples = samples;
            }
        }

        public static void Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings,
            ref RenderTarget2D tex)
        {
            if (_samples == null || _samples.Count < 2 || tex == null) return;

            device.SetRenderTarget(tex);
            device.Clear(_renderer.ClearColor);

            Game1.SpriteBatch.Begin();

            switch (settings)
            {
                case DrawMode.Blocked:
                    for (var s = 0; s < _samples.Count; s++)
                    {
                        var x = Game1.VIRTUAL_RESOLUTION.Width * s / _samples.Count;
                        var width = 8;
                        var y =
                            (int)
                                (_samples[s] > 0
                                    ? Game1.VIRTUAL_RESOLUTION.Height / 2f -
                                      _samples[s] * (Game1.VIRTUAL_RESOLUTION.Height / 4f)
                                    : Game1.VIRTUAL_RESOLUTION.Height / 2f - 1);
                        var height = (int)(Math.Abs(_samples[s]) * Game1.VIRTUAL_RESOLUTION.Height / 4f);

                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle(x, y, width, height),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Dashed:
                    for (var s = 0; s < _samples.Count; s++)
                    {
                        var x = Game1.VIRTUAL_RESOLUTION.Width * s / _samples.Count;
                        var width = 1;
                        var y =
                            (int)
                                (Game1.VIRTUAL_RESOLUTION.Height / 2f -
                                 _samples[s] * (Game1.VIRTUAL_RESOLUTION.Height / 4f));
                        var height = 2;

                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle(x, y, width, height),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Connected:
                    var last = new Vector2(0,
                        Game1.VIRTUAL_RESOLUTION.Height / 2f - _samples[0] * (Game1.VIRTUAL_RESOLUTION.Height / 4f));

                    for (var s = 1; s < _samples.Count - 1; s++)
                    {
                        var c = new Vector2(Game1.VIRTUAL_RESOLUTION.Width * s / (float)_samples.Count,
                            Game1.VIRTUAL_RESOLUTION.Height / 2f -
                            _samples[s] * (Game1.VIRTUAL_RESOLUTION.Height / 4f));

                        Game1.SpriteBatch.DrawLine(last, c, _renderer.ForeGroundColor);
                        last = c;
                    }

                    Game1.SpriteBatch.DrawLine(last,
                        new Vector2(Game1.VIRTUAL_RESOLUTION.Width * _samples.Count / (float)_samples.Count,
                            _samples[0] > 0
                                ? Game1.VIRTUAL_RESOLUTION.Height / 2f -
                                  _samples[0] * (Game1.VIRTUAL_RESOLUTION.Height / 4f)
                                : Game1.VIRTUAL_RESOLUTION.Height / 2f - 1), _renderer.ForeGroundColor);
                    break;
                default:
                    break;
            }

            Game1.SpriteBatch.End();

            device.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
        }

        public static Texture2D Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode setting)
        {
            var toRet = new RenderTarget2D(device, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height, true,
                device.DisplayMode.Format, DepthFormat.Depth24);

            Draw(device, gameTime, cam, setting, ref toRet);

            return toRet;
        }
    }
}