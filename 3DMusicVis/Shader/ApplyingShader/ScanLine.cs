#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ScanLine.cs
// Date - created:2017.04.14 - 11:29
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Shader.ApplyingShader
{
    internal class ScanLine : ApplyShader
    {
        public ScanLine(Effect eff) : base(eff)
        {
        }

        public override void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            params object[] paramArray)
        {
            graphics.SetRenderTarget(MyRenderTarget);
            sB.GraphicsDevice.Clear(Color.Transparent);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                MyEffect);
            MyEffect.Parameters["ImageHeight"].SetValue(
                ResolutionManager.VIRTUAL_RESOLUTION.Height);
            MyEffect.Parameters["LineColor"].SetValue(
                ((Color) paramArray[0]).ToVector4());
            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();

            toUse = MyRenderTarget;
        }
    }
}