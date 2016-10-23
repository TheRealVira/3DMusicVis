#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: RenderForm.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.23 - 18:25

#endregion

#region Usings

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.RecordingType;
using _3DMusicVis2.RenderFrame;
using _3DMusicVis2.Setting.Visualizer;
using _3DMusicVis2.Shader;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class RenderForm : Screen
    {
        private readonly RenderTarget2D _alphaDeletionRendertarget;
        private readonly Camera _cam;
        private readonly PauseMenu _menu;
        private readonly Setting.Visualizer.Setting _mySetting;
        private readonly RenderTarget2D _scanLineRendertarget;

        private readonly RenderTarget2D _wavesRendertarget;

        private float _breathingGradiant;
        private float _breathinggradiantMultiplier = 1;
        private RenderTarget2D _gausianBlurRendertarget;

        private float _rainbowGradiant;
        private float _rainbowgradiantMultiplier = 1;

        public RenderForm(GraphicsDeviceManager gdm, Setting.Visualizer.Setting currentSetting)
            : base(gdm, "RenderForm")
        {
            _mySetting = currentSetting;
            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
            _menu = new PauseMenu(GDM);
            _wavesRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);
            _gausianBlurRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);
            _scanLineRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);
            _alphaDeletionRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);
        }

        public override void LoadedUp()
        {
            base.LoadedUp();
            RealTimeRecording.StartRecording();
            Game1.FreeBeer.IsMouseVisible = false;
        }

        public override void Unloade()
        {
            base.Unloade();
            RealTimeRecording.StopRecording();
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            Texture2D dashedFrequ = null;
            if (_mySetting.Bundles.Any(x => x.IsFrequency && x.HowIDraw == DrawMode.Dashed))
            {
                dashedFrequ = _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Dashed);
            }

            Texture2D frequ = null;
            if (_mySetting.Bundles.Any(x => x.IsFrequency && x.HowIDraw == DrawMode.Blocked))
            {
                frequ = _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Blocked);
            }

            Texture2D dashedSamp = null;
            if (_mySetting.Bundles.Any(x => !x.IsFrequency && x.HowIDraw == DrawMode.Dashed))
            {
                dashedSamp = _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Dashed);
            }

            Texture2D samp = null;
            if (_mySetting.Bundles.Any(x => !x.IsFrequency && x.HowIDraw == DrawMode.Blocked))
            {
                samp = _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Blocked);
            }

            Texture2D connectedFrequ = null;
            if (_mySetting.Bundles.Any(x => x.IsFrequency && x.HowIDraw == DrawMode.Connected))
            {
                connectedFrequ = _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Connected);
            }

            Texture2D connectedSamp = null;
            if (_mySetting.Bundles.Any(x => !x.IsFrequency && x.HowIDraw == DrawMode.Connected))
            {
                connectedSamp = _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Connected);
            }

            GDM.GraphicsDevice.SetRenderTarget(_wavesRendertarget);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            sB.GraphicsDevice.Clear(Color.Transparent);

            for (var i = 0; i < _mySetting.Bundles.Count; i++)
            {
                var toDraw = _mySetting.Bundles[i].Color.Color;

                switch (_mySetting.Bundles[i].Color.Mode)
                {
                    case Setting.Visualizer.ColorMode.Static: // The color was set before (this is for some null errors)
                        break;

                    case Setting.Visualizer.ColorMode.Rainbow:
                        toDraw = MyMath.Rainbow(_rainbowGradiant);
                        break;

                    case Setting.Visualizer.ColorMode.Breath:
                        toDraw = Color.Lerp(_mySetting.BackgroundColor, toDraw, _breathingGradiant);
                        break;
                }

                var pos = _mySetting.Bundles[i].Trans.Position;
                var scale = _mySetting.Bundles[i].Trans.Scale;

                if (_mySetting.Bundles[i].IsFrequency)
                {
                    switch (_mySetting.Bundles[i].HowIDraw)
                    {
                        case DrawMode.Blocked:
                            sB.Draw(frequ,
                                new Rectangle(
                                    (int) (pos.X*Game1.VIRTUAL_RESOLUTION.Width + Game1.VIRTUAL_RESOLUTION.Center.X),
                                    (int) (pos.Y*Game1.VIRTUAL_RESOLUTION.Height + Game1.VIRTUAL_RESOLUTION.Center.Y),
                                    (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                                    (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                                _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                                SpriteEffects.None, 0);
                            continue;
                        case DrawMode.Dashed:
                            sB.Draw(dashedFrequ,
                                new Rectangle(
                                    (int) (pos.X*Game1.VIRTUAL_RESOLUTION.Width + Game1.VIRTUAL_RESOLUTION.Width/2f),
                                    (int) (pos.Y*Game1.VIRTUAL_RESOLUTION.Height + Game1.VIRTUAL_RESOLUTION.Center.Y),
                                    (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                                    (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                                _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                                SpriteEffects.None, 0);
                            continue;
                        case DrawMode.Connected:
                            sB.Draw(connectedFrequ,
                                new Rectangle(
                                    (int) (pos.X*Game1.VIRTUAL_RESOLUTION.Width + Game1.VIRTUAL_RESOLUTION.Center.X),
                                    (int) (pos.Y*Game1.VIRTUAL_RESOLUTION.Height + Game1.VIRTUAL_RESOLUTION.Center.Y),
                                    (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                                    (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                                _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                                SpriteEffects.None, 0);
                            continue;
                        default:
                            continue;
                    }
                }

                switch (_mySetting.Bundles[i].HowIDraw)
                {
                    case DrawMode.Blocked:
                        sB.Draw(samp,
                            new Rectangle(
                                (int) (pos.X*Game1.VIRTUAL_RESOLUTION.Width + Game1.VIRTUAL_RESOLUTION.Center.X),
                                (int) (pos.Y*Game1.VIRTUAL_RESOLUTION.Height + Game1.VIRTUAL_RESOLUTION.Height/2f) +
                                Game1.VIRTUAL_RESOLUTION.Center.Y,
                                (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                                (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                            _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                            SpriteEffects.None, 0);
                        continue;
                    case DrawMode.Dashed:
                        sB.Draw(dashedSamp,
                            new Rectangle(
                                (int) (pos.X*Game1.VIRTUAL_RESOLUTION.Width + Game1.VIRTUAL_RESOLUTION.Center.X),
                                (int) (pos.Y*Game1.VIRTUAL_RESOLUTION.Height + Game1.VIRTUAL_RESOLUTION.Height/2f) +
                                Game1.VIRTUAL_RESOLUTION.Center.Y,
                                (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                                (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                            _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                            SpriteEffects.None, 0);
                        continue;
                    case DrawMode.Connected:
                        sB.Draw(connectedSamp,
                            new Rectangle(
                                (int) (pos.X*Game1.VIRTUAL_RESOLUTION.Width + Game1.VIRTUAL_RESOLUTION.Center.X),
                                (int) (pos.Y*Game1.VIRTUAL_RESOLUTION.Height + Game1.VIRTUAL_RESOLUTION.Height/2f) +
                                Game1.VIRTUAL_RESOLUTION.Center.Y,
                                (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                                (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                            _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                            SpriteEffects.None, 0);
                        continue;
                    default:
                        continue;
                }
            }

            sB.End();

            var toUse = _wavesRendertarget;

            if ((_mySetting.Shaders & ShaderMode.Blur) != 0)
            {
                // Blur the bloom
                _gausianBlurRendertarget =
                    (RenderTarget2D) GaussianBlurManager.Compute(toUse, sB);

                toUse = _gausianBlurRendertarget;
            }

            if ((_mySetting.Shaders & ShaderMode.Bloom) != 0)
            {
                // Apply bloom
                BloomManager.Bloom.BeginDraw();
                sB.GraphicsDevice.Clear(Color.Transparent);
                // Applying shader
                sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                sB.Draw(toUse, Game1.VIRTUAL_RESOLUTION, Color.White);
                sB.End();
                BloomManager.Bloom.EndDraw();

                toUse = BloomManager.Bloom.FinalRenderTarget;
            }

            if ((_mySetting.Shaders & ShaderMode.Liquify) != 0)
            {
                GDM.GraphicsDevice.SetRenderTarget(_alphaDeletionRendertarget);
                sB.GraphicsDevice.Clear(Color.Transparent);
                sB.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, null, null, null, Game1.LiquifyEffect);
                Game1.LiquifyEffect.Parameters["width"].SetValue( /*.5f*/0.2f);
                Game1.LiquifyEffect.Parameters["toBe"].SetValue(_mySetting.BackgroundColor.Negate().ToVector4());
                sB.Draw(toUse, Game1.VIRTUAL_RESOLUTION, Color.White);
                sB.End();

                toUse = _alphaDeletionRendertarget;
            }

            if ((_mySetting.Shaders & ShaderMode.ScanLine) != 0)
            {
                GDM.GraphicsDevice.SetRenderTarget(_scanLineRendertarget);
                sB.GraphicsDevice.Clear(Color.Transparent);
                sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, Game1.ScanlinEffect);
                Game1.ScanlinEffect.Parameters["ImageHeight"].SetValue(Game1.VIRTUAL_RESOLUTION.Height);
                Game1.ScanlinEffect.Parameters["LineColor"].SetValue(_mySetting.BackgroundColor.ToVector4());
                sB.Draw(toUse, Game1.VIRTUAL_RESOLUTION, Color.White);
                sB.End();

                toUse = _scanLineRendertarget;
            }

            GDM.GraphicsDevice.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
            sB.GraphicsDevice.Clear(_mySetting.BackgroundColor);
            sB.Begin();
            sB.Draw(toUse, Game1.VIRTUAL_RESOLUTION, Color.White);
            sB.End();

            if (_menu.IsVisible)
            {
                _menu.Draw(sB, gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!RealTimeRecording.IsRecording) return;

            if (_mySetting.Bundles.Any(x => x.IsFrequency))
            {
                _2DFrequencyRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.FrequencySpectrum));
            }

            if (_mySetting.Bundles.Any(x => !x.IsFrequency))
            {
                _2DSampleRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.CurrentSamples));
            }

            if (Keys.Escape.KeyWasClicked())
            {
                _menu.IsVisible = !_menu.IsVisible;
                Game1.FreeBeer.IsMouseVisible = _menu.IsVisible;
            }

            _rainbowGradiant += (float) gameTime.ElapsedGameTime.TotalMilliseconds*.00001f*_rainbowgradiantMultiplier;
            _breathingGradiant += (float) gameTime.ElapsedGameTime.TotalMilliseconds*.0002f*_breathinggradiantMultiplier;

            if (_rainbowGradiant > .8)
            {
                _rainbowGradiant = .8f;
                _rainbowgradiantMultiplier = -1;
            }
            else if (_rainbowGradiant < .5f)
            {
                _rainbowGradiant = .5f;
                _rainbowgradiantMultiplier = 1;
            }

            if (_breathingGradiant > 1)
            {
                _breathingGradiant = 1;
                _breathinggradiantMultiplier = -1;
            }
            else if (_breathingGradiant < 0)
            {
                _breathingGradiant = 0;
                _breathinggradiantMultiplier = 1;
            }

            if (!_menu.IsVisible) return;

            _menu.Update(gameTime);
        }
    }
}