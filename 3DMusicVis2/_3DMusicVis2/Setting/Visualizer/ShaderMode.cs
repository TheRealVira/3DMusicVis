﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ShaderMode.cs
// Date - created:2016.10.18 - 17:50
// Date - current: 2016.10.19 - 19:59

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    [Flags]
    internal enum ShaderMode
    {
        Bloom = 1,
        Blur = 2,
        Liquify = 4,
        ScanLine = 8
    }
}