#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Program.cs
// Date - created:2016.12.10 - 09:36
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis
{
#if WINDOWS || LINUX
    /// <summary>
    ///     The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}