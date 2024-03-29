﻿#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: SampleAggregator.cs
// Date - created:2016.12.10 - 09:37
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;
using System.Diagnostics;
using NAudio.Dsp;

// The Complex and FFT are here!

#endregion

namespace _3DMusicVis
{
    internal class SampleAggregator
    {
        private readonly FftEventArgs fftArgs;

        // This Complex is NAudio's own! 
        private readonly Complex[] fftBuffer;
        private readonly int fftLength;
        private readonly int m;
        private int fftPos;

        public SampleAggregator(int fftLength)
        {
            if (!IsPowerOfTwo(fftLength))
                throw new ArgumentException("FFT Length must be a power of two");
            m = (int) Math.Log(fftLength, 2.0);
            this.fftLength = fftLength;
            fftBuffer = new Complex[fftLength];
            fftArgs = new FftEventArgs(fftBuffer);
        }

        public bool PerformFFT { get; set; }
        // FFT
        public event EventHandler<FftEventArgs> FftCalculated;

        private bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public void Add(float value)
        {
            if (PerformFFT && FftCalculated != null)
            {
                // Remember the window function! There are many others as well.
                fftBuffer[fftPos].X = (float) (value * FastFourierTransform.HammingWindow(fftPos, fftLength));
                fftBuffer[fftPos].Y = 0; // This is always zero with audio.
                fftPos++;
                if (fftPos >= fftLength)
                {
                    fftPos = 0;
                    FastFourierTransform.FFT(true, m, fftBuffer);
                    FftCalculated(this, fftArgs);
                }
            }
        }
    }

    public class FftEventArgs : EventArgs
    {
        [DebuggerStepThrough]
        public FftEventArgs(Complex[] result)
        {
            Result = result;
        }

        public Complex[] Result { get; private set; }
    }
}