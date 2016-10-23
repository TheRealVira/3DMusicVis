#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: GaussianBlurManager.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.23 - 18:25

#endregion

#region Usings

using Dhpoware;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.Shader
{
    public static class GaussianBlurManager
    {
        // WARNING:
        // If you change the BLUR_RADIUS you *MUST* also change the RADIUS
        // constant in GaussianBlur.fx. Both values *MUST* be the same.
        private const int BLUR_RADIUS = 7;
        private const float BLUR_AMOUNT = 2.0f;
        private static RenderTarget2D renderTarget1;
        private static RenderTarget2D renderTarget2;

        private static GaussianBlur myBlur;

        public static void Initialize(GraphicsDevice gd, Game game)
        {
            myBlur = new GaussianBlur(game);
            myBlur.ComputeKernel(BLUR_RADIUS, BLUR_AMOUNT);

            var renderTargetWidth = Game1.VIRTUAL_RESOLUTION.Width/2;
            var renderTargetHeight = Game1.VIRTUAL_RESOLUTION.Height/2;
            renderTarget1 = new RenderTarget2D(gd,
                renderTargetWidth, renderTargetHeight, false,
                gd.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            renderTarget2 = new RenderTarget2D(gd,
                renderTargetWidth, renderTargetHeight, false,
                gd.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
        }

        public static Texture2D Compute(Texture2D texture, SpriteBatch sB)
        {
            return myBlur.PerformGaussianBlur(texture, renderTarget1, renderTarget2, sB);
        }
    }
}