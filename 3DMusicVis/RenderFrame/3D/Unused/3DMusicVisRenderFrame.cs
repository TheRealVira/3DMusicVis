#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: 3DMusicVisRenderFrame.cs
// Date - created:2017.04.14 - 09:23
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.RenderFrame._3D.Unused
{
    [Obsolete]
    internal struct _3DMusicVisRenderFrame
    {
        public RasterizerState RastState;
        public RenderToTexture Render;
        public Update UpdateRenderer;
        public Color ClearColor;
        public Color ForeGroundColor;
        public Color FadeOutColor;
        public ColorMode ColorMode;
        public RenderTarget2D FinalRenderTarget;
        public bool UpdateOnlyTheCenterOfATile;
        public float HightMultiplier;
    }

    internal delegate Texture2D RenderToTexture(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings);

    public delegate void Update(ReadOnlyCollection<float> samples);
}