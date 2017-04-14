#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: SettingsBundle.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis.Setting.Visualizer
{
    [Serializable]
    public struct SettingsBundle
    {
        public bool Equals(SettingsBundle other)
        {
            return Trans.Equals(other.Trans) && Is3D == other.Is3D && HowIDraw == other.HowIDraw &&
                   IsFrequency == other.IsFrequency && Color.Equals(other.Color) &&
                   VerticalMirror == other.VerticalMirror && HorizontalMirror == other.HorizontalMirror;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Trans.GetHashCode();
                hashCode = (hashCode * 397) ^ Is3D.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) HowIDraw;
                hashCode = (hashCode * 397) ^ IsFrequency.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ VerticalMirror.GetHashCode();
                hashCode = (hashCode * 397) ^ HorizontalMirror.GetHashCode();
                return hashCode;
            }
        }

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