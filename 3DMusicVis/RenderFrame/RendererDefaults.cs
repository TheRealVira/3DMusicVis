#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: RendererDefaults.cs
// Date - created:2017.04.13 - 12:55
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.RenderFrame
{
    internal static class RendererDefaults
    {
        public delegate void DrawGraphicsOnRenderTarget(
            GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings, ref RenderTarget2D tex);
    }
}