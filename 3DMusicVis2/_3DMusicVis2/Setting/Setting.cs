#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Setting.cs
// Date - created:2016.09.18 - 10:13
// Date - current: 2016.10.13 - 20:11

#endregion

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace _3DMusicVis2.Setting
{
    [Serializable]
    internal struct Setting
    {
        public string SettingName;
        public List<SettingsBundle> Bundles;

        public override string ToString()
        {
            return SettingName;
        }
    }
}