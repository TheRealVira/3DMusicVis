#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Transformation.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis.Setting.Visualizer
{
    [Serializable]
    public struct Transformation
    {
        public Vector2 Position;

        // NOTE: v(1,1) would be the default vector
        public Vector2 Scale;
        public float Rotation;

        public Transformation(Vector2 position, float rotation)
        {
            Rotation = rotation;
            Position = position;
            Scale = Vector2.One;
        }

        public Transformation(Vector2 position, Vector2 scale, float rotation)
        {
            Rotation = rotation;
            Position = position;
            Scale = scale;
        }
    }
}