#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ImageMode.cs
// Date - created:2016.11.10 - 18:58
// Date - current: 2016.11.26 - 14:25

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
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