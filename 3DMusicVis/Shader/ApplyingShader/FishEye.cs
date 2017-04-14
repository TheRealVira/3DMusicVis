#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: FishEye.cs
// Date - created:2017.04.14 - 11:41
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Shader.ApplyingShader
{
    internal class FishEye : ApplyShader
    {
        public FishEye(Effect eff) : base(eff)
        {
        }

        public override void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            params object[] paramArray)
        {
            /*
             * float distance;
             * float distortion;
             * float3 colorDist;
             */

            graphics.SetRenderTarget(MyRenderTarget);
            sB.GraphicsDevice.Clear(Color.Transparent);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null,
                MyEffect);
            MyEffect.Parameters["distance"].SetValue(.1f);
            MyEffect.Parameters["distortion"].SetValue(.5f);
            MyEffect.Parameters["colorDist"].SetValue(new Vector3(.005f, 0, -.005f));
            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();

            toUse = MyRenderTarget;
        }
    }
}