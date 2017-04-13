#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ShadersManager.cs
// Date - created:2017.04.13 - 12:31
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis.Shader
{
    internal static class ShadersManager
    {
        /// <summary>
        ///     If true: Do not initialize
        /// </summary>
        private static bool Singelton;

        public static Dictionary<string, Effect> ShaderDictionary { get; private set; }

        public static void Initialize()
        {
            if (Singelton)
                return;

            Singelton = true;
            ShaderDictionary = Game1.FreeBeer.Content.LoadListContent<Effect>(SettingsManager.SHADER_DIR);
        }
    }
}