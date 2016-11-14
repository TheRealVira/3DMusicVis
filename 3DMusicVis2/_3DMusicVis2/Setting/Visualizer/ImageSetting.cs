#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ImageSetting.cs
// Date - created:2016.11.10 - 18:57
// Date - current: 2016.11.14 - 18:39

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    internal class ImageSetting
    {
        public string ImageFileName;
        public ImageMode Mode;
        public Vector2 Offset;

        public bool ReverseRotation;
        public float Rotation;
        public float RotationNotice = .8f;
        public float RotationSpeedMutliplier = 1f;
        public Color Tint;
    }
}