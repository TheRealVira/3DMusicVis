#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: OutputManagerEventArgs.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.23 - 18:24

#endregion

#region Usings

using System;

#endregion

namespace _3DMusicVis2.OutputConsole
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