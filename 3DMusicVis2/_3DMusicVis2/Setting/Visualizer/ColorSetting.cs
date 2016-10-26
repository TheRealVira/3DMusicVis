#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ColorSetting.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    public struct ColorSetting
    {
        public Color Color;
        public ColorMode Mode;

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ColorSetting)) return false;

            var temp = (ColorSetting) obj;

            return (Color == temp.Color) && (Mode == temp.Mode);
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