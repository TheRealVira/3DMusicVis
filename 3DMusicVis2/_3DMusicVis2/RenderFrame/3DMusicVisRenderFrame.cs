#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 3DMusicVisRenderFrame.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.11.26 - 14:25

#endregion

#region Usings

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Setting.Visualizer;

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
        public RenderTarget2D FinalRenderTarget;
        public bool UpdateOnlyTheCenterOfATile;
        public float HightMultiplier;
    }

    internal delegate Texture2D RenderToTexture(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings);

    public delegate void Update(ReadOnlyCollection<float> samples);
}