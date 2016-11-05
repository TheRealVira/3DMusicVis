﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: RealTimeRecording.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;

#endregion

namespace _3DMusicVis2.RecordingType
{
    internal static class RealTimeRecording
    {
        public const int FftLength = 512; // NAudio fft wants powers of two!
        private static IWaveIn _capture;
        private static int BitsPerSample;
        private static int Channels;
        private static int BytesPerFrame;
        private static BufferedWaveProvider waveBuffer;

        public static bool IsRecording;

        public static float[] CurrentSamples = new float[0];
        public static float[] FrequencySpectrum;

        public static float[] TestSampleData;
        public static float[] TestFrequencyleData;

        private static SampleAggregator aggregator;

        private const float MULTIPLACTOR = 10;

        public static void Initialize()
        {
            if (_capture != null) return;

            _capture = new WasapiLoopbackCapture();
            Channels = _capture.WaveFormat.Channels;

            BitsPerSample = _capture.WaveFormat.BitsPerSample;

            BytesPerFrame = BitsPerSample/8*(Channels == 0 ? 1 : Channels);

            waveBuffer = new BufferedWaveProvider(_capture.WaveFormat);

            FrequencySpectrum = new float[waveBuffer.BufferLength/999];
            TestFrequencyleData = new float[FrequencySpectrum.Length];
            TestSampleData = new float[FrequencySpectrum.Length];

            for (var i = 0; i < TestFrequencyleData.Length; i++)
            {
                TestFrequencyleData[i] = (float) Game1.Rand.NextDouble()/2f;
                TestSampleData[i] = (float) Game1.Rand.NextDouble()/2f - .25f;
            }

            aggregator = new SampleAggregator(FftLength) {PerformFFT = true};
            aggregator.FftCalculated += Aggregator_FftCalculated;
        }

        private static void Aggregator_FftCalculated(object sender, FftEventArgs e)
        {
            FrequencySpectrum = new float[e.Result.Length];

            for (var i = 0; i < e.Result.Length; i++)
            {
                // Add both numbers together and multiply them by ten (this is because the numbers would be too small).
                // Then apply the square root, so the differences will get visible.

                // Example:
                // sqrt(1/16) = 1/4
                // sqrt(1.5)  = 1.225
                var freq = (Math.Abs(e.Result[i].Y) + Math.Abs(e.Result[i].X))* MULTIPLACTOR;
                FrequencySpectrum[i] = (float) Math.Min(Math.Sqrt(freq), 1); // Apply maximum level of one.
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
            if (bufferedFrames < 1) return; // Nothing was buffered -> nothing is recorded -> quick return

            var samples = waveBuffer.ToSampleProvider();


            // Gather samples
            var frames = new float[bufferedFrames];
            samples.Read(frames, 0, bufferedFrames);

            //var stepper = frames.Length;
            //var toDisp = new float[stepper];
            //for (var i = 0; i < stepper; i++)
            //{
            //    toDisp[i] = frames[i];
            //}

            // Saving data into a static field
            CurrentSamples = frames;

            // Get parameters
            var buffer = waveInEventArgs.Buffer;
            var bytesRecorded = waveInEventArgs.BytesRecorded;
            var bufferIncrement = _capture.WaveFormat.BlockAlign;

            // Aggregate every bufferblock (till the total bytes recorded boundarie has been reached).
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