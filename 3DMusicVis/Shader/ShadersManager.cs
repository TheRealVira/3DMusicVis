#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ShadersManager.cs
// Date - created:2017.04.13 - 12:31
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.Shader
{
    internal static class ShadersManager
    {
        /// <summary>
        ///     If true: Do not initialize
        /// </summary>
        private static bool Singelton;

        public static Dictionary<ShaderMode, ApplyShader> ShaderDictionary { get; private set; }

        public static void Initialize(ContentManager content)
        {
            if (Singelton)
                return;

            ShaderDictionary = new Dictionary<ShaderMode, ApplyShader>();

            Singelton = true;
            var type = typeof(ApplyShader);
            //AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(s => s.GetTypes())
            //    .Where(p => !p.IsAbstract && type.IsAssignableFrom(p))
            //    .AsParallel()
            //    .ForAll(x =>
            //    {
            //        ShaderMode key;
            //        Enum.TryParse(x.Name, out key);
            //        ShaderDictionary.Add(key,
            //            (ApplyShader)
            //            Activator.CreateInstance(x, content.Load<Effect>(SettingsManager.SHADER_DIR + "\\" + x.Name)));
            //    });

            foreach (var type1 in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsAbstract && type.IsAssignableFrom(p)))
            {
                Enum.TryParse(type1.Name, out ShaderMode key);

                if (key.Equals(ShaderMode.None))
                    continue;

                ShaderDictionary.Add(key,
                    (ApplyShader)
                    Activator.CreateInstance(type1,
                        File.Exists(content.RootDirectory + "\\" + SettingsManager.SHADER_DIR + "\\" + type1.Name +
                                    ".xnb")
                            ? content.Load<Effect>(SettingsManager.SHADER_DIR + "\\" + type1.Name)
                            : null));
            }
        }
    }
}