#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: RealTimeRecording.cs
// Date - created:2016.09.11 - 12:04
// Date - current: 2016.09.11 - 17:35

#endregion

#region Usings

using System;
using NAudio.Dsp;
using NAudio.Wave;

#endregion

namespace _3DMusicVis2.RecordingType
{
    internal static class RealTimeRecording
    {
        private static IWaveIn _capture;
        private static int BitsPerSample;
        private static int Channels;
        private static int BytesPerFrame;
        private static BufferedWaveProvider waveBuffer;
        private static readonly int fftLength = 512; // NAudio fft wants powers of two!

        public static bool IsRecording;

        public static float[] CurrentSamples = new float[0];
        public static float[] FrequencySpectrum;

        private static SampleAggregator aggregator;

        public static void Initialize()
        {
            if (_capture != null) return;

            _capture = new WasapiLoopbackCapture();
            Channels = _capture.WaveFormat.Channels;

            BitsPerSample = _capture.WaveFormat.BitsPerSample;

            BytesPerFrame = BitsPerSample/8*(Channels == 0 ? 1 : Channels);

            waveBuffer = new BufferedWaveProvider(_capture.WaveFormat);

            FrequencySpectrum = new float[waveBuffer.BufferLength];

            aggregator = new SampleAggregator(fftLength) {PerformFFT = true};
            aggregator.FftCalculated += Aggregator_FftCalculated;
        }

        private static void Aggregator_FftCalculated(object sender, FftEventArgs e)
        {
            FrequencySpectrum = new float[e.Result.Length];
            for (var i = 0; i < e.Result.Length; i++)
            {
                FrequencySpectrum[i] = (float) Math.Sqrt((Math.Abs(e.Result[i].Y) + Math.Abs(e.Result[i].X))*10);
            }
        }

        public static void StartRecording()
        {
            if (IsRecording) return;

            _capture.DataAvailable += waveSource_DataAvailable;
            _capture.StartRecording();
            IsRecording = true;
        }

        public static void StopRecording()
        {
            if (!IsRecording) return;

            _capture.DataAvailable -= waveSource_DataAvailable;
            _capture.StopRecording();
            IsRecording = false;
        }

        private static void waveSource_DataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            waveBuffer.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);
            var bufferedFrames = waveBuffer.BufferedBytes/BytesPerFrame;
            var samples = waveBuffer.ToSampleProvider();

            if (bufferedFrames < 1) return;

            var frames = new float[bufferedFrames];
            samples.Read(frames, 0, bufferedFrames);

            var stepper = frames.Length;
            var toDisp = new float[stepper];
            for (var i = 0; i < stepper; i++)
            {
                toDisp[i] = frames[i];
            }

            CurrentSamples = toDisp;

            var buffer = waveInEventArgs.Buffer;
            var bytesRecorded = waveInEventArgs.BytesRecorded;
            var bufferIncrement = _capture.WaveFormat.BlockAlign;

            for (var index = 0; index < bytesRecorded; index += bufferIncrement)
            {
                var sample32 = BitConverter.ToSingle(buffer, index);
                aggregator.Add(sample32);
            }
        }


        private static Complex[] ToComplex(float[] values)
        {
            var toRet = new Complex[values.Length];

            for (var i = 0; i < values.Length; i++)
            {
                toRet[i] = new Complex {X = 0, Y = values[i]};
            }

            return toRet;
        }

        public static void UnloadMe()
        {
            if (_capture != null)
            {
                _capture.StopRecording();
                _capture.DataAvailable -= waveSource_DataAvailable;
                _capture.Dispose();
                _capture = null;
            }

            waveBuffer?.ClearBuffer();
            waveBuffer = null;
        }
    }
}