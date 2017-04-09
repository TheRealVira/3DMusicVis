#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: 2DMusicVisRenderFrame.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using Microsoft.Xna.Framework;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.RenderFrame
{
    internal struct _2DMusicVisRenderFrame
    {
        public RenderToTexture Render;
        public Update UpdateRenderer;
        public Color ClearColor;
        public Color ForeGroundColor;
        public Color FadeOutColor;
        public ColorMode ColorMode;
        public float HightMultiplier;

        public Transformation Trans;
    }
}