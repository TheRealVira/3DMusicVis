#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: 2DSampleRenderer.cs
// Date - created:2017.04.14 - 09:21
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.RenderFrame._2D
{
    internal static class _2DSampleRenderer
    {
        private static _2DMusicVisRenderFrame _renderer;
        private static ReadOnlyCollection<float> _samples;

        public static RendererDefaults.DrawGraphicsOnRenderTarget DrawingTechnique;
        public new static string ToString() => "2F";

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

            DrawingTechnique = Draw;
        }

        public static void UpdateRenderer(ReadOnlyCollection<float> samples)
        {
            lock (_samples)
            {
                _samples = samples;
            }
        }

        private static void Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings,
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
                        var x = ResolutionManager.VIRTUAL_RESOLUTION.Width * s / _samples.Count;
                        const int width = 8;
                        var y =
                            (int)
                            (_samples[s] > 0
                                ? ResolutionManager.VIRTUAL_RESOLUTION.Height / 2f -
                                  _samples[s] * (ResolutionManager.VIRTUAL_RESOLUTION.Height / 4f)
                                : ResolutionManager.VIRTUAL_RESOLUTION.Height / 2f - 1);
                        var height = (int) (Math.Abs(_samples[s]) * ResolutionManager.VIRTUAL_RESOLUTION.Height / 4f);

                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle(x, y, width, height),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Dashed:
                    for (var s = 0; s < _samples.Count; s++)
                    {
                        var x = ResolutionManager.VIRTUAL_RESOLUTION.Width * s / _samples.Count;
                        const int width = 1;
                        var y =
                            (int)
                            (ResolutionManager.VIRTUAL_RESOLUTION.Height / 2f -
                             _samples[s] * (ResolutionManager.VIRTUAL_RESOLUTION.Height / 4f));
                        var height = 2;

                        Game1.SpriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle(x, y, width, height),
                            _renderer.ForeGroundColor);
                    }
                    break;
                case DrawMode.Connected:
                    var last = new Vector2(0,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height / 2f -
                        _samples[0] * (ResolutionManager.VIRTUAL_RESOLUTION.Height / 4f));

                    for (var s = 1; s < _samples.Count - 1; s++)
                    {
                        var c = new Vector2(ResolutionManager.VIRTUAL_RESOLUTION.Width * s / (float) _samples.Count,
                            ResolutionManager.VIRTUAL_RESOLUTION.Height / 2f -
                            _samples[s] * (ResolutionManager.VIRTUAL_RESOLUTION.Height / 4f));

                        Game1.SpriteBatch.DrawLine(last, c, _renderer.ForeGroundColor);
                        last = c;
                    }

                    Game1.SpriteBatch.DrawLine(last,
                        new Vector2(
                            ResolutionManager.VIRTUAL_RESOLUTION.Width * _samples.Count / (float) _samples.Count,
                            _samples[0] > 0
                                ? ResolutionManager.VIRTUAL_RESOLUTION.Height / 2f -
                                  _samples[0] * (ResolutionManager.VIRTUAL_RESOLUTION.Height / 4f)
                                : ResolutionManager.VIRTUAL_RESOLUTION.Height / 2f - 1), _renderer.ForeGroundColor);
                    break;
                default:
                    break;
            }

            Game1.SpriteBatch.End();

            device.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
        }

        public static Texture2D Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode setting)
        {
            var toRet = new RenderTarget2D(device, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height, true,
                device.DisplayMode.Format, DepthFormat.Depth24);

            Draw(device, gameTime, cam, setting, ref toRet);

            return toRet;
        }
    }
}