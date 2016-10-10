#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: RenderForm.cs
// Date - created:2016.09.18 - 11:20
// Date - current: 2016.10.10 - 19:36

#endregion

#region Usings

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.RecordingType;
using _3DMusicVis2.RenderFrame;
using _3DMusicVis2.Setting;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class RenderForm : Screen
    {
        private readonly Camera _cam;
        private readonly PauseMenu _menu;
        private readonly Setting.Setting _mySetting;
        private float _gradiant;
        private float _gradiantMultiplier = 1;

        public RenderForm(GraphicsDeviceManager gdm, Setting.Setting currentSetting) : base(gdm, "RenderForm")
        {
            _mySetting = currentSetting;
            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
            _menu = new PauseMenu(GDM);
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
            sB.Begin();

            Texture2D dashedFrequ = null;
            if (_mySetting.Bundles.Any(x => x.Type == TypeOfRenderer.Frequency && x.Dashed))
            {
                _2DFrequencyRenderer.Dashed = true;
                dashedFrequ = _2DFrequencyRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            Texture2D frequ = null;
            if (_mySetting.Bundles.Any(x => x.Type == TypeOfRenderer.Frequency && !x.Dashed))
            {
                _2DFrequencyRenderer.Dashed = false;
                frequ = _2DFrequencyRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            Texture2D dashedSamp = null;
            if (_mySetting.Bundles.Any(x => x.Type == TypeOfRenderer.Sample && x.Dashed))
            {
                _2DSampleRenderer.Dashed = true;
                dashedSamp = _2DSampleRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            Texture2D samp = null;
            if (_mySetting.Bundles.Any(x => x.Type == TypeOfRenderer.Sample && !x.Dashed))
            {
                _2DSampleRenderer.Dashed = false;
                samp = _2DSampleRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            for (var i = 0; i < _mySetting.Bundles.Count; i++)
            {
                var toDraw = _mySetting.Bundles[i].Color.Color;

                switch (_mySetting.Bundles[i].Color.Mode)
                {
                    case Setting.ColorMode.Static:
                        break;

                    case Setting.ColorMode.Rainbow:
                        toDraw = MyMath.Rainbow(_gradiant);
                        break;
                }

                switch (_mySetting.Bundles[i].Type)
                {
                    case TypeOfRenderer.Frequency:
                        var pos = _mySetting.Bundles[i].Trans.Position;
                        var scale = _mySetting.Bundles[i].Trans.Scale;
                        sB.Draw(_mySetting.Bundles[i].Dashed ? dashedFrequ : frequ,
                            new Rectangle((int) (pos.X + Game1.VIRTUAL_RESOLUTION.Width/2f),
                                (int) (pos.Y + Game1.VIRTUAL_RESOLUTION.Height/2f),
                                (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                                (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                            _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                            SpriteEffects.None, 0);
                        break;

                    case TypeOfRenderer.Sample:
                        var pos2 = _mySetting.Bundles[i].Trans.Position;
                        var scale2 = _mySetting.Bundles[i].Trans.Scale;
                        sB.Draw(_mySetting.Bundles[i].Dashed ? dashedSamp : samp,
                            new Rectangle((int) (pos2.X + Game1.VIRTUAL_RESOLUTION.Width/2f),
                                (int) (pos2.Y + Game1.VIRTUAL_RESOLUTION.Height/2f),
                                (int) (Game1.VIRTUAL_RESOLUTION.Width*scale2.X),
                                (int) (Game1.VIRTUAL_RESOLUTION.Height*scale2.Y)), null, toDraw,
                            _mySetting.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                            SpriteEffects.None, 0);
                        break;
                    default:
                        Console.WriteLine("Something happened here...");
                        break;
                }
            }

            GDM.GraphicsDevice.Clear(Color.Black);
            sB.End();

            if (_menu.IsVisible)
            {
                _menu.Draw(sB, gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!RealTimeRecording.IsRecording) return;

            if (_mySetting.Bundles.Any(x => x.Type == TypeOfRenderer.Frequency))
            {
                _2DFrequencyRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.FrequencySpectrum));
            }

            if (_mySetting.Bundles.Any(x => x.Type == TypeOfRenderer.Sample))
            {
                _2DSampleRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.CurrentSamples));
            }

            if (Keys.Escape.KeyWasClicked())
            {
                _menu.IsVisible = !_menu.IsVisible;
                Game1.FreeBeer.IsMouseVisible = _menu.IsVisible;
            }

            _gradiant += (float) gameTime.ElapsedGameTime.TotalMilliseconds*.00001f*_gradiantMultiplier;
            if (_gradiant > .8)
            {
                _gradiant = .8f;
                _gradiantMultiplier = -1;
            }
            else if (_gradiant < .5f)
            {
                _gradiant = .5f;
                _gradiantMultiplier = 1;
            }

            if (!_menu.IsVisible) return;

            _menu.Update(gameTime);
        }
    }
}