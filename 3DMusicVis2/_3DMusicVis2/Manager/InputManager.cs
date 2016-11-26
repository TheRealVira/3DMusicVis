#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: InputManager.cs
// Date - created:2016.11.26 - 13:02
// Date - current: 2016.11.26 - 14:25

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace _3DMusicVis2.Manager
{
    internal static class InputManager
    {
        public static Vector2 MousePosition;

        public static void UpdateMousePosition(MouseState state, GraphicsDeviceManager graphics)
        {
            MousePosition = new Vector2(state.X, state.Y)*ResolutionManager.ResolutionRatio;
        }

        public static bool KeyWasClicked(this Keys key)
            => Game1.NewKeyboardState.IsKeyUp(key) && Game1.OldKeyboardState.IsKeyDown(key);
    }
}