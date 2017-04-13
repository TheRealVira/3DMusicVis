#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: MyMath.cs
// Date - created:2016.12.10 - 09:37
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis
{
    public static class MyMath
    {
        public static float Abs(this float value)
        {
            return value > 0 ? value : -value;
        }

        public static float Normalize(this float value) => 1f - 1f / (1f + value);

        public static Color Negate(this Color color)
        {
            return new Color(255 - color.R, 255 - color.G, 255 - color.B);
        }

        public static Color HalfNegate(this Color color)
        {
            return new Color(127 - color.R, 127 - color.G, 127 - color.B);
        }

        public static float Lerp(this float value, float otherValue, float amount)
        {
            return value + (otherValue - value) * amount;
        }

        public static Color DarkenColor(this Color color, byte amount)
        {
            return new Color(color.R - amount, color.B - amount, color.B - amount, color.A);
        }

        public static Color Rainbow(float progress)
        {
            var div = Math.Abs(progress % 1) * 6;
            var ascending = (int) (div % 1 * 255);
            var descending = 255 - ascending;

            switch ((int) div)
            {
                case 0:
                    return Color.FromNonPremultiplied(255, 255, ascending, 0);
                case 1:
                    return Color.FromNonPremultiplied(255, descending, 255, 0);
                case 2:
                    return Color.FromNonPremultiplied(255, 0, 255, ascending);
                case 3:
                    return Color.FromNonPremultiplied(255, 0, descending, 255);
                case 4:
                    return Color.FromNonPremultiplied(255, ascending, 0, 255);
                default: // case 5:
                    return Color.FromNonPremultiplied(255, 255, 0, descending);
            }
        }
    }
}