#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Grid.cs
// Date - created:2016.11.13 - 15:30
// Date - current: 2016.11.14 - 18:39

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Setting.Visualizer;

#endregion

namespace _3DMusicVis2._3DHelper
{
    /// <summary>
    ///     This is a game component that implements IUpdateable.
    /// </summary>
    public class Grid
    {
        private static Effect _effect;
        private readonly RasterizerState MyStateSolid;
        private readonly RasterizerState MyStateWire;

        private float[,] _heightData;
        private int[] _indices;
        private int _terrainHeight;

        private int _terrainWidth;
        private VertexPositionColor[] _vertices;

        public Grid(ContentManager manager)
        {
            MyStateSolid = new RasterizerState
            {
                FillMode = FillMode.Solid
            };

            MyStateWire = new RasterizerState
            {
                FillMode = FillMode.WireFrame
            };

            if (_effect != null) return;

            _effect = manager.Load<Effect>("Shader/Heightmap/Heightmap");
        }

        private Point SetTerrainSpacing
        {
            set
            {
                _terrainWidth = value.X;
                _terrainHeight = value.Y;

                _indices = new int[(_terrainWidth - 1)*(_terrainHeight - 1)*6];
                _vertices = new VertexPositionColor[_terrainWidth*_terrainHeight];
                _heightData = new float[_terrainWidth, _terrainHeight];
            }
        }

        public void Dispose()
        {
            MyStateSolid.Dispose();
            MyStateWire.Dispose();
        }

        public void Update(float[,] heightmap)
        {
            LoadHeightData(heightmap);
            SetUpVertices();
            SetUpIndices();
        }

        public void Draw(GraphicsDevice device, Camera cam, DrawMode mode)
        {
            if (_vertices == null) return;

            var temp = device.RasterizerState;

            device.RasterizerState = mode.Equals(DrawMode.Connected) ? MyStateSolid : MyStateWire;

            _effect.CurrentTechnique = _effect.Techniques["ColoredNoShading"];
            _effect.Parameters["xView"].SetValue(cam.View);
            _effect.Parameters["xProjection"].SetValue(cam.Projektion);
            _effect.Parameters["xWorld"].SetValue(Matrix.Identity);

            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.SamplerStates[0] = SamplerState.LinearWrap;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0,
                    _indices.Length/3, VertexPositionColor.VertexDeclaration);
            }

            device.RasterizerState = temp;
        }

        private void SetUpVertices()
        {
            for (var x = 0; x < _terrainWidth; x++)
            {
                for (var y = 0; y < _terrainHeight; y++)
                {
                    var cur = _vertices[x + y*_terrainWidth];
                    cur.Position.X = x;
                    cur.Position.Y = _heightData[x, y];
                    cur.Position.Z = -y;

                    //if (_heightData[x, y] < minHeight + (maxHeight - minHeight) / 4)
                    //    _vertices[x + y * _terrainWidth].Color = Color.DodgerBlue;
                    //else if (_heightData[x, y] < minHeight + (maxHeight - minHeight) * 2 / 4)
                    //    _vertices[x + y * _terrainWidth].Color = Color.Green;
                    //else if (_heightData[x, y] < minHeight + (maxHeight - minHeight) * 3 / 4)
                    //    _vertices[x + y * _terrainWidth].Color = Color.DarkGray;
                    //else
                    cur.Color = Color.White;

                    _vertices[x + y*_terrainWidth] = cur;
                }
            }
        }

        private void SetUpIndices()
        {
            var counter = 0;
            for (var y = 0; y < _terrainHeight - 1; y++)
            {
                for (var x = 0; x < _terrainWidth - 1; x++)
                {
                    var lowerLeft = x + y*_terrainWidth;
                    var lowerRight = x + 1 + y*_terrainWidth;
                    var topLeft = x + (y + 1)*_terrainWidth;
                    var topRight = x + 1 + (y + 1)*_terrainWidth;

                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = lowerLeft;

                    _indices[counter++] = topLeft;
                    _indices[counter++] = topRight;
                    _indices[counter++] = lowerRight;
                }
            }
        }

        private void LoadHeightData(float[,] heightMap)
        {
            if (_terrainWidth != heightMap.GetLength(0) || _terrainHeight != heightMap.GetLength(1))
            {
                SetTerrainSpacing = new Point(heightMap.GetLength(0), heightMap.GetLength(1));
            }

            for (var x = 0; x < _terrainWidth; x++)
            {
                for (var y = 0; y < _terrainHeight; y++)
                {
                    _heightData[x, y] = heightMap[x, y]/1.25f;
                }
            }
        }
    }
}