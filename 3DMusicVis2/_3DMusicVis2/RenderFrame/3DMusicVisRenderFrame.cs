﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 3DMusicVisRenderFrame.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.11 - 17:35

#endregion

#region Usings

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    internal struct _3DMusicVisRenderFrame
    {
        public RasterizerState RastState;
        public RenderToTexture Render;
        public Update UpdateRenderer;
        public Color ClearColor;
        public Color ForeGroundColor;
        public Color FadeOutColor;
        public ColorMode ColorMode;
        public bool UpdateOnlyTheCenterOfATile;
        public float HightMultiplier;
    }

    public delegate Texture2D RenderToTexture(GraphicsDevice device, GameTime gameTime, Camera cam);

    public delegate void Update(ReadOnlyCollection<float> samples);
}