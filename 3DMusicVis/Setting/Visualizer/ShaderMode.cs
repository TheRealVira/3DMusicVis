#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ShaderMode.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis.Setting.Visualizer
{
    [Serializable]
    [Flags]
    internal enum ShaderMode
    {
        None = 0,
        Bloom = 1,
        Blur = 2,
        Liquify = 4,
        ScanLine = 8
    }
}