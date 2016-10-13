#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Program.cs
// Date - created:2016.07.02 - 17:04
// Date - current: 2016.10.13 - 20:11

#endregion

namespace _3DMusicVis2
{
#if WINDOWS || XBOX
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}