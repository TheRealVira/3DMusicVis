#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Grid.cs
// Date - created:2016.12.10 - 09:37
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis._3DHelper
{
    /// <summary>
    ///     This is a game component that implements IUpdateable.
    /// </summary>
    public class Grid
    {
        private static BasicEffect _effect;
        private readonly RasterizerState MyStateSolid;
        private readonly RasterizerState MyStateWire;

        private float[,] _heightData;
        private int[] _indices;
        private int _terrainHeight;

        private int _terrainWidth;
        private VertexPositionColorNormal[] _vertices;

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

            //_effect = manager.Load<Effect>("Shader/Heightmap/Heightmap");
            _effect = new BasicEffect(Game1.Graphics.GraphicsDevice);
            _effect.EnableDefaultLighting();
            _effect.DirectionalLight0.Direction = new Vector3(1, 0, 0); // coming along the x-axis
            _effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights
            _effect.AmbientLightColor = new Vector3(.2f, .2f, .2f);
            _effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
        }

        private Point SetTerrainSpacing
        {
            set
            {
                _terrainWidth = value.X;
                _terrainHeight = value.Y;

                _indices = new int[(_terrainWidth - 1) * (_terrainHeight - 1) * 6];
                _vertices = new VertexPositionColorNormal[_terrainWidth * _terrainHeight];
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
            SetUpNormals();
        }

        public void Draw(GraphicsDevice device, Camera cam, DrawMode mode)
        {
            if (_vertices == null) return;

            device.BeginRender3D();

            var temp = device.RasterizerState;

            device.RasterizerState = mode.Equals(DrawMode.Connected) ? MyStateSolid : MyStateWire;

            //_effect.CurrentTechnique = _effect.Techniques["Colored"];
            //_effect.Parameters["xView"].SetValue(cam.View);
            //_effect.Parameters["xProjection"].SetValue(cam.Projektion);
            //_effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            //_effect.Parameters["xEnableLighting"].SetValue(true);
            //_effect.Parameters["xLightDirection"].SetValue(new Vector3(-1,10,0));
            //_effect.Parameters["xAmbient"].SetValue(.3f);
            //_effect.Parameters["xShowNormals"].SetValue(true);
            //_effect.Parameters["xCamPos"].SetValue(cam.Position);
            //_effect.Parameters["xCamUp"].SetValue(Vector3.Up);
            //_effect.Parameters["xPointSpriteSize"].SetValue(1f);

            //foreach (var pass in _effect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();

            _effect.World = Matrix.Identity;
            _effect.View = cam.View;
            _effect.Projection = cam.Projektion;
            _effect.CurrentTechnique.Passes[0].Apply();

            device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0,
                _indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);


            //}

            device.RasterizerState = temp;
            device.DepthStencilState = new DepthStencilState {DepthBufferEnable = false};
        }

        private void SetUpVertices()
        {
            if (_vertices == null) return;

            for (var x = 0; x < _terrainWidth; x++)
            for (var y = 0; y < _terrainHeight; y++)
            {
                var cur = _vertices[x + y * _terrainWidth];
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


                cur.Color = Color.White * cur.Position.Y;

                _vertices[x + y * _terrainWidth] = cur;
            }
        }

        private void SetUpIndices()
        {
            if (_indices == null) return;

            var counter = 0;
            for (var y = 0; y < _terrainHeight - 1; y++)
            for (var x = 0; x < _terrainWidth - 1; x++)
            {
                var lowerLeft = x + y * _terrainWidth;
                var lowerRight = x + 1 + y * _terrainWidth;
                var topLeft = x + (y + 1) * _terrainWidth;
                var topRight = x + 1 + (y + 1) * _terrainWidth;

                _indices[counter++] = topLeft;
                _indices[counter++] = lowerRight;
                _indices[counter++] = lowerLeft;

                _indices[counter++] = topLeft;
                _indices[counter++] = topRight;
                _indices[counter++] = lowerRight;
            }
        }

        private void SetUpNormals()
        {
            if (_vertices == null) return;

            for (var i = 0; i < _indices.Length / 3; i++)
            {
                var index1 = _indices[i * 3];
                var index2 = _indices[i * 3 + 1];
                var index3 = _indices[i * 3 + 2];

                var side1 = _vertices[index1].Position - _vertices[index3].Position;
                var side2 = _vertices[index1].Position - _vertices[index2].Position;
                var normal = Vector3.Cross(side1, side2);

                _vertices[index1].Normal = normal;
                _vertices[index2].Normal = normal;
                _vertices[index3].Normal = normal;

                _vertices[index1].Normal.Normalize();
                _vertices[index2].Normal.Normalize();
                _vertices[index3].Normal.Normalize();
            }
        }

        private void LoadHeightData(float[,] heightMap)
        {
            if (heightMap.GetLength(0) == 0) return;

            if (_terrainWidth != heightMap.GetLength(0) || _terrainHeight != heightMap.GetLength(1))
                SetTerrainSpacing = new Point(heightMap.GetLength(0), heightMap.GetLength(1));

            for (var x = 0; x < _terrainWidth; x++)
            for (var y = 0; y < _terrainHeight; y++)
                _heightData[x, y] = heightMap[x, y] / 1.25f;
        }
    }
}