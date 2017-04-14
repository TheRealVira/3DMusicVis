#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ApplyShader.cs
// Date - created:2017.04.14 - 10:42
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Shader
{
    /// <summary>
    ///     NOTE: The classname must have the same name as the shader file!!!
    /// </summary>
    internal abstract class ApplyShader
    {
        protected readonly Effect MyEffect;

        protected readonly RenderTarget2D MyRenderTarget;

        protected ApplyShader(Effect eff)
        {
            MyRenderTarget = new RenderTarget2D(Game1.FreeBeer.GraphicsDevice,
                ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);

            MyEffect = eff;
        }

        public abstract void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            params object[] paramArray);
    }
}