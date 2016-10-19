#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 2DMusicVisRenderFrame.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.10.19 - 19:59

#endregion

#region Usings

using Microsoft.Xna.Framework;
using _3DMusicVis2.Setting.Visualizer;

#endregion

namespace _3DMusicVis2.RenderFrame
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