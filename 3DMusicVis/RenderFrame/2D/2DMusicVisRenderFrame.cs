#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: 2DMusicVisRenderFrame.cs
// Date - created:2017.04.14 - 09:21
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using _3DMusicVis.RenderFrame._3D.Unused;

#endregion

namespace _3DMusicVis.RenderFrame._2D
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
    }
}