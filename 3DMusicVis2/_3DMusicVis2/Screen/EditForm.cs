#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: EditForm.cs
// Date - created:2016.09.19 - 14:54
// Date - current: 2016.10.17 - 20:43

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.RecordingType;
using _3DMusicVis2.RenderFrame;
using _3DMusicVis2.Screen.Prompt;
using _3DMusicVis2.Setting;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class EditForm : Screen
    {
        private readonly Setting.Setting _loaded;
        private Camera _cam;
        private ListBox _currentItems;
        private PauseMenu _menu;

        private OkPrompt _prompt;

        private Button _save;
        private bool somePromptIsOpen;

        public EditForm(GraphicsDeviceManager gdm) : base(gdm, "Editform")
        {
            _loaded = new Setting.Setting
            {
                Bundles = new List<SettingsBundle>(),
                SettingName = "Temp"
            };

            Initialize(gdm);
        }

        public EditForm(GraphicsDeviceManager gdm, Setting.Setting loaded) : base(gdm, "Editform")
        {
            _loaded = loaded;

            Initialize(gdm);
        }

        private void Initialize(GraphicsDeviceManager gdm)
        {
            _menu = new PauseMenu(gdm);
            _prompt = new OkPrompt(_loaded.SettingName + " is saved!", gdm);
            _prompt.Okay.MousePressed += Okay_MousePressed;

            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);

            _currentItems = new ListBox(new Rectangle(20, 20, 200, Game1.VIRTUAL_RESOLUTION.Height - 40),
                Game1.FamouseOnePixel, _loaded.Bundles.Select(x => x.ToString()).ToList(), Game1.FamouseOnePixel,
                Game1.InformationFont);

            _save =
                new Button(
                    new Rectangle(Game1.VIRTUAL_RESOLUTION.Width - 130, Game1.VIRTUAL_RESOLUTION.Height - 80, 100, 50),
                    Game1.FamouseOnePixel, Game1.InformationFont, "Save");
            _save.MousePressed += _save_MousePressed;
        }

        private void Okay_MousePressed(object sender, EventArgs e)
        {
            _prompt.IsVisible = false;
            somePromptIsOpen = false;
        }

        private void _save_MousePressed(object sender, EventArgs e)
        {
            SettingsManager.SaveSetting(_loaded);
            _prompt.IsVisible = true;
            somePromptIsOpen = true;
        }

        public override void LoadedUp()
        {
            base.LoadedUp();
            Game1.FreeBeer.IsMouseVisible = true;

            _2DFrequencyRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.TestFrequencyleData));
            _2DSampleRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.TestSampleData));
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            sB.Begin();

            Texture2D dashedFrequ = null;
            if (_loaded.Bundles.Any(x => x.IsFrequency && x.IsDashed))
            {
                _2DFrequencyRenderer.Dashed = true;
                dashedFrequ = _2DFrequencyRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            Texture2D frequ = null;
            if (_loaded.Bundles.Any(x => x.IsFrequency && !x.IsDashed))
            {
                _2DFrequencyRenderer.Dashed = false;
                frequ = _2DFrequencyRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            Texture2D dashedSamp = null;
            if (_loaded.Bundles.Any(x => !x.IsFrequency && x.IsDashed))
            {
                _2DSampleRenderer.Dashed = true;
                dashedSamp = _2DSampleRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            Texture2D samp = null;
            if (_loaded.Bundles.Any(x => !x.IsFrequency && !x.IsDashed))
            {
                _2DSampleRenderer.Dashed = false;
                samp = _2DSampleRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            }

            for (var i = 0; i < _loaded.Bundles.Count; i++)
            {
                var toDraw = _currentItems.SelectedItem == null
                    ? Color.White
                    : i == _currentItems.SelectedIndex ? _loaded.Bundles[i].Color.Color : Color.White;


                if (_loaded.Bundles[i].IsFrequency)
                {
                    var pos = _loaded.Bundles[i].Trans.Position;
                    var scale = _loaded.Bundles[i].Trans.Scale;
                    sB.Draw(_loaded.Bundles[i].IsDashed ? dashedFrequ : frequ,
                        new Rectangle((int) (pos.X + Game1.VIRTUAL_RESOLUTION.Width/2f),
                            (int) (pos.Y + Game1.VIRTUAL_RESOLUTION.Height/2f),
                            (int) (Game1.VIRTUAL_RESOLUTION.Width*scale.X),
                            (int) (Game1.VIRTUAL_RESOLUTION.Height*scale.Y)), null, toDraw,
                        _loaded.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                        SpriteEffects.None, 0);
                    continue;
                }

                var pos2 = _loaded.Bundles[i].Trans.Position;
                var scale2 = _loaded.Bundles[i].Trans.Scale;
                sB.Draw(_loaded.Bundles[i].IsDashed ? dashedSamp : samp,
                    new Rectangle((int) (pos2.X + Game1.VIRTUAL_RESOLUTION.Width/2f),
                        (int) (pos2.Y + Game1.VIRTUAL_RESOLUTION.Height/2f),
                        (int) (Game1.VIRTUAL_RESOLUTION.Width*scale2.X),
                        (int) (Game1.VIRTUAL_RESOLUTION.Height*scale2.Y)), null, toDraw,
                    _loaded.Bundles[i].Trans.Rotation, Game1.VIRTUAL_RESOLUTION.Center.ToVector2(),
                    SpriteEffects.None, 0);
            }

            GDM.GraphicsDevice.Clear(Color.Black);
            sB.End();

            _currentItems.Draw(gameTime, sB, 3);
            _save.Draw(gameTime, sB);

            if (_menu.IsVisible)
            {
                _menu.Draw(sB, gameTime);
            }

            if (_prompt.IsVisible)
            {
                _prompt.Draw(sB, gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keys.Escape.KeyWasClicked())
            {
                _menu.IsVisible = !_menu.IsVisible && !somePromptIsOpen;
                Game1.FreeBeer.IsMouseVisible = _menu.IsVisible;
                somePromptIsOpen = _menu.IsVisible;
            }

            _currentItems.Update(gameTime);
            _save.Update(gameTime);

            if (_menu.IsVisible)
            {
                _menu.Update(gameTime);
            }

            if (_prompt.IsVisible)
            {
                _prompt.Update(gameTime);
            }
        }
    }
}