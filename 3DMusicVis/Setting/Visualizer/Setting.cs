#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Setting.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis.Setting.Visualizer
{
    [Serializable]
    internal struct Setting
    {
        public string SettingName;
        public ShaderMode Shaders;
        public List<SettingsBundle> Bundles;

        public byte HasRenders { get; set; }

        public ColorSetting BackgroundColor;

        public ImageSetting BackgroundImage;
        public ImageSetting ForegroundImage;

        public override string ToString()
        {
            return SettingName;
        }
    }
}