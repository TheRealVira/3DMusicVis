#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: VertexPositionColorNormal.cs
// Date - created:2016.11.15 - 15:10
// Date - current: 2016.11.26 - 14:25

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2._3DHelper
{
    public struct VertexPositionColorNormal
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float)*3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float)*3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            );
    }
}