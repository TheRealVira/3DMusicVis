#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: BloomManager.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.11.14 - 18:39

#endregion

#region Usings

using BloomPostprocess;
using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis2.Shader
{
    internal static class BloomManager
    {
        public static BloomComponent Bloom;

        public static void Initialize(Game game)
        {
            Bloom = new BloomComponent(game);
            game.Components.Add(Bloom);
            Bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
        }
    }
}