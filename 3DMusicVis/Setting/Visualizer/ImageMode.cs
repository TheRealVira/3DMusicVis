#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ImageMode.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis.Setting.Visualizer
{
    [Flags]
    internal enum ImageMode
    {
        None = 0,
        Vibrate = 1,
        Rotate = 1 << 1,
        ReverseOnBeat = 1 << 2,
        HoverRender = 1 << 3
    }
}