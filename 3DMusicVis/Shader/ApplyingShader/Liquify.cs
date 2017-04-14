#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Liquify.cs
// Date - created:2017.04.14 - 10:43
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Shader.ApplyingShader
{
    internal class Liquify : ApplyShader
    {
        public Liquify(Effect eff) : base(eff)
        {
        }

        public override void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            params object[] paramArray)
        {
            graphics.SetRenderTarget(MyRenderTarget);
            sB.GraphicsDevice.Clear(Color.Transparent);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                MyEffect);
            MyEffect.Parameters["width"].SetValue( /*.5f*/0.2f);
            MyEffect.Parameters["toBe"].SetValue(((Color) paramArray[0]).Negate().ToVector4());
            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();

            toUse = MyRenderTarget;
        }
    }
}