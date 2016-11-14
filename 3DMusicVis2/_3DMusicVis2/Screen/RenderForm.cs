#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: RenderForm.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.11.14 - 18:39

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.Manager;
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

        private readonly Dictionary<string, RenderTarget2D> _bufferedDrawer;
        private readonly Camera _cam;

        private readonly List<string> _dicKeys;
        private readonly PauseMenu _menu;
        private readonly Setting.Visualizer.Setting _mySetting;
        private readonly RenderTarget2D _scanLineRendertarget;

        private readonly RenderTarget2D _wavesRendertarget;

        private float _breathingGradiant;
        private float _breathinggradiantMultiplier = 1;
        private RenderTarget2D _gausianBlurRendertarget;

        private float _rainbowGradiant;
        private float _rainbowgradiantMultiplier = 1;
        public Color NotSelectedColor = Color.White;

        public SettingsBundle Selected;
        public Color SelectedColor = Color.Red;
        public bool UseColor = true;

        public bool UseShader = true;

        public RenderForm(GraphicsDeviceManager gdm, Setting.Visualizer.Setting currentSetting)
            : base(gdm, "RenderForm")
        {
            _mySetting = currentSetting;
            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 15f);
            _menu = new PauseMenu(GDM);
            _wavesRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);
            _gausianBlurRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);
            _scanLineRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);
            _alphaDeletionRendertarget = new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height);

            _dicKeys = new List<string>();
            _bufferedDrawer = new Dictionary<string, RenderTarget2D>();
            currentSetting.Bundles.ForEach(x =>
            {
                if (_bufferedDrawer.ContainsKey(x.ToString())) return;

                _bufferedDrawer.Add(x.ToString(),
                    new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width,
                        Game1.VIRTUAL_RESOLUTION.Height));
                _dicKeys.Add(x.ToString());
            });
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
            DisposeBuffered();
            _bufferedDrawer.Clear();
        }

        private void DisposeBuffered()
        {
            foreach (var renderTarget2D in _bufferedDrawer)
            {
                renderTarget2D.Value.Dispose();
            }
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            var curMode = DrawMode.Blocked;
            RenderTarget2D curTex = null;

            foreach (var key in _dicKeys)
            {
                switch (key)
                {
                        #region DRAWING

                    case "2DDrawFrequency":
                        _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, curMode, ref curTex);
                        break;
                    case "2DDrawSample":
                        _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, curMode, ref curTex);
                        break;

                    case "3DDrawFrequency":
                        _3DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, curMode, ref curTex);
                        break;
                    case "3DDrawSample":
                        _3DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, curMode, ref curTex);
                        break;

                        #endregion

                        #region 2D Frequency render settings

                    case "2FDashed":
                        curTex = _bufferedDrawer["2FDashed"];
                        curMode = DrawMode.Dashed;
                        goto case "2DDrawFrequency";
                    case "2FBlocked":
                        curTex = _bufferedDrawer["2FBlocked"];
                        curMode = DrawMode.Blocked;
                        goto case "2DDrawFrequency";
                    case "2FConnected":
                        curTex = _bufferedDrawer["2FConnected"];
                        curMode = DrawMode.Connected;
                        goto case "2DDrawFrequency";

                        #endregion

                        #region 2D Sample render settings

                    case "2SDashed":
                        curTex = _bufferedDrawer["2SDashed"];
                        curMode = DrawMode.Dashed;
                        goto case "2DDrawSample";
                    case "2SBlocked":
                        curTex = _bufferedDrawer["2SBlocked"];
                        curMode = DrawMode.Blocked;
                        goto case "2DDrawSample";
                    case "2SConnected":
                        curTex = _bufferedDrawer["2SConnected"];
                        curMode = DrawMode.Connected;
                        goto case "2DDrawSample";

                        #endregion

                        #region 3D Frequency render settings

                    case "3FDashed":
                        curTex = _bufferedDrawer["3FDashed"];
                        curMode = DrawMode.Dashed;
                        goto case "3DDrawFrequency";
                    case "3FBlocked":
                        curTex = _bufferedDrawer["3FBlocked"];
                        curMode = DrawMode.Blocked;
                        goto case "3DDrawFrequency";
                    case "3FConnected":
                        curTex = _bufferedDrawer["3FConnected"];
                        curMode = DrawMode.Connected;
                        goto case "3DDrawFrequency";

                        #endregion

                    default:
                        continue;
                }
            }

            GDM.GraphicsDevice.SetRenderTarget(_wavesRendertarget);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null);
            sB.GraphicsDevice.Clear(Color.Transparent);

            for (var i = 0; i < _mySetting.Bundles.Count; i++)
            {
                var toDraw = _mySetting.Bundles[i].Color.Color;

                if (UseColor)
                {
                    switch (_mySetting.Bundles[i].Color.Mode)
                    {
                        case Setting.Visualizer.ColorMode.Static:
                            // The color was set before (this is for some null errors)
                            break;

                        case Setting.Visualizer.ColorMode.Rainbow:
                            toDraw = MyMath.Rainbow(_rainbowGradiant);
                            break;

                        case Setting.Visualizer.ColorMode.Breath:
                            toDraw = Color.Lerp(_mySetting.BackgroundColor, toDraw, _breathingGradiant);
                            break;
                        default:
                            break;
                    }

                    if (_mySetting.Bundles[i].Color.Negate)
                    {
                        toDraw = toDraw.Negate();
                    }
                }
                else
                {
                    toDraw = _mySetting.Bundles[i] == Selected ? SelectedColor : NotSelectedColor;
                }

                var pos = _mySetting.Bundles[i].Trans.Position;
                var scale = _mySetting.Bundles[i].Trans.Scale;
                var effect = (_mySetting.Bundles[i].HorizontalMirror
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None) |
                             (_mySetting.Bundles[i].VerticalMirror
                                 ? SpriteEffects.FlipVertically
                                 : SpriteEffects.None);


                var origin = Game1.VIRTUAL_RESOLUTION.Center.ToVector2();

                var myRec = new Rectangle(
                    (int) (pos.X*Game1.VIRTUAL_RESOLUTION.Width + Game1.VIRTUAL_RESOLUTION.Width*scale.X/2),
                    (int) (pos.Y*Game1.VIRTUAL_RESOLUTION.Height + Game1.VIRTUAL_RESOLUTION.Height*scale.Y/2),
                    (int) (scale.X*Game1.VIRTUAL_RESOLUTION.Width),
                    (int) (scale.Y*Game1.VIRTUAL_RESOLUTION.Height));

                sB.Draw(
                    _bufferedDrawer[_mySetting.Bundles[i].ToString()],
                    myRec,
                    null,
                    toDraw,
                    _mySetting.Bundles[i].Trans.Rotation,
                    origin,
                    effect,
                    0);
            }

            sB.End();

            var toUse = _wavesRendertarget;

            if (UseShader)
            {
                if ((_mySetting.Shaders & ShaderMode.Blur) != 0)
                {
                    // Blur the bloom
                    _gausianBlurRendertarget = (RenderTarget2D) GaussianBlurManager.Compute(toUse, sB);

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
            }

            GDM.GraphicsDevice.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
            sB.GraphicsDevice.Clear(_mySetting.BackgroundColor);
            sB.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);

            if (_mySetting.BackgroundImage != null)
            {
                DrawImageFromSetting(_mySetting.BackgroundImage, ref _mySetting.BackgroundImage.Rotation, sB, gameTime);
            }

            sB.Draw(toUse, Game1.VIRTUAL_RESOLUTION, Color.White);

            if (_mySetting.ForegroundImage != null)
            {
                DrawImageFromSetting(_mySetting.ForegroundImage, ref _mySetting.ForegroundImage.Rotation, sB, gameTime);
            }

            sB.End();

            if (_menu.IsVisible)
            {
                _menu.Draw(sB, gameTime);
            }
        }

        private void DrawImageFromSetting(ImageSetting set, ref float rot, SpriteBatch sB, GameTime gt)
        {
            Texture2D tryImage;
            if (!ImageManager.Images.TryGetValue(set.ImageFileName ?? "", out tryImage)) return;

            var mid = 1 +
                      ((set.Mode & ImageMode.Vibrate) != 0
                          ? Math.Min(MathHelper.Lerp(RealTimeRecording.PrevMaxFreq, RealTimeRecording.MaxFreq, .2f), 1f)*
                            .1f
                          : 0);

            if ((set.Mode & ImageMode.ReverseOnBeat) != 0 && RealTimeRecording.MaxFreq > set.RotationNotice)
            {
                set.ReverseRotation = !set.ReverseRotation;
            }

            if (Math.Abs(rot) == 360)
            {
                rot = 0;
            }
            else
            {
                rot += (float) gt.ElapsedGameTime.TotalMilliseconds*.0002f*(set.ReverseRotation ? -1 : 1)*
                       set.RotationSpeedMutliplier;
            }

            if (rot > 360)
            {
                rot = 360;
            }

            if ((set.Mode & ImageMode.HoverRender) != 0)
            {
                sB.Draw(tryImage,
                    new Rectangle(
                        Game1.VIRTUAL_RESOLUTION.Center.X + (int) (set.Offset.X*Game1.VIRTUAL_RESOLUTION.Width),
                        Game1.VIRTUAL_RESOLUTION.Center.Y + (int) (set.Offset.Y*Game1.VIRTUAL_RESOLUTION.Height),
                        (int) (MathHelper.Min(tryImage.Width, Game1.VIRTUAL_RESOLUTION.Width)*(1 + -1*(mid - 1))),
                        (int) (MathHelper.Min(tryImage.Height, Game1.VIRTUAL_RESOLUTION.Height)*(1 + -1*(mid - 1)))),
                    null, set.Tint*.5f, (set.Mode & ImageMode.Rotate) != 0 ? rot : 0f,
                    tryImage.Bounds.Center.ToVector2(), SpriteEffects.None, 0f);
            }

            sB.Draw(tryImage,
                new Rectangle(Game1.VIRTUAL_RESOLUTION.Center.X + (int) (set.Offset.X*Game1.VIRTUAL_RESOLUTION.Width),
                    Game1.VIRTUAL_RESOLUTION.Center.Y + (int) (set.Offset.Y*Game1.VIRTUAL_RESOLUTION.Height),
                    (int) (MathHelper.Min(tryImage.Width, Game1.VIRTUAL_RESOLUTION.Width)*mid),
                    (int) (MathHelper.Min(tryImage.Height, Game1.VIRTUAL_RESOLUTION.Height)*mid)), null, set.Tint,
                (set.Mode & ImageMode.Rotate) != 0 ? rot : 0f,
                tryImage.Bounds.Center.ToVector2(), SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (!RealTimeRecording.IsRecording) return;

            if (_dicKeys.Any(x => x.StartsWith("3")))
            {
                if (!_menu.IsVisible)
                {
                    _cam.Update(gameTime);
                    Mouse.SetPosition(Game1.VIRTUAL_RESOLUTION.Center.X, Game1.VIRTUAL_RESOLUTION.Center.Y);
                }

                if (_dicKeys.Any(x => x.StartsWith("3F")))
                {
                    _3DFrequencyRenderer.UpdateRenderer(
                        new ReadOnlyCollection<float>(RealTimeRecording.FrequencySpectrum));
                }
            }
            if (_dicKeys.Any(x => x.StartsWith("2F")))
            {
                _2DFrequencyRenderer.UpdateRenderer(
                    new ReadOnlyCollection<float>(RealTimeRecording.FrequencySpectrum));
            }

            if (_dicKeys.Any(x => x.StartsWith("2S")))
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