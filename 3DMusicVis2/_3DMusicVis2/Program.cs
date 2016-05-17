#region License

// Copyright (c) 2015, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Program.cs
// Date - created: 2015.08.26 - 14:45
// Date - current: 2016.05.17 - 16:53

#endregion

namespace _3DMusicVis2
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}