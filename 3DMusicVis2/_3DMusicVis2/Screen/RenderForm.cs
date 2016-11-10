#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: RenderForm.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Manager;
using _3DMusicVis2.RecordingType;
using _3DMusicVis2.RenderFrame;
using _3DMusicVis2.Setting.Visualizer;
using _3DMusicVis2.Shader;
using DrawMode = _3DMusicVis2.Setting.Visualizer.DrawMode;
using Keys = Microsoft.Xna.Framework.Input.Keys;

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
        public Color NotSelectedColor = Color.White;

        public SettingsBundle Selected;
        public Color SelectedColor = Color.Red;
        public bool UseColor = true;

        public bool UseShader = true;

        private Dictionary<string, RenderTarget2D> BufferedDrawer;

        private List<string> DicKeys;

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

            DicKeys = new List<string>();
            BufferedDrawer = new Dictionary<string, RenderTarget2D>();
            currentSetting.Bundles.ForEach(x =>
            {
                if (BufferedDrawer.ContainsKey(x.ToString())) return;

                BufferedDrawer.Add(x.ToString(), new RenderTarget2D(GDM.GraphicsDevice, Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height));
                DicKeys.Add(x.ToString());
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
            BufferedDrawer.Clear();
        }

        private void DisposeBuffered()
        {
            foreach (var renderTarget2D in BufferedDrawer)
            {
                renderTarget2D.Value.Dispose();
            }
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            foreach (var key in DicKeys)
            {
                switch (key)
                {
                    case "2FDashed":
                        RenderTarget2D temp1 = BufferedDrawer["2FDashed"];
                        _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Dashed, ref temp1);
                        continue;
                    case "2FBlocked":
                        RenderTarget2D temp2 = BufferedDrawer["2FBlocked"];
                        _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Blocked, ref temp2);
                        continue;
                    case "2FConnected":
                        var temp3 = BufferedDrawer["2FConnected"];
                        _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Connected, ref temp3);
                        continue;
                    case "2SDashed":
                        var temp4 = BufferedDrawer["2SDashed"];
                        _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Dashed, ref temp4);
                        continue;
                    case "2SBlocked":
                        var temp5 = BufferedDrawer["2SBlocked"];
                        _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Blocked, ref temp5);
                        continue;
                    case "2SConnected":
                        var temp6 = BufferedDrawer["2SConnected"];
                        _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam, DrawMode.Connected, ref temp6);
                        continue;
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
                    x: (int) (pos.X * Game1.VIRTUAL_RESOLUTION.Width + (Game1.VIRTUAL_RESOLUTION.Width * scale.X) / 2),
                    y: (int) (pos.Y * Game1.VIRTUAL_RESOLUTION.Height + (Game1.VIRTUAL_RESOLUTION.Height * scale.Y) / 2),
                    width: (int) (scale.X*Game1.VIRTUAL_RESOLUTION.Width),
                    height: (int) (scale.Y*Game1.VIRTUAL_RESOLUTION.Height));

                sB.Draw(
                    BufferedDrawer[_mySetting.Bundles[i].ToString()],
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
            sB.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null);

            Texture2D backTryImage;
            if (ImageManager.Images.TryGetValue(_mySetting.BackgroundImage ?? "", out backTryImage))
            {
                var height = MathHelper.Min(backTryImage.Height, Game1.VIRTUAL_RESOLUTION.Height);
                var width = MathHelper.Min(backTryImage.Width, Game1.VIRTUAL_RESOLUTION.Width);

                sB.Draw(backTryImage, new Rectangle(0, 0, (int)width, (int)height), Color.White);
            }

            sB.Draw(toUse, Game1.VIRTUAL_RESOLUTION, Color.White);

            Texture2D foreTryImage;
            if (ImageManager.Images.TryGetValue(_mySetting.ForegroundImage ?? "", out foreTryImage))
            {
                var height = MathHelper.Min(foreTryImage.Height, Game1.VIRTUAL_RESOLUTION.Height);
                var width = MathHelper.Min(foreTryImage.Width, Game1.VIRTUAL_RESOLUTION.Width);

                sB.Draw(foreTryImage, new Rectangle(0,0,(int)width, (int)height), Color.White);
            }

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