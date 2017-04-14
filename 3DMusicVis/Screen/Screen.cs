#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Screen.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis.Screen
{
    internal abstract class Screen
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
        public bool IsInitialised { protected set; get; }

        public abstract void Draw(SpriteBatch sB, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public virtual void LoadedUp()
        {
            IsInitialised = true;
        }

        public virtual void Unloade()
        {
        }
    }
}