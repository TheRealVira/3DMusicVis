#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ColorSetting.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.23 - 18:25

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
    }
}