#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 3DLinearWaveRenderer.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.19 - 16:56

#endregion

#region Usings

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.TileHelper;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    internal static class _3DLinearWaveRenderer
    {
        private static _3DMusicVisRenderFrame _renderer;
        private static TileField Field;

        public static void Initialise(GraphicsDevice device)
        {
            var pp = device.PresentationParameters;
            _renderer =
                new _3DMusicVisRenderFrame
                {
                    Render = Target,
                    UpdateRenderer = UpdateRenderer,
                    ClearColor = Color.Transparent,
                    ColorMode = ColorMode.SideEqualsCenter,
                    FadeOutColor = Color.Black,
                    ForeGroundColor = Color.Red,
                    HightMultiplier = 1.5f,
                    UpdateOnlyTheCenterOfATile = false,
                    RastState = new RasterizerState
                    {
                        CullMode = CullMode.None,
                        FillMode = FillMode.Solid
                    }
                };

            Field = new TileField(device, Vector3.Zero, 20, Game1.INITAIL_HEIGHT, 20, Game1.FIELD_WIDTH,
                Game1.FIELD_WIDTH,
                Color.Black);
        }

        private static void MyMethod(Tile[,] tiles, ReadOnlyCollection<float> samples, int arrayStep, Color fadeOutColor,
            ColorMode colorMode, bool onlycenter, float heightMulitplier)
        {
            for (var x = tiles.GetLength(0) - 1; x > -1; x--)
            {
                if (samples.Count <= x*arrayStep)
                {
                    break;
                }

                tiles[x, 0].ChangeMiddleColors(Color.Lerp(_renderer.ForeGroundColor, fadeOutColor,
                    1 - samples[samples.Count - 1 - x*arrayStep].Normalize()));
                tiles[x, 0].ChangeCenterHeight(samples[samples.Count - 1 - x*arrayStep]*1.5f*heightMulitplier);
            }

            for (var x = tiles.GetLength(0) - 1; x > -1; x--)
            {
                if (samples.Count < x*arrayStep)
                {
                    break;
                }

                for (var y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x, y].ChangeMiddleColors(tiles[x, 0].CenterColor);
                    tiles[x, y].ChangeCenterHeight(tiles[x, 0].CenterHeight);
                }
            }

            if (onlycenter && colorMode == ColorMode.OnlyCenter) return;

            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                if (samples.Count < x*arrayStep)
                {
                    break;
                }

                for (var y = 0; y < tiles.GetLength(1); y++)
                {
                    if (!onlycenter)
                    {
                        tiles[x, y].UpdateOutsideHeight(tiles, new Point(x, y));
                    }

                    switch (colorMode)
                    {
                        case ColorMode.SideEqualsCenter:
                            tiles[x, y].ChangeSideColor(tiles[x, y].CenterColor);
                            break;
                        case ColorMode.DynamicSideColorShiat:
                            tiles[x, y].ChangeSideColorsDynamic(fadeOutColor);
                            break;
                        case ColorMode.OnlyCenter:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(colorMode), colorMode, null);
                    }
                }
            }
        }

        public static void UpdateRenderer(ReadOnlyCollection<float> samples)
        {
            Field.Update(MyMethod, samples, samples.Count/87, _renderer.FadeOutColor, _renderer.ColorMode,
                _renderer.UpdateOnlyTheCenterOfATile, _renderer.HightMultiplier);
        }

        public static Texture2D Target(GraphicsDevice device, GameTime gameTime, Camera cam)
        {
            var pp = device.PresentationParameters;
            var MyRenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, true,
                device.DisplayMode.Format, DepthFormat.Depth24);
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = _renderer.RastState;
            device.SamplerStates[0] = SamplerState.AnisotropicWrap;

            device.SetRenderTarget(MyRenderTarget);
            device.Clear(_renderer.ClearColor);
            using (var sprite = new SpriteBatch(device))
            {
                sprite.Begin();
                Game1.BasicEffect.Projection = cam.Projektion;
                Game1.BasicEffect.View = cam.View;

                Field.Draw(Game1.BasicEffect, device);
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