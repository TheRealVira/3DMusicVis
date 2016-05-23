#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Screen.cs
// Date - created: 2016.05.19 - 17:57
// Date - current: 2016.05.23 - 21:16

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.Screen
{
    abstract class Screen
    {
        public readonly string Name;
        protected GraphicsDeviceManager GDM;

        protected Screen(GraphicsDeviceManager gdm, string name)
        {
            GDM = gdm;
            Name = name;
            Console.WriteLine($"Initialising {name}...");
        }

        public float Alpha { get; }

        public bool ShouldBeDeleted { get; protected set; }
        public abstract void Draw(SpriteBatch sB, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public virtual void LoadedUp()
        {
        }
    }
}