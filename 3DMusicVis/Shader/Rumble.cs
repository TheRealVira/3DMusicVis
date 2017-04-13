#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Rumble.cs
// Date - created:2017.04.13 - 13:56
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;
using _3DMusicVis.RecordingType;

#endregion

namespace _3DMusicVis.Shader
{
    internal static class Rumble
    {
        private static readonly float[] TempValues = new float[6];

        public static void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            RenderTarget2D rumbleRendertarget, float rotationNotive)
        {
            graphics.SetRenderTarget(rumbleRendertarget);
            sB.GraphicsDevice.Clear(Color.Transparent);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null,
                ShadersManager.ShaderDictionary["Rumble"]);

            if (RealTimeRecording.MaxFreq > rotationNotive)
            {
                TempValues[0] = (Game1.Rand.Next(0, 10) - 5) *
                                (gameTime.ElapsedGameTime.Milliseconds /
                                 (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                TempValues[1] = (Game1.Rand.Next(0, 10) - 5) *
                                (gameTime.ElapsedGameTime.Milliseconds /
                                 (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                TempValues[2] = (Game1.Rand.Next(0, 10) - 5) *
                                (gameTime.ElapsedGameTime.Milliseconds /
                                 (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                TempValues[3] = (Game1.Rand.Next(0, 10) - 5) *
                                (gameTime.ElapsedGameTime.Milliseconds /
                                 (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                TempValues[4] = (Game1.Rand.Next(0, 10) - 5) *
                                (gameTime.ElapsedGameTime.Milliseconds /
                                 (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                TempValues[5] = (Game1.Rand.Next(0, 10) - 5) *
                                (gameTime.ElapsedGameTime.Milliseconds /
                                 (float) gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            else
            {
                for (var i = 0; i < TempValues.Length; i++)
                    TempValues[i] = TempValues[i].Lerp(0, .01f);
            }

            ShadersManager.ShaderDictionary["Rumble"].Parameters["RumbleVectorR"].SetValue(
                new Vector2(TempValues[0] / ResolutionManager.VIRTUAL_RESOLUTION.Width,
                    TempValues[1] / ResolutionManager.VIRTUAL_RESOLUTION.Height));
            ShadersManager.ShaderDictionary["Rumble"].Parameters["RumbleVectorG"].SetValue(
                new Vector2(TempValues[2] / ResolutionManager.VIRTUAL_RESOLUTION.Width,
                    TempValues[3] / ResolutionManager.VIRTUAL_RESOLUTION.Height));
            ShadersManager.ShaderDictionary["Rumble"].Parameters["RumbleVectorB"].SetValue(
                new Vector2(TempValues[4] / ResolutionManager.VIRTUAL_RESOLUTION.Width,
                    TempValues[5] / ResolutionManager.VIRTUAL_RESOLUTION.Height));
            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();


            toUse = rumbleRendertarget;
        }
    }
}