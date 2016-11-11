using System;
using Microsoft.Xna.Framework;

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    internal class ImageSetting
    {
        public string ImageFileName;
        public ImageMode Mode;
        public Color Tint;
        public float RotationSpeedMutliplier = 1f;
        public float RotationNotice = .8f;
        public Vector2 Offset;

        public bool ReverseRotation;
        public float Rotation;
    }
}