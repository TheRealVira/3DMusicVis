#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: BloomManager.cs
// Date - created:2017.04.14 - 11:32
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using Microsoft.Xna.Framework;

#endregion

namespace _3DMusicVis.Shader
{
    internal static class BloomManager
    {
        public static BloomComponent Bloom;

        public static void Initialize(Game game)
        {
            Bloom = new BloomComponent(game);
            game.Components.Add(Bloom);
            Bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
            //Bloom.Settings = BloomSettings.PresetSettings[0];
        }
    }
}