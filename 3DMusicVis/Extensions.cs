#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Extensions.cs
// Date - created:2016.12.10 - 09:37
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis
{
    public static class Extensions
    {
        public static int Clamp(this int number, int smalestPossible, int biggestPossible)
        {
            if (number < biggestPossible && number > smalestPossible)
                return number;
            if (number > smalestPossible)
                return biggestPossible;
            return smalestPossible;
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle((int) begin.X, (int) begin.Y, (int) (end - begin).Length() + width, width), null, color,
                (begin.Y > end.Y ? MathHelper.TwoPi : 0) -
                (float) Math.Acos(Vector2.Dot(Vector2.Normalize(begin - end), -Vector2.UnitX)), Vector2.Zero,
                SpriteEffects.None, 0);
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

        public static Point ToPoint(this Vector2 value)
        {
            return new Point((int) value.X, (int) value.Y);
        }

        public static RenderTarget2D Clone(this RenderTarget2D target)
        {
            var clone = new RenderTarget2D(target.GraphicsDevice, target.Width,
                target.Height, target.LevelCount > 1, target.Format,
                target.DepthStencilFormat, target.MultiSampleCount,
                target.RenderTargetUsage);

            for (var i = 0; i < target.LevelCount; i++)
            {
                var rawMipWidth = target.Width / Math.Pow(2, i);
                var rawMipHeight = target.Height / Math.Pow(2, i);

                // make sure that mipmap dimensions are always > 0.
                var mipWidth = rawMipWidth < 1 ? 1 : (int) rawMipWidth;
                var mipHeight = rawMipHeight < 1 ? 1 : (int) rawMipHeight;

                var mipData = new Color[mipWidth * mipHeight];
                target.GetData(i, null, mipData, 0, mipData.Length);
                clone.SetData(i, null, mipData, 0, mipData.Length);
            }

            return clone;
        }

        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0) return enumerationValue.ToString();
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0 ? ((DescriptionAttribute) attrs[0]).Description : enumerationValue.ToString();
            //If we have no description attribute, just return the ToString of the enum
        }

        public static void ResetGraphic(this GraphicsDevice dev)
        {
            dev.BlendState = BlendState.AlphaBlend;
            dev.DepthStencilState = DepthStencilState.None;
            dev.RasterizerState = RasterizerState.CullCounterClockwise;
            dev.SamplerStates[0] = SamplerState.AnisotropicWrap;
        }

        public static void BeginRender3D(this GraphicsDevice dev)
        {
            dev.BlendState = BlendState.Opaque;
            dev.DepthStencilState = DepthStencilState.Default;
            dev.SamplerStates[0] = SamplerState.LinearWrap;
        }

        public static Color GetAppliedColor(this ColorSetting setting, float breathTime, float rainbowTime,
            Color baseColor)
        {
            switch (setting.Mode)
            {
                case Setting.Visualizer.ColorMode.Static:
                    return setting.Color;

                case Setting.Visualizer.ColorMode.Rainbow:
                    return MyMath.Rainbow(rainbowTime);

                case Setting.Visualizer.ColorMode.Breath:
                    return Color.Lerp(setting.Color, baseColor, breathTime);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Color GetAppliedColor(this ColorSetting setting, float breathTime, float rainbowTime)
        {
            return setting.GetAppliedColor(breathTime, rainbowTime, setting.BaseColor);
        }

        public static Dictionary<string, T> LoadListContent<T>(this ContentManager contentManager, string contentFolder)
        {
            var dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            var result = new Dictionary<string, T>();

            var files = dir.GetFiles("*.*");
            foreach (var file in files)
            {
                var key = Path.GetFileNameWithoutExtension(file.Name);


                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            return result;
        }
    }
}