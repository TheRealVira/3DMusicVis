#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: EditForm.cs
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
using _3DMusicVis.RenderFrame._2D;
using _3DMusicVis.Screen.Prompt;
using _3DMusicVis.Setting.Visualizer;
using _3DMusicVis.VisualControls;

#endregion

namespace _3DMusicVis.Screen
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

            _currentItems = new ListBox(new Rectangle(20, 20, 200, ResolutionManager.VIRTUAL_RESOLUTION.Height - 40),
                Game1.FamouseOnePixel, _loaded.Bundles.Select(x => x.ToString()).ToList(), Game1.FamouseOnePixel,
                Game1.InformationFont);

            _save =
                new Button(
                    new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width - 130,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height - 80, 100, 50),
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
            backRenderer.Selected = _loaded.Bundles.Count == 0
                ? new SettingsBundle()
                : _loaded.Bundles[_currentItems.SelectedIndex];

            backRenderer.Draw(sB, gameTime);

            sB.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null);
            _currentItems.Draw(gameTime, sB, 3);
            _save.Draw(gameTime, sB);

            if (_menu.IsVisible)
                _menu.Draw(sB, gameTime);

            if (_prompt.IsVisible)
                _prompt.Draw(sB, gameTime);

            sB.End();
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
                _menu.Update(gameTime);

            if (_prompt.IsVisible)
                _prompt.Update(gameTime);
        }
    }
}