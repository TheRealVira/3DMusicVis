#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: InputManager.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace _3DMusicVis.Manager
{
    internal static class InputManager
    {
        public static Vector2 MousePosition;

        public static void UpdateMousePosition(MouseState state, GraphicsDeviceManager graphics)
        {
            MousePosition = new Vector2(state.X, state.Y) * ResolutionManager.ResolutionRatio;
        }

        public static bool KeyWasClicked(this Keys key)
            => Game1.NewKeyboardState.IsKeyUp(key) && Game1.OldKeyboardState.IsKeyDown(key);
    }
}