#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: 3DMusicVisRenderFrame.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.RenderFrame
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