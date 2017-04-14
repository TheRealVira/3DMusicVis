#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Bloom.cs
// Date - created:2017.04.14 - 11:32
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Shader.ApplyingShader.Bloom
{
    internal class Bloom : ApplyShader
    {
        public Bloom(Effect eff) : base(null)
        {
        }

        public override void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            params object[] paramArray)
        {
            // Apply bloom
            BloomManager.Bloom.BeginDraw();
            sB.GraphicsDevice.Clear(Color.Transparent);
            // Applying shader
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();
            BloomManager.Bloom.EndDraw();

            graphics.SetRenderTarget(MyRenderTarget);
            sB.GraphicsDevice.Clear(Color.Transparent);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            sB.Draw(BloomManager.Bloom.FinalRenderTarget, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();

            toUse = MyRenderTarget;
        }
    }
}