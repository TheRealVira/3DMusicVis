#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: LoadFromSetting.cs
// Date - created:2016.09.19 - 15:03
// Date - current: 2016.09.19 - 16:56

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Manager;
using _3DMusicVis2.Screen.LoadSetting;
using _3DMusicVis2.Setting;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class LoadFromSetting : Screen
    {
        private readonly Button _back;
        private readonly KindOfLoadingSettingScreen _myKind;
        private readonly Button _new;
        private readonly List<Setting.Setting> _settings;
        private readonly ListBox _settingsBox;
        private readonly Button _use;

        public LoadFromSetting(GraphicsDeviceManager gdm, KindOfLoadingSettingScreen kind)
            : base(gdm, "Load from Setting")
        {
            _settings = SettingsManager.LoadSettings();
            _myKind = kind;

            _use = new Button(new Rectangle(100, Game1.VIRTUAL_RESOLUTION.Height - 200, 200, 50), Game1.FamouseOnePixel,
                Game1.InformationFont, _myKind == KindOfLoadingSettingScreen.OnlyLoad ? "Load" : "Edit");
            _back = new Button(new Rectangle(100, Game1.VIRTUAL_RESOLUTION.Height - 100, 200, 50), Game1.FamouseOnePixel,
                Game1.InformationFont, "Back");
            _settingsBox = new ListBox(new Rectangle(100, 100, 300, 500), Game1.FamouseOnePixel,
                _settings.Select(x => x.SettingName).ToList(), Game1.FamouseOnePixel, Game1.InformationFont);

            _back.MousePressed += _back_MousePressed;
            _settingsBox.ItemWasSelected += _settingsBox_ItemWasSelected;
            _use.MousePressed += _use_MousePressed;

            _use.IsVisible = false;

            if (_myKind == KindOfLoadingSettingScreen.OnlyLoad) return;

            _new =
                new Button(
                    new Rectangle(Game1.VIRTUAL_RESOLUTION.Width - 230, Game1.VIRTUAL_RESOLUTION.Height - 100, 200, 50),
                    Game1.FamouseOnePixel,
                    Game1.InformationFont, "New");
            _new.MousePressed += _new_MousePressed;
        }

        private void _new_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.TempLoadScreen(new EditForm(Game1.Graphics));
        }

        private void _use_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.UnloadAll();

            switch (_myKind)
            {
                case KindOfLoadingSettingScreen.OnlyLoad:
                    ScreenManager.TempLoadScreen(new RenderForm(Game1.Graphics,
                        _settings.First(x => x.SettingName == _settingsBox.SelectedItem.Text)));
                    break;

                case KindOfLoadingSettingScreen.LoadOrCreate:
                    ScreenManager.TempLoadScreen(new EditForm(Game1.Graphics,
                        _settings.First(x => x.SettingName == _settingsBox.SelectedItem.Text)));
                    break;
            }
        }

        private void _settingsBox_ItemWasSelected(object sender, EventArgs e)
        {
            if (_settingsBox.SelectedItem != null)
            {
                _use.IsVisible = true;
            }
        }

        private void _back_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.LoadNextScreen(this);
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            _use.Draw(gameTime, sB);
            _back.Draw(gameTime, sB);
            _settingsBox.Draw(gameTime, sB, 3);

            _new?.Draw(gameTime, sB);
        }

        public override void Update(GameTime gameTime)
        {
            _use.Update(gameTime);
            _back.Update(gameTime);
            _settingsBox.Update(gameTime);

            _new?.Update(gameTime);
        }
    }
}