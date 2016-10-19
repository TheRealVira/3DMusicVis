#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ColorSetting.cs
// Date - created:2016.10.18 - 17:49
// Date - current: 2016.10.19 - 19:59

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    internal struct ColorSetting
    {
        public Color Color;
        public ColorMode Mode;
    }
}