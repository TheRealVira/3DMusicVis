#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: SettingsBundle.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis.Setting.Visualizer
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

            return Is3D == temp.Is3D && HowIDraw == temp.HowIDraw && IsFrequency == temp.IsFrequency &&
                   Color == temp.Color;
        }

        public static bool operator ==(SettingsBundle a, SettingsBundle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SettingsBundle a, SettingsBundle b)
        {
            return !(a == b);
        }
    }
}