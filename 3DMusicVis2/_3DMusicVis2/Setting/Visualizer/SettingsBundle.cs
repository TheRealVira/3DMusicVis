﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: SettingsBundle.cs
// Date - created:2016.10.18 - 17:49
// Date - current: 2016.10.19 - 19:59

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    internal struct SettingsBundle
    {
        public Transformation Trans;

        /// <summary>
        ///     False: It is a 2D visualizer; True: It is a 3D visualizer;
        /// </summary>
        public bool Is3D;

        /// <summary>
        ///     False: It is a non dashed visualizer; True: It is a dashed visualizer;
        /// </summary>
        public bool IsDashed;

        /// <summary>
        ///     False: It is a sample visualizer; True: It is a frequency visualizer;
        /// </summary>
        public bool IsFrequency;

        public ColorSetting Color;

        public override string ToString()
        {
            return (Is3D ? "[3D] " : "[2D] ") + (IsFrequency ? "Frequency" : "Sample");
        }
    }
}