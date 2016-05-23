#region License

// Copyright (c) 2015, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: TileField.cs
// Date - created: 2015.08.26 - 14:46
// Date - current: 2016.05.23 - 21:16

#endregion

#region Usings

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.TileHelper
{
    public delegate void UpdateMethod(Tile[,] Tiles, ReadOnlyCollection<float> samples, int arrayStep,
        Color fadeOutColor,
        ColorMode colorMode = ColorMode.OnlyCenter, bool onlycenter = false, float heightMulitplier = 1f);

    class TileField
    {
        private readonly Tile[,] Tiles;

        public TileField(GraphicsDevice device, Vector3 position, float width, float height, float depth,
            int tileCountWidth,
            int tileCountDepth, Color color)
        {
            Tiles = new Tile[tileCountWidth, tileCountDepth];

            float tileWidth = width/tileCountWidth, tileDepth = depth/tileCountDepth;

            for (var x = 0; x < tileCountWidth; x++)
            {
                for (var z = 0; z < tileCountDepth; z++)
                {
                    Tiles[x, z] = new Tile(device,
                        new Vector3(position.X + x*tileWidth, position.Y, position.Z + z*tileDepth),
                        tileWidth, height, tileDepth, color);
                }
            }
        }

        // HUGE PERFORMANCE LOSS!!!
        //public VertexPositionColor[] GetData()
        //{
        //    return Tiles.Cast<Tile>().SelectMany(item => item.Verts).ToArray();
        //}

        public void Draw(BasicEffect basicEffect, GraphicsDevice device)
        {
            for (var i = 0; i < Tiles.GetLength(0); i++)
            {
                for (var j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[i, j].Draw(basicEffect, device);
                }
            }
        }

        public int GetElementCount()
        {
            return Tiles.Length*24;
        }

        public void FillEdgeColors(Color color)
        {
            for (var index00 = 0; index00 < Tiles.GetLength(0); index00++)
                for (var index01 = 0; index01 < Tiles.GetLength(1); index01++)
                {
                    var item = Tiles[index00, index01];
                    item.ChangeSideColor(color);
                }
        }

        // Mode:
        // 1: LTR
        // 2: Middle to Edge
        internal void Update(ReadOnlyCollection<float> samples, int arrayStep, Color centerColorHue, int mode,
            Color fadeOutColor,
            ColorMode colorMode = ColorMode.OnlyCenter, bool onlycenter = false, float heightMulitplier = 1f)
        {
            if (samples != null && samples.Count != 0)
            {
                if (mode == 1)
                {
                    UpdateLinearMode(samples, arrayStep, centerColorHue, fadeOutColor, colorMode, onlycenter,
                        heightMulitplier);
                }
                else if (mode == 2)
                {
                    UpdateCircleMode(samples, arrayStep, centerColorHue, fadeOutColor, colorMode, onlycenter,
                        heightMulitplier);
                }
            }

            for (var index00 = 0; index00 < Tiles.GetLength(0); index00++)
                for (var index01 = 0; index01 < Tiles.GetLength(1); index01++)
                {
                    var tile = Tiles[index00, index01];
                    tile.UpdatedSide = false;
                }
        }

        public void Update(UpdateMethod method, ReadOnlyCollection<float> samples, int arrayStep,
            Color fadeOutColor,
            ColorMode colorMode = ColorMode.OnlyCenter, bool onlycenter = false, float heightMulitplier = 1f)
        {
            method(Tiles, samples, arrayStep, fadeOutColor, colorMode,
                onlycenter, heightMulitplier);
        }

        private void UpdateCircleMode(ReadOnlyCollection<float> samples, int arrayStep, Color centerColorHue,
            Color fadeOutColor,
            ColorMode colorMode = ColorMode.OnlyCenter, bool onlycenter = false, float heightMulitplier = 1f)
        {
            var center = new Vector2((Tiles.GetLength(0))/2 - 1, (Tiles.GetLength(1))/2 - 1);

            for (var x = 0; x < Tiles.GetLength(0); x++)
            {
                for (var y = 0; y < Tiles.GetLength(1); y++)
                {
                    var calculatedCen = Math.Round((decimal) Vector2.Distance(center, new Vector2(x, y)))*arrayStep;

                    Tiles[x, y].ChangeMiddleColors(Color.Lerp(centerColorHue, fadeOutColor,
                        1 - samples[(int) calculatedCen].Normalize(0, 1)));
                    Tiles[x, y].ChangeCenterHeight(
                        samples[(int) calculatedCen]*1.5f*heightMulitplier);
                }
            }

            if (onlycenter && colorMode == ColorMode.OnlyCenter) return;

            for (var x = 0; x < Tiles.GetLength(0); x++)
            {
                for (var y = 0; y < Tiles.GetLength(1); y++)
                {
                    if (!onlycenter)
                    {
                        Tiles[x, y].UpdateOutsideHeight(Tiles, new Point(x, y));
                    }

                    switch (colorMode)
                    {
                        case ColorMode.SideEqualsCenter:
                            Tiles[x, y].ChangeSideColor(Tiles[x, y].CenterColor);
                            break;
                        case ColorMode.DynamicSideColorShiat:
                            Tiles[x, y].ChangeSideColorsDynamic(fadeOutColor);
                            break;
                        case ColorMode.OnlyCenter:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(colorMode), colorMode, null);
                    }
                }
            }
        }

        private void UpdateLinearMode(ReadOnlyCollection<float> samples, int arrayStep, Color centerColorHue,
            Color fadeOutColor,
            ColorMode colorMode = ColorMode.OnlyCenter, bool onlycenter = false, float heightMulitplier = 1f)
        {
            for (var x = Tiles.GetLength(0) - 1; x > -1; x--)
            {
                if (samples.Count <= x*arrayStep)
                {
                    break;
                }

                Tiles[x, 0].ChangeMiddleColors(Color.Lerp(centerColorHue, fadeOutColor,
                    1 - samples[samples.Count - 1 - x*arrayStep].Normalize(0, 1)));
                Tiles[x, 0].ChangeCenterHeight(samples[samples.Count - 1 - x*arrayStep]*1.5f*heightMulitplier);
            }

            for (var x = Tiles.GetLength(0) - 1; x > -1; x--)
            {
                if (samples.Count < x*arrayStep)
                {
                    break;
                }

                for (var y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y].ChangeMiddleColors(Tiles[x, 0].CenterColor);
                    Tiles[x, y].ChangeCenterHeight(Tiles[x, 0].CenterHeight);
                }
            }

            if (onlycenter && colorMode == ColorMode.OnlyCenter) return;

            for (var x = 0; x < Tiles.GetLength(0); x++)
            {
                if (samples.Count < x*arrayStep)
                {
                    break;
                }

                for (var y = 0; y < Tiles.GetLength(1); y++)
                {
                    if (!onlycenter)
                    {
                        Tiles[x, y].UpdateOutsideHeight(Tiles, new Point(x, y));
                    }

                    switch (colorMode)
                    {
                        case ColorMode.SideEqualsCenter:
                            Tiles[x, y].ChangeSideColor(Tiles[x, y].CenterColor);
                            break;
                        case ColorMode.DynamicSideColorShiat:
                            Tiles[x, y].ChangeSideColorsDynamic(fadeOutColor);
                            break;
                        case ColorMode.OnlyCenter:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(colorMode), colorMode, null);
                    }
                }
            }
        }

        public void ResetHeight(float initialHeight)
        {
            foreach (var tile in Tiles)
            {
                tile.ChangeCenterHeight(initialHeight);
                tile.ChangeOutSideHeihgt(initialHeight);
            }
        }
    }
}