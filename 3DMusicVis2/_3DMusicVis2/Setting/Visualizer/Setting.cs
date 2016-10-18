#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Setting.cs
// Date - created:2016.10.18 - 17:49
// Date - current: 2016.10.18 - 18:21

#endregion

#region Usings

using System;
using System.Collections.Generic;

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

        public override string ToString()
        {
            return SettingName;
        }
    }
}