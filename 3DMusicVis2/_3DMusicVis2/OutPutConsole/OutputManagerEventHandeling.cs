#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: OutputManagerEventHandeling.cs
// Date - created: 2016.05.23 - 16:08
// Date - current: 2016.05.23 - 21:16

#endregion

#region Usings

using System;
using System.IO;
using System.Text;
using _3DMusicVis2.OutputConsole;

#endregion

namespace _3DMusicVis2.OutPutConsole
{
    class OutputManagerEventHandeling : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string value)
        {
            Writing?.Invoke(value, new OutputManagerEventArgs(value));
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            Writlineing?.Invoke(value, new OutputManagerEventArgs(value));
            base.Write(value);
        }

        public event EventHandler<OutputManagerEventArgs> Writing;
        public event EventHandler<OutputManagerEventArgs> Writlineing;
    }
}