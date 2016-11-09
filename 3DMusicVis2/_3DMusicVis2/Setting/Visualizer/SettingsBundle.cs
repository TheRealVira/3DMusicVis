﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: SettingsBundle.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    public struct SettingsBundle
    {
        public Transformation Trans;

        /// <summary>
        ///     False: It is a 2D visualizer; True: It is a 3D visualizer;
        /// </summary>
        public bool Is3D;

        /// <summary>
        ///     False: It is a non dashed visualizer; True: It is a dashed visualizer;
        /// </summary>
        public DrawMode HowIDraw;

        /// <summary>
        ///     False: It is a sample visualizer; True: It is a frequency visualizer;
        /// </summary>
        public bool IsFrequency;

        public ColorSetting Color;

        public bool VerticalMirror;

        public bool HorizontalMirror;

        public override string ToString()
        {
            return (Is3D ? "3" : "2") + (IsFrequency ? "F" : "S") + HowIDraw;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(SettingsBundle)) return false;

            var temp = (SettingsBundle) obj;

            return (Is3D == temp.Is3D) && (HowIDraw == temp.HowIDraw) && (IsFrequency == temp.IsFrequency) &&
                   (Color == temp.Color);
        }

        public static bool operator == (SettingsBundle a, SettingsBundle b)
        {
            return a.Equals(b);
        }

        public static bool operator != (SettingsBundle a, SettingsBundle b)
        {
            return !(a == b);
        }
    }
}