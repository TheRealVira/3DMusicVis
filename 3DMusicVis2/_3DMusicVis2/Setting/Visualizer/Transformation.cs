﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Transformation.cs
// Date - created:2016.10.18 - 17:49
// Date - current: 2016.10.19 - 19:59

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2.Setting.Visualizer
{
    [Serializable]
    internal struct Transformation
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