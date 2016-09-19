﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: SettingsBundle.cs
// Date - created:2016.09.18 - 10:19
// Date - current: 2016.09.19 - 15:38

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis2.Setting
{
    [Serializable]
    internal struct SettingsBundle
    {
        public Transformation Trans;
        public TypeOfRenderer Type;
        public ColorSetting Color;
        public bool Dashed;

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}