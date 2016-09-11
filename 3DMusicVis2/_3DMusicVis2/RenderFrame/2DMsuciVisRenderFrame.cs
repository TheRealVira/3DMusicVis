#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 2DMsuciVisRenderFrame.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.11 - 17:35

#endregion

#region Usings

using Microsoft.Xna.Framework;

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
    }
}