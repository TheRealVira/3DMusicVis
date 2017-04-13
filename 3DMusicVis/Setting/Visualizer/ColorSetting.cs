#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ColorSetting.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis.Setting.Visualizer
{
    [Serializable]
    public struct ColorSetting
    {
        public bool Equals(ColorSetting other)
        {
            return Color.Equals(other.Color) && BaseColor.Equals(other.BaseColor) && Mode == other.Mode &&
                   Negate == other.Negate;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Color.GetHashCode();
                hashCode = (hashCode * 397) ^ BaseColor.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Mode;
                hashCode = (hashCode * 397) ^ Negate.GetHashCode();
                return hashCode;
            }
        }

        public Color Color;

        /// <summary>
        ///     Is used as baseline for gradiantal effects (breafing, etc.)
        /// </summary>
        public Color BaseColor;

        public ColorMode Mode;
        public bool Negate;

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ColorSetting)) return false;

            var temp = (ColorSetting) obj;

            return Color == temp.Color && Mode == temp.Mode && BaseColor == temp.BaseColor;
        }

        public static bool operator ==(ColorSetting a, ColorSetting b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ColorSetting a, ColorSetting b)
        {
            return !(a == b);
        }
    }
}