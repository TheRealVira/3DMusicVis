#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Setting.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    internal struct Setting
    {
        public string SettingName;
        public ShaderMode Shaders;
        public List<SettingsBundle> Bundles;

        public byte HasRenders { get; private set; }

        public Color BackgroundColor;

        public override string ToString()
        {
            return SettingName;
        }
    }
}