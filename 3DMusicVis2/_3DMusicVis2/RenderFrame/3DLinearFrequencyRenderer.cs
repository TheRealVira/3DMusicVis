#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: 3DLinearFrequencyRenderer.cs
// Date - created:2016.10.14 - 18:07
// Date - current: 2016.10.19 - 19:59

#endregion

#region Usings

using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.RenderFrame
{
    internal static class _3DLinearFrequencyRenderer
    {
        private static _3DMusicVisRenderFrame mFrame;

        public static float Width = 100;
        public static float MaxHeight = 1, MinHeight = -1;

        private static VertexPositionColor[] _vertices;
        private static int[] _indices;
        private static Effect myEffect;
        private static float[] _heightData;

        public static void Initialise(GraphicsDevice device)
        {
            mFrame =
                new _3DMusicVisRenderFrame
                {
                    Render = Target,
                    UpdateRenderer = UpdateRenderer,
                    ClearColor = Color.Transparent,
                    ColorMode = ColorMode.SideEqualsCenter,
                    FadeOutColor = Color.Black,
                    ForeGroundColor = Color.White,
                    HightMultiplier = 1.5f,
                    FinalRenderTarget =
                        new RenderTarget2D(device, Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height, true,
                            device.DisplayMode.Format, DepthFormat.Depth24)
                };

            myEffect = Game1.FreeBeer.Content.Load<Effect>("Shader/3DGrid");
            _heightData = new float[100];
            SetupIndices();
            SetupVerticies();
        }

        private static void SetupIndices()
        {
            _indices = new int[(int) ((Width - 1)*6)];
            var counter = 0;
            for (var x = 0; x < Width - 1; x++)
            {
                var lowerLeft = x;
                var lowerRight = x + 1;
                var topLeft = x + 1*Width;
                var topRight = x + 1 + 1*Width;

                _indices[counter++] = (int) topLeft;
                _indices[counter++] = lowerRight;
                _indices[counter++] = lowerLeft;

                _indices[counter++] = (int) topLeft;
                _indices[counter++] = (int) topRight;
                _indices[counter++] = lowerRight;
            }
        }

        private static void SetupVerticies()
        {
            _vertices = new VertexPositionColor[(int) Width];

            for (var x = 0; x < Width; x++)
            {
                _vertices[x].Position = new Vector3(x, _heightData[x], 0);

                if (_heightData[x] < (MaxHeight - MinHeight)/4)
                    _vertices[x].Color = Color.DodgerBlue;
                else if (_heightData[x] < MinHeight + (MaxHeight - MinHeight)*2/4)
                    _vertices[x].Color = Color.Green;
                else if (_heightData[x] < MinHeight + (MaxHeight - MinHeight)*3/4)
                    _vertices[x].Color = Color.DarkGray;
                else
                    _vertices[x].Color = Color.White;
            }
        }

        public static void UpdateRenderer(ReadOnlyCollection<float> samples)
        {
            _heightData = new float[samples.Count];

            for (var x = 0; x < Width; x++)
            {
                _heightData[x] = samples[x]/5.0f;
            }
        }

        public static Texture2D Target(GraphicsDevice device, GameTime gametime, Camera cam)
        {
            device.SetRenderTarget(mFrame.FinalRenderTarget);
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            var rasterState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.WireFrame
            };
            device.RasterizerState = rasterState;

            myEffect.CurrentTechnique = myEffect.Techniques["ColoredNoShading"];
            myEffect.Parameters["xView"].SetValue(cam.View);
            myEffect.Parameters["xProjection"].SetValue(cam.Projektion);
            myEffect.Parameters["xWorld"].SetValue(Matrix.CreateWorld(cam.Position, Vector3.Forward, Vector3.Up));

            foreach (var pass in myEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0,
                    _indices.Length/3, VertexPositionColor.VertexDeclaration);
            }

            device.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);

            return mFrame.FinalRenderTarget;
        }
    }
}