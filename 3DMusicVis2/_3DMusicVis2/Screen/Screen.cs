#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Screen.cs
// Date - created: 2016.05.19 - 17:57
// Date - current: 2016.05.22 - 12:52

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.Screen
{
    abstract class Screen
    {
        protected GraphicsDeviceManager GDM;

        protected Screen(GraphicsDeviceManager gdm)
        {
            GDM = gdm;
        }

        public float Alpha { get; }

        public bool ShouldBeDeleted { get; protected set; }
        public abstract void Draw(SpriteBatch sB, GameTime gameTime);

        public abstract void Update(GameTime gameTime);
    }
}