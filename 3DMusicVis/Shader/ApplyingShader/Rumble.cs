#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Rumble.cs
// Date - created:2017.04.14 - 10:42
// Date - current: 2017.04.14 - 12:00

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;
using _3DMusicVis.RecordingType;

#endregion

namespace _3DMusicVis.Shader
{
    internal class Rumble : ApplyShader
    {
        private readonly float[] _tempValues = new float[6];
        private bool _rumbled;

        public Rumble(Effect eff) : base(eff)
        {
        }

        public override void Apply(GraphicsDevice graphics, ref RenderTarget2D toUse, SpriteBatch sB, GameTime gameTime,
            params object[] paramArray)
        {
            graphics.SetRenderTarget(MyRenderTarget);
            sB.GraphicsDevice.Clear(Color.Transparent);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, MyEffect);

            if (RealTimeRecording.MaxFreq > ((Setting.Visualizer.Setting) paramArray[1]).RotationNotice)
            {
                if (!_rumbled)
                {
                    _rumbled = true;

                    _tempValues[0] = (Game1.Rand.Next(0, 10) - 5) *
                                     (gameTime.ElapsedGameTime.Milliseconds /
                                      (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                    _tempValues[1] = (Game1.Rand.Next(0, 10) - 5) *
                                     (gameTime.ElapsedGameTime.Milliseconds /
                                      (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                    _tempValues[2] = (Game1.Rand.Next(0, 10) - 5) *
                                     (gameTime.ElapsedGameTime.Milliseconds /
                                      (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                    _tempValues[3] = (Game1.Rand.Next(0, 10) - 5) *
                                     (gameTime.ElapsedGameTime.Milliseconds /
                                      (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                    _tempValues[4] = (Game1.Rand.Next(0, 10) - 5) *
                                     (gameTime.ElapsedGameTime.Milliseconds /
                                      (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                    _tempValues[5] = (Game1.Rand.Next(0, 10) - 5) *
                                     (gameTime.ElapsedGameTime.Milliseconds /
                                      (float) gameTime.ElapsedGameTime.TotalMilliseconds);
                }
            }
            else
            {
                _rumbled = false;

                for (var i = 0; i < _tempValues.Length; i++)
                    _tempValues[i] = _tempValues[i].Lerp(0, .01f);
            }

            MyEffect.Parameters["RumbleVectorR"].SetValue(
                new Vector2(_tempValues[0] / ResolutionManager.VIRTUAL_RESOLUTION.Width,
                    _tempValues[1] / ResolutionManager.VIRTUAL_RESOLUTION.Height));
            MyEffect.Parameters["RumbleVectorG"].SetValue(
                new Vector2(_tempValues[2] / ResolutionManager.VIRTUAL_RESOLUTION.Width,
                    _tempValues[3] / ResolutionManager.VIRTUAL_RESOLUTION.Height));
            MyEffect.Parameters["RumbleVectorB"].SetValue(
                new Vector2(_tempValues[4] / ResolutionManager.VIRTUAL_RESOLUTION.Width,
                    _tempValues[5] / ResolutionManager.VIRTUAL_RESOLUTION.Height));
            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);

            sB.End();

            toUse = MyRenderTarget;
        }
    }
}