#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: OutputManagerEventHandeling.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;
using System.IO;
using System.Text;
using _3DMusicVis.OutputConsole;

#endregion

namespace _3DMusicVis.OutPutConsole
{
    internal class OutputManagerEventHandeling : TextWriter
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