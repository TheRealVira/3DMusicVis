#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: GaussianBlurManager.cs
// Date - created:2017.04.14 - 11:10
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Shader
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

        private static GaussianBlur _myGaussianBlur;

        public static void Initialize(GraphicsDevice gd, Game game)
        {
            _myGaussianBlur = new GaussianBlur();
            _myGaussianBlur.ComputeKernel(BLUR_RADIUS, BLUR_AMOUNT);
            _myGaussianBlur.ComputeOffsets(ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);

            var renderTargetWidth = ResolutionManager.VIRTUAL_RESOLUTION.Width / 2;
            var renderTargetHeight = ResolutionManager.VIRTUAL_RESOLUTION.Height / 2;
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
            return _myGaussianBlur.PerformGaussianBlur(texture, renderTarget1, renderTarget2, sB);
        }
    }
}