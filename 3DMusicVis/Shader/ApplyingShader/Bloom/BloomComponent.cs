#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: BloomComponent.cs
// Date - created:2017.04.14 - 11:32
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;

#endregion

namespace _3DMusicVis.Shader
{
    public class BloomComponent : DrawableGameComponent
    {
        #region Fields

        private SpriteBatch spriteBatch;

        private Effect bloomExtractEffect;
        private Effect bloomCombineEffect;
        private Effect gaussianBlurEffect;

        private RenderTarget2D sceneRenderTarget;
        private RenderTarget2D renderTarget1;
        private RenderTarget2D renderTarget2;


        public RenderTarget2D FinalRenderTarget { get; private set; }

        // Choose what display settings the bloom should use.
        public BloomSettings Settings { get; set; } = BloomSettings.PresetSettings[0];


        // Optionally displays one of the intermediate buffers used
        // by the bloom postprocess, so you can see exactly what is
        // being drawn into each rendertarget.
        public enum IntermediateBuffer
        {
            PreBloom,
            BlurredHorizontally,
            BlurredBothWays,
            FinalResult
        }

        public IntermediateBuffer ShowBuffer { get; set; } = IntermediateBuffer.FinalResult;

        #endregion

        #region Initialization

        public BloomComponent(Game game)
            : base(game)
        {
            if (game == null)
                throw new ArgumentNullException("game");
        }


        /// <summary>
        ///     Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bloomExtractEffect = Game.Content.Load<Effect>("Shader/Bloom/BloomExtract");
            bloomCombineEffect = Game.Content.Load<Effect>("Shader/Bloom/BloomCombine");
            gaussianBlurEffect = Game.Content.Load<Effect>("Shader/Bloom/GaussianBlur");

            // Look up the resolution and format of our main backbuffer.
            var pp = GraphicsDevice.PresentationParameters;

            var width = ResolutionManager.VIRTUAL_RESOLUTION.Width;
            var height = ResolutionManager.VIRTUAL_RESOLUTION.Height;

            var format = pp.BackBufferFormat;

            // Create a texture for rendering the main scene, prior to applying bloom.
            sceneRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
                format, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.DiscardContents);

            FinalRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
                format, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.DiscardContents);

            // Create two rendertargets for the bloom processing. These are half the
            // size of the backbuffer, in order to minimize fillrate costs. Reducing
            // the resolution in this way doesn't hurt quality, because we are going
            // to be blurring the bloom images in any case.
            width /= 2;
            height /= 2;

            renderTarget1 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
        }


        /// <summary>
        ///     Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            sceneRenderTarget.Dispose();
            renderTarget1.Dispose();
            renderTarget2.Dispose();
        }

        #endregion

        #region Draw

        /// <summary>
        ///     This should be called at the very start of the scene rendering. The bloom
        ///     component uses it to redirect drawing into its custom rendertarget, so it
        ///     can capture the scene image in preparation for applying the bloom filter.
        /// </summary>
        public void BeginDraw()
        {
            if (Visible)
                GraphicsDevice.SetRenderTarget(sceneRenderTarget);
        }


        /// <summary>
        ///     This is where it all happens. Grabs a scene that has already been rendered,
        ///     and uses postprocess magic to add a glowing bloom effect over the top of it.
        /// </summary>
        public void EndDraw()
        {
            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            // Pass 1: draw the scene into rendertarget 1, using a
            // shader that extracts only the brightest parts of the image.
            bloomExtractEffect.Parameters["BloomThreshold"].SetValue(
                Settings.BloomThreshold);

            DrawFullscreenQuad(sceneRenderTarget, renderTarget1,
                bloomExtractEffect,
                IntermediateBuffer.PreBloom);

            // Pass 2: draw from rendertarget 1 into rendertarget 2,
            // using a shader to apply a horizontal gaussian blur filter.
            SetBlurEffectParameters(1.0f / renderTarget1.Width, 0);

            DrawFullscreenQuad(renderTarget1, renderTarget2,
                gaussianBlurEffect,
                IntermediateBuffer.BlurredHorizontally);

            // Pass 3: draw from rendertarget 2 back into rendertarget 1,
            // using a shader to apply a vertical gaussian blur filter.
            SetBlurEffectParameters(0, 1.0f / renderTarget1.Height);

            DrawFullscreenQuad(renderTarget2, renderTarget1,
                gaussianBlurEffect,
                IntermediateBuffer.BlurredBothWays);

            // Pass 4: draw both rendertarget 1 and the original scene
            // image back into the main backbuffer, using a shader that
            // combines them to produce the final bloomed result.
            GraphicsDevice.SetRenderTarget(FinalRenderTarget);

            var parameters = bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(Settings.BloomIntensity);
            parameters["BaseIntensity"].SetValue(Settings.BaseIntensity);
            parameters["BloomSaturation"].SetValue(Settings.BloomSaturation);
            parameters["BaseSaturation"].SetValue(Settings.BaseSaturation);

            GraphicsDevice.Textures[1] = sceneRenderTarget;

            var viewport = GraphicsDevice.Viewport;

            DrawFullscreenQuad(renderTarget1,
                viewport.Width, viewport.Height,
                bloomCombineEffect,
                IntermediateBuffer.FinalResult);

            GraphicsDevice.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
        }


        /// <summary>
        ///     Helper for drawing a texture into a rendertarget, using
        ///     a custom shader to apply postprocessing effects.
        /// </summary>
        private void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget,
            Effect effect, IntermediateBuffer currentBuffer)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            DrawFullscreenQuad(texture,
                renderTarget.Width, renderTarget.Height,
                effect, currentBuffer);
        }


        /// <summary>
        ///     Helper for drawing a texture into the current rendertarget,
        ///     using a custom shader to apply postprocessing effects.
        /// </summary>
        private void DrawFullscreenQuad(Texture2D texture, int width, int height,
            Effect effect, IntermediateBuffer currentBuffer)
        {
            // If the user has selected one of the show intermediate buffer options,
            // we still draw the quad to make sure the image will end up on the screen,
            // but might need to skip applying the custom pixel shader.
            if (ShowBuffer < currentBuffer)
                effect = null;

            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();
        }


        /// <summary>
        ///     Computes sample weightings and texture coordinate offsets
        ///     for one pass of a separable gaussian blur filter.
        /// </summary>
        private void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

            // Look up how many samples our gaussian blur effect supports.
            var sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            var sampleWeights = new float[sampleCount];
            var sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            var totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (var i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                var weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                var sampleOffset = i * 2 + 1.5f;

                var delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (var i = 0; i < sampleWeights.Length; i++)
                sampleWeights[i] /= totalWeights;

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }


        /// <summary>
        ///     Evaluates a single point on the gaussian falloff curve.
        ///     Used for setting up the blur filter weightings.
        /// </summary>
        private float ComputeGaussian(float n)
        {
            var theta = Settings.BlurAmount;

            return (float) (1.0 / Math.Sqrt(2 * Math.PI * theta) *
                            Math.Exp(-(n * n) / (2 * theta * theta)));
        }

        #endregion
    }
}