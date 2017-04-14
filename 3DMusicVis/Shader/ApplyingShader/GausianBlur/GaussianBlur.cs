#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: GaussianBlur.cs
// Date - created:2017.04.14 - 11:10
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis.Shader
{
    /// <summary>
    ///     A Gaussian blur filter kernel class. A Gaussian blur filter kernel is
    ///     perfectly symmetrical and linearly separable. This means we can split
    ///     the full 2D filter kernel matrix into two smaller horizontal and
    ///     vertical 1D filter kernel matrices and then perform the Gaussian blur
    ///     in two passes. Contrary to what you might think performing the Gaussian
    ///     blur in this way is actually faster than performing the Gaussian blur
    ///     in a single pass using the full 2D filter kernel matrix.
    ///     <para>
    ///         The GaussianBlur class is intended to be used in conjunction with an
    ///         HLSL Gaussian blur shader. The following code snippet shows a typical
    ///         Effect file implementation of a Gaussian blur.
    ///         <code>
    ///  #define RADIUS  7
    ///  #define KERNEL_SIZE (RADIUS * 2 + 1)
    /// 
    ///  float weights[KERNEL_SIZE];
    ///  float2 offsets[KERNEL_SIZE];
    /// 
    ///  texture colorMapTexture;
    /// 
    ///  sampler2D colorMap = sampler_state
    ///  {
    ///      Texture = <![CDATA[<colorMapTexture>;]]>
    ///      MipFilter = Linear;
    ///      MinFilter = Linear;
    ///      MagFilter = Linear;
    ///  };
    /// 
    ///  float4 PS_GaussianBlur(float2 texCoord : TEXCOORD) : COLOR0
    ///  {
    ///      float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
    /// 
    ///      <![CDATA[for (int i = 0; i < KERNEL_SIZE; ++i)]]>
    ///          color += tex2D(colorMap, texCoord + offsets[i]) * weights[i];
    ///  
    ///      return color;
    ///  }
    ///  
    ///  technique GaussianBlur
    ///  {
    ///      pass
    ///      {
    ///          PixelShader = compile ps_2_0 PS_GaussianBlur();
    ///      }
    ///  }
    ///  </code>
    ///         The RADIUS constant in the effect file must match the radius value in
    ///         the GaussianBlur class. The effect file's weights global variable
    ///         corresponds to the GaussianBlur class' kernel field. The effect file's
    ///         offsets global variable corresponds to the GaussianBlur class'
    ///         offsetsHoriz and offsetsVert fields.
    ///     </para>
    /// </summary>
    internal class GaussianBlur : ApplyShader
    {
        private readonly Effect effect;
        private readonly Game game;

        /// <summary>
        ///     This overloaded constructor instructs the GaussianBlur class to
        ///     load and use its GaussianBlur.fx effect file that implements the
        ///     two pass Gaussian blur operation on the GPU. The effect file must
        ///     be already bound to the asset name: 'Effects\GaussianBlur' or
        ///     'GaussianBlur'.
        /// </summary>
        public GaussianBlur(Effect eff = null)
            : base(eff ?? Game1.FreeBeer.Content.Load<Effect>("Shader/GaussianBlur/GaussianBlur"))
        {
            game = Game1.FreeBeer;
            effect = eff ?? game.Content.Load<Effect>("Shader/GaussianBlur/GaussianBlur");
        }

        /// <summary>
        ///     Returns the radius of the Gaussian blur filter kernel in pixels.
        /// </summary>
        public int Radius { get; private set; }

        /// <summary>
        ///     Returns the blur amount. This value is used to calculate the
        ///     Gaussian blur filter kernel's sigma value. Good values for this
        ///     property are 2 and 3. 2 will give a more blurred result whilst 3
        ///     will give a less blurred result with sharper details.
        /// </summary>
        public float Amount { get; private set; }

        /// <summary>
        ///     Returns the Gaussian blur filter's standard deviation.
        /// </summary>
        public float Sigma { get; private set; }

        /// <summary>
        ///     Returns the Gaussian blur filter kernel matrix. Note that the
        ///     kernel returned is for a 1D Gaussian blur filter kernel matrix
        ///     intended to be used in a two pass Gaussian blur operation.
        /// </summary>
        public float[] Kernel { get; private set; }

        /// <summary>
        ///     Returns the texture offsets used for the horizontal Gaussian blur
        ///     pass.
        /// </summary>
        public Vector2[] TextureOffsetsX { get; private set; }

        /// <summary>
        ///     Returns the texture offsets used for the vertical Gaussian blur
        ///     pass.
        /// </summary>
        public Vector2[] TextureOffsetsY { get; private set; }

        /// <summary>
        ///     Calculates the Gaussian blur filter kernel. This implementation is
        ///     ported from the original Java code appearing in chapter 16 of
        ///     "Filthy Rich Clients: Developing Animated and Graphical Effects for
        ///     Desktop Java".
        /// </summary>
        /// <param name="blurRadius">The blur radius in pixels.</param>
        /// <param name="blurAmount">Used to calculate sigma.</param>
        public void ComputeKernel(int blurRadius, float blurAmount)
        {
            Radius = blurRadius;
            Amount = blurAmount;

            Kernel = null;
            Kernel = new float[Radius * 2 + 1];
            Sigma = Radius / Amount;

            var twoSigmaSquare = 2.0f * Sigma * Sigma;
            var sigmaRoot = (float) Math.Sqrt(twoSigmaSquare * Math.PI);
            var total = 0.0f;
            var distance = 0.0f;
            var index = 0;

            for (var i = -Radius; i <= Radius; ++i)
            {
                distance = i * i;
                index = i + Radius;
                Kernel[index] = (float) Math.Exp(-distance / twoSigmaSquare) / sigmaRoot;
                total += Kernel[index];
            }

            for (var i = 0; i < Kernel.Length; ++i)
                Kernel[i] /= total;
        }

        /// <summary>
        ///     Calculates the texture coordinate offsets corresponding to the
        ///     calculated Gaussian blur filter kernel. Each of these offset values
        ///     are added to the current pixel's texture coordinates in order to
        ///     obtain the neighboring texture coordinates that are affected by the
        ///     Gaussian blur filter kernel. This implementation has been adapted
        ///     from chapter 17 of "Filthy Rich Clients: Developing Animated and
        ///     Graphical Effects for Desktop Java".
        /// </summary>
        /// <param name="textureWidth">The texture width in pixels.</param>
        /// <param name="textureHeight">The texture height in pixels.</param>
        public void ComputeOffsets(float textureWidth, float textureHeight)
        {
            TextureOffsetsX = null;
            TextureOffsetsX = new Vector2[Radius * 2 + 1];

            TextureOffsetsY = null;
            TextureOffsetsY = new Vector2[Radius * 2 + 1];

            var index = 0;
            var xOffset = 1.0f / textureWidth;
            var yOffset = 1.0f / textureHeight;

            for (var i = -Radius; i <= Radius; ++i)
            {
                index = i + Radius;
                TextureOffsetsX[index] = new Vector2(i * xOffset, 0.0f);
                TextureOffsetsY[index] = new Vector2(0.0f, i * yOffset);
            }
        }

        /// <summary>
        ///     Performs the Gaussian blur operation on the source texture image.
        ///     The Gaussian blur is performed in two passes: a horizontal blur
        ///     pass followed by a vertical blur pass. The output from the first
        ///     pass is rendered to renderTarget1. The output from the second pass
        ///     is rendered to renderTarget2. The dimensions of the blurred texture
        ///     is therefore equal to the dimensions of renderTarget2.
        /// </summary>
        /// <param name="srcTexture">The source image to blur.</param>
        /// <param name="renderTarget1">Stores the output from the horizontal blur pass.</param>
        /// <param name="renderTarget2">Stores the output from the vertical blur pass.</param>
        /// <param name="spriteBatch">Used to draw quads for the blur passes.</param>
        /// <returns>The resulting Gaussian blurred image.</returns>
        public Texture2D PerformGaussianBlur(Texture2D srcTexture,
            RenderTarget2D renderTarget1,
            RenderTarget2D renderTarget2,
            SpriteBatch spriteBatch)
        {
            if (effect == null)
                throw new InvalidOperationException("GaussianBlur.fx effect not loaded.");

            Texture2D outputTexture = null;
            var srcRect = new Rectangle(0, 0, srcTexture.Width, srcTexture.Height);
            var destRect1 = new Rectangle(0, 0, renderTarget1.Width, renderTarget1.Height);
            var destRect2 = new Rectangle(0, 0, renderTarget2.Width, renderTarget2.Height);

            // Perform horizontal Gaussian blur.

            game.GraphicsDevice.SetRenderTarget(renderTarget1);

            effect.CurrentTechnique = effect.Techniques["GaussianBlur"];
            effect.Parameters["weights"].SetValue(Kernel);
            effect.Parameters["colorMapTexture"].SetValue(srcTexture);
            effect.Parameters["offsets"].SetValue(TextureOffsetsX);

            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(srcTexture, destRect1, Color.White);
            spriteBatch.End();

            // Perform vertical Gaussian blur.

            game.GraphicsDevice.SetRenderTarget(renderTarget2);
            outputTexture = renderTarget1;

            effect.Parameters["colorMapTexture"].SetValue(outputTexture);
            effect.Parameters["offsets"].SetValue(TextureOffsetsY);

            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            spriteBatch.Draw(outputTexture, destRect2, Color.White);
            spriteBatch.End();

            // Return the Gaussian blurred texture.

            game.GraphicsDevice.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
            outputTexture = renderTarget2;

            return outputTexture;
        }

        public override void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            params object[] paramArray)
        {
            // GaussianBlur the bloom

            toUse = (RenderTarget2D) GaussianBlurManager.Compute(toUse, sB);
        }
    }
}