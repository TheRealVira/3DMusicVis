﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: MyMath.cs
// Date - created: 2016.05.09 - 16:14
// Date - current: 2016.05.19 - 20:03

#endregion

#region Usings

using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2
{
    public static class MyMath
    {
        public static float Abs(this float value)
        {
            return value > 0 ? value : -value;
        }

        public static float Normalize(this float value, float min, float max) => (value - min)/(max - min);

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
            return value + (otherValue - value)*amount;
        }

        public static Color DarkenColor(this Color color, byte amount)
        {
            return new Color(color.R - amount, color.B - amount, color.B - amount, color.A);
        }
    }
}