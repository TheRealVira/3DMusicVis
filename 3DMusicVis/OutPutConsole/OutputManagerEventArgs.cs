#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: OutputManagerEventArgs.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis.OutputConsole
{
    public class OutputManagerEventArgs : EventArgs
    {
        public OutputManagerEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}