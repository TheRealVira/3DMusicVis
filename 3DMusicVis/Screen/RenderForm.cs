﻿#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: RenderForm.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis.Manager;
using _3DMusicVis.RecordingType;
using _3DMusicVis.RenderFrame;
using _3DMusicVis.RenderFrame._2D;
using _3DMusicVis.RenderFrame._3D.Used;
using _3DMusicVis.Setting.Visualizer;
using _3DMusicVis.Shader;

#endregion

namespace _3DMusicVis.Screen
{
    internal class RenderForm : Screen
    {
        private readonly RenderTarget2D _alphaDeletionRendertarget;
        private readonly RenderTarget2D _bloomRendertarget;

        private readonly Dictionary<string, RenderTarget2D> _bufferedDrawer;
        private readonly Camera _cam;

        private readonly List<string> _dicKeys;
        private readonly PauseMenu _menu;
        private readonly Setting.Visualizer.Setting _mySetting;
        private readonly RenderTarget2D _scanLineRendertarget;

        private readonly RenderTarget2D _wavesRendertarget;
        private readonly Dictionary<string, RendererDefaults.DrawGraphicsOnRenderTarget> DrawingTechniques;

        private float _breathingGradiant;
        private float _breathinggradiantMultiplier = 1;
        private RenderTarget2D _gausianBlurRendertarget;

        private float _rainbowGradiant;
        private float _rainbowgradiantMultiplier = 1;

        public SettingsBundle Selected;
        public bool UseColor = true;

        public bool UseShader = true;

        public RenderForm(GraphicsDeviceManager gdm, Setting.Visualizer.Setting currentSetting)
            : base(gdm, "RenderForm")
        {
            _mySetting = currentSetting;
            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 15f);
            _menu = new PauseMenu(GDM);
            _wavesRendertarget = new RenderTarget2D(GDM.GraphicsDevice, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);
            _gausianBlurRendertarget = new RenderTarget2D(GDM.GraphicsDevice, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);
            _scanLineRendertarget = new RenderTarget2D(GDM.GraphicsDevice, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);
            _alphaDeletionRendertarget = new RenderTarget2D(GDM.GraphicsDevice,
                ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);
            _bloomRendertarget = new RenderTarget2D(GDM.GraphicsDevice,
                ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);

            _dicKeys = new List<string>();
            _bufferedDrawer = new Dictionary<string, RenderTarget2D>();
            currentSetting.Bundles.ForEach(x =>
            {
                if (_bufferedDrawer.ContainsKey(x.ToString())) return;

                _bufferedDrawer.Add(x.ToString(),
                    new RenderTarget2D(GDM.GraphicsDevice, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height));
                _dicKeys.Add(x.ToString());
            });

            DrawingTechniques = new Dictionary<string, RendererDefaults.DrawGraphicsOnRenderTarget>
            {
                {
                    "2F", _2DFrequencyRenderer.DrawingTechnique
                },

                {
                    "2S", _2DSampleRenderer.DrawingTechnique
                },
                {
                    "3F", _3DFrequencyRenderer.DrawingTechnique
                },

                {
                    "3S", _3DSampleRenderer.DrawingTechnique
                }
            };
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
                renderTarget2D.Value.Dispose();
        }

        private RendererDefaults.DrawGraphicsOnRenderTarget SetSettingsFromString(string setting,
            ref RenderTarget2D curTex, ref DrawMode curMode)
        {
            curTex = _bufferedDrawer[setting];
            Enum.TryParse(setting.Substring(2, setting.Length - 2), out curMode);

            return DrawingTechniques[setting.Substring(0, 2)];
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            var curMode = DrawMode.Blocked;
            RenderTarget2D curTex = null;

            for (var i = 0; i < _dicKeys.Count; i++)
                SetSettingsFromString(_dicKeys[i], ref curTex, ref curMode)
                    .Invoke(GDM.GraphicsDevice, gameTime, _cam, curMode, ref curTex);

            GDM.GraphicsDevice.SetRenderTarget(_wavesRendertarget);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null);
            sB.GraphicsDevice.Clear(Color.Transparent);

            var backGroundColor = _mySetting.BackgroundColor.GetAppliedColor(_breathingGradiant, _rainbowGradiant);

            for (var i = 0; i < _mySetting.Bundles.Count; i++)
            {
                Color toDraw;

                if (UseColor)
                    toDraw = _mySetting.Bundles[i].Color.Negate
                        ? _mySetting.Bundles[i].Color.GetAppliedColor(_breathingGradiant, _rainbowGradiant,
                            backGroundColor).Negate()
                        : _mySetting.Bundles[i].Color.GetAppliedColor(_breathingGradiant, _rainbowGradiant,
                            backGroundColor);
                else
                    toDraw = _mySetting.Bundles[i] == Selected ? backGroundColor.Negate() : backGroundColor;

                var pos = _mySetting.Bundles[i].Trans.Position;
                var scale = _mySetting.Bundles[i].Trans.Scale;
                var effect = (_mySetting.Bundles[i].HorizontalMirror
                                 ? SpriteEffects.FlipHorizontally
                                 : SpriteEffects.None) |
                             (_mySetting.Bundles[i].VerticalMirror
                                 ? SpriteEffects.FlipVertically
                                 : SpriteEffects.None);


                var origin = ResolutionManager.VIRTUAL_RESOLUTION.Center.ToVector2();

                var myRec = new Rectangle(
                    (int)
                    (pos.X * ResolutionManager.VIRTUAL_RESOLUTION.Width +
                     ResolutionManager.VIRTUAL_RESOLUTION.Width * scale.X / 2),
                    (int)
                    (pos.Y * ResolutionManager.VIRTUAL_RESOLUTION.Height +
                     ResolutionManager.VIRTUAL_RESOLUTION.Height * scale.Y / 2),
                    (int) (scale.X * ResolutionManager.VIRTUAL_RESOLUTION.Width),
                    (int) (scale.Y * ResolutionManager.VIRTUAL_RESOLUTION.Height));

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
                foreach (ShaderMode value in Enum.GetValues(typeof(ShaderMode)))
                {
                    if (!Enum.TryParse(value.ToString(), out ShaderMode currentShader) ||
                        !ShadersManager.ShaderDictionary.ContainsKey(currentShader) ||
                        !_mySetting.Shaders.HasFlag(value))
                        continue;

                    ShadersManager.ShaderDictionary[currentShader].Apply(GDM.GraphicsDevice,
                        ref toUse, sB, gameTime, backGroundColor, _mySetting);
                }

            GDM.GraphicsDevice.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
            sB.GraphicsDevice.Clear(backGroundColor);
            sB.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);

            if (_mySetting.BackgroundImage != null)
                DrawImageFromSetting(_mySetting.BackgroundImage, ref _mySetting.BackgroundImage.Rotation, sB, gameTime);

            sB.Draw(toUse, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);

            if (_mySetting.ForegroundImage != null)
                DrawImageFromSetting(_mySetting.ForegroundImage, ref _mySetting.ForegroundImage.Rotation, sB, gameTime);

            if (_menu.IsVisible)
                _menu.Draw(sB, gameTime);

            sB.End();
        }

        private static void DrawImageFromSetting(ImageSetting set, ref float rot, SpriteBatch sB, GameTime gt)
        {
            Texture2D tryImage;
            if (!ImageManager.Images.TryGetValue(set.ImageFileName ?? "", out tryImage)) return;

            var mid = 1 +
                      ((set.Mode & ImageMode.Vibrate) != 0
                          ? Math.Min(MathHelper.Lerp(RealTimeRecording.PrevMaxFreq, RealTimeRecording.MaxFreq, .2f), 1f) *
                            .1f
                          : 0);

            if ((set.Mode & ImageMode.ReverseOnBeat) != 0 && RealTimeRecording.MaxFreq > set.RotationNotice)
                set.ReverseRotation = !set.ReverseRotation;

            if (Math.Abs(rot) == 360)
                rot = 0;
            else
                rot += (float) gt.ElapsedGameTime.TotalMilliseconds * .0002f * (set.ReverseRotation ? -1 : 1) *
                       set.RotationSpeedMutliplier;

            if (rot > 360)
                rot = 360;

            if ((set.Mode & ImageMode.HoverRender) != 0)
                sB.Draw(tryImage,
                    new Rectangle(
                        ResolutionManager.VIRTUAL_RESOLUTION.Center.X +
                        (int) (set.Offset.X * ResolutionManager.VIRTUAL_RESOLUTION.Width),
                        ResolutionManager.VIRTUAL_RESOLUTION.Center.Y +
                        (int) (set.Offset.Y * ResolutionManager.VIRTUAL_RESOLUTION.Height),
                        (int)
                        (MathHelper.Min(tryImage.Width, ResolutionManager.VIRTUAL_RESOLUTION.Width) *
                         (1 + -1 * (mid - 1))),
                        (int)
                        (MathHelper.Min(tryImage.Height, ResolutionManager.VIRTUAL_RESOLUTION.Height) *
                         (1 + -1 * (mid - 1)))),
                    null, set.Tint * .5f, (set.Mode & ImageMode.Rotate) != 0 ? rot : 0f,
                    tryImage.Bounds.Center.ToVector2(), SpriteEffects.None, 0f);

            sB.Draw(tryImage,
                new Rectangle(
                    ResolutionManager.VIRTUAL_RESOLUTION.Center.X +
                    (int) (set.Offset.X * ResolutionManager.VIRTUAL_RESOLUTION.Width),
                    ResolutionManager.VIRTUAL_RESOLUTION.Center.Y +
                    (int) (set.Offset.Y * ResolutionManager.VIRTUAL_RESOLUTION.Height),
                    (int) (MathHelper.Min(tryImage.Width, ResolutionManager.VIRTUAL_RESOLUTION.Width) * mid),
                    (int) (MathHelper.Min(tryImage.Height, ResolutionManager.VIRTUAL_RESOLUTION.Height) * mid)), null,
                set.Tint,
                (set.Mode & ImageMode.Rotate) != 0 ? rot : 0f,
                tryImage.Bounds.Center.ToVector2(), SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (!RealTimeRecording.IsRecording) return;

            if (Keys.L.KeyWasClicked())
                _cam.Locked = !_cam.Locked;

            if (_dicKeys.Any(x => x.StartsWith("3")))
            {
                if (!_menu.IsVisible && !_cam.Locked)
                {
                    _cam.Update(gameTime);
                    Mouse.SetPosition(ResolutionManager.REAL_RESOLUTION.Center.X,
                        ResolutionManager.REAL_RESOLUTION.Center.Y);
                }

                if (_dicKeys.Any(x => x.StartsWith("3F")))
                    _3DFrequencyRenderer.UpdateRenderer(
                        new ReadOnlyCollection<float>(RealTimeRecording.FrequencySpectrum));

                if (_dicKeys.Any(x => x.StartsWith("3S")))
                    _3DSampleRenderer.UpdateRenderer(
                        new ReadOnlyCollection<float>(RealTimeRecording.CurrentSamples));
            }
            if (_dicKeys.Any(x => x.StartsWith("2F")))
                _2DFrequencyRenderer.UpdateRenderer(
                    new ReadOnlyCollection<float>(RealTimeRecording.FrequencySpectrum));

            if (_dicKeys.Any(x => x.StartsWith("2S")))
                _2DSampleRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.CurrentSamples));

            if (Keys.Escape.KeyWasClicked())
            {
                _menu.IsVisible = !_menu.IsVisible;
                Game1.FreeBeer.IsMouseVisible = _menu.IsVisible;
            }

            _rainbowGradiant += (float) gameTime.ElapsedGameTime.TotalMilliseconds * .00001f *
                                _rainbowgradiantMultiplier;
            _breathingGradiant += (float) gameTime.ElapsedGameTime.TotalMilliseconds * .0002f *
                                  _breathinggradiantMultiplier;

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