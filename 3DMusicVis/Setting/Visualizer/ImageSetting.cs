#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ImageSetting.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis.Setting.Visualizer
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