#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ColorSetting.cs
// Date - created:2016.09.19 - 13:43
// Date - current: 2016.09.19 - 16:56

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2.Setting
{
    [Serializable]
    internal struct ColorSetting
    {
        public Color Color;
        public ColorMode Mode;
    }
}