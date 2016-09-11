#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Extensions.cs
// Date - created:2016.07.02 - 17:04
// Date - current: 2016.09.11 - 17:35

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace _3DMusicVis2
{
    public static class Extensions
    {
        public static int Clamp(this int number, int smalestPossible, int biggestPossible)
        {
            if (number < biggestPossible && number > smalestPossible)
            {
                return number;
            }
            if (number > smalestPossible)
            {
                return biggestPossible;
            }
            return smalestPossible;
        }

        public static bool KeyWasClicked(this Keys key)
            => Game1.NewKeyboardState.IsKeyUp(key) && Game1.OldKeyboardState.IsKeyDown(key);

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            var r = new Rectangle((int) begin.X, (int) begin.Y, (int) (end - begin).Length() + width, width);
            var v = Vector2.Normalize(begin - end);
            var angle = (float) Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(Game1.FamouseOnePixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static Vector2 ToVector2(this Point value)
        {
            return new Vector2(value.X, value.Y);
        }

        public static RenderTarget2D Clone(this RenderTarget2D target)
        {
            var clone = new RenderTarget2D(target.GraphicsDevice, target.Width,
                target.Height, target.LevelCount > 1, target.Format,
                target.DepthStencilFormat, target.MultiSampleCount,
                target.RenderTargetUsage);

            for (var i = 0; i < target.LevelCount; i++)
            {
                var rawMipWidth = target.Width/Math.Pow(2, i);
                var rawMipHeight = target.Height/Math.Pow(2, i);

                // make sure that mipmap dimensions are always > 0.
                var mipWidth = rawMipWidth < 1 ? 1 : (int) rawMipWidth;
                var mipHeight = rawMipHeight < 1 ? 1 : (int) rawMipHeight;

                var mipData = new Color[mipWidth*mipHeight];
                target.GetData(i, null, mipData, 0, mipData.Length);
                clone.SetData(i, null, mipData, 0, mipData.Length);
            }

            return clone;
        }
    }
}