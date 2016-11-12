#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: EditForm.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.11.11 - 09:51

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
using _3DMusicVis2.Setting.Visualizer;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class EditForm : Screen
    {
        private readonly Setting.Visualizer.Setting _loaded;
        private Camera _cam;
        private ListBox _currentItems;
        private PauseMenu _menu;

        private OkPrompt _prompt;

        private Button _save;
        private RenderForm backRenderer;
        private bool somePromptIsOpen;

        public EditForm(GraphicsDeviceManager gdm) : base(gdm, "Editform")
        {
            _loaded = new Setting.Visualizer.Setting
            {
                Bundles = new List<SettingsBundle>(),
                SettingName = "Temp"
            };

            Initialize(gdm);
        }

        public EditForm(GraphicsDeviceManager gdm, Setting.Visualizer.Setting loaded) : base(gdm, "Editform")
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

            backRenderer = new RenderForm(gdm, _loaded)
            {
                UseColor = false,
                UseShader = false
            };
        }

        private void Okay_MousePressed(object sender, EventArgs e)
        {
            _prompt.IsVisible = false;
            somePromptIsOpen = false;
        }

        private void _save_MousePressed(object sender, EventArgs e)
        {
            SettingsManager.Save(_loaded, SettingsManager.SETTINGS_DIR, _loaded.SettingName,
                SettingsManager.SETTINGS_EXT);
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
            backRenderer.Selected = _loaded.Bundles[_currentItems.SelectedIndex];

            backRenderer.Draw(sB, gameTime);

            sB.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null);
            _currentItems.Draw(gameTime, sB, 3);
            _save.Draw(gameTime, sB);

            sB.End();

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