#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: LoadFromSetting.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Manager;
using _3DMusicVis2.Screen.Prompt;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen.LoadSetting
{
    internal class LoadFromSetting : Screen
    {
        private readonly Button _back;
        private readonly Button _delete;
        private readonly OkPrompt _deletedPrompt;

        private readonly YesNoPrompt _deletePrompt;
        private readonly KindOfLoadingSettingScreen _myKind;
        private readonly Button _new;
        private readonly List<Setting.Visualizer.Setting> _settings;
        private readonly ListBox _settingsBox;
        private readonly Button _use;

        public LoadFromSetting(GraphicsDeviceManager gdm, KindOfLoadingSettingScreen kind)
            : base(gdm, "Load from Setting")
        {
            _settings = SettingsManager.Load<Setting.Visualizer.Setting>(SettingsManager.SETTINGS_DIR,
                SettingsManager.SETTINGS_EXT);
            _myKind = kind;

            _delete = new Button(new Rectangle(100, Game1.VIRTUAL_RESOLUTION.Height - 300, 200, 50),
                Game1.FamouseOnePixel,
                Game1.InformationFont, "Delete");
            _use = new Button(new Rectangle(100, Game1.VIRTUAL_RESOLUTION.Height - 200, 200, 50), Game1.FamouseOnePixel,
                Game1.InformationFont, _myKind == KindOfLoadingSettingScreen.OnlyLoad ? "Load" : "Edit");
            _back = new Button(new Rectangle(100, Game1.VIRTUAL_RESOLUTION.Height - 100, 200, 50), Game1.FamouseOnePixel,
                Game1.InformationFont, "Back");

            _settingsBox = new ListBox(new Rectangle(100, 100, 300, 500), Game1.FamouseOnePixel,
                _settings.Select(x => x.SettingName).ToList(), Game1.FamouseOnePixel, Game1.InformationFont);

            _back.MousePressed += _back_MousePressed;
            _settingsBox.ItemWasSelected += _settingsBox_ItemWasSelected;
            _use.MousePressed += _use_MousePressed;
            _delete.MousePressed += _delete_MousePressed;

            _delete.IsVisible = false;
            _use.IsVisible = false;

            _deletePrompt = new YesNoPrompt("Temp", gdm);
            _deletePrompt.Yes.MousePressed += Yes_MousePressed;
            _deletePrompt.No.MousePressed += No_MousePressed;
            _deletePrompt.IsVisible = false;

            _deletedPrompt = new OkPrompt("Temp", gdm);
            _deletedPrompt.Okay.MousePressed += Okay_MousePressed;

            if (_myKind == KindOfLoadingSettingScreen.OnlyLoad) return;

            _new =
                new Button(
                    new Rectangle(Game1.VIRTUAL_RESOLUTION.Width - 230, Game1.VIRTUAL_RESOLUTION.Height - 100, 200, 50),
                    Game1.FamouseOnePixel,
                    Game1.InformationFont, "New");
            _new.MousePressed += _new_MousePressed;
        }

        private void Okay_MousePressed(object sender, EventArgs e)
        {
            _deletedPrompt.IsVisible = false;
        }

        private void No_MousePressed(object sender, EventArgs e)
        {
            _deletePrompt.IsVisible = false;
        }

        private void Yes_MousePressed(object sender, EventArgs e)
        {
            _deletePrompt.IsVisible = false;
            var temp = _settingsBox.SelectedItem.Text;
            SettingsManager.Delete(SettingsManager.SETTINGS_DIR, _settingsBox.SelectedItem.Text,
                SettingsManager.SETTINGS_EXT);
            _settingsBox.RemoveItem(_settingsBox.SelectedItem);

            _deletedPrompt.SetPrompt("Deleted:  " + temp);
            _deletedPrompt.IsVisible = true;
        }

        private void _delete_MousePressed(object sender, EventArgs e)
        {
            _deletePrompt.SetPrompt("Do you really want to delete \"" + _settingsBox.SelectedItem.Text + "\"?");
            _deletePrompt.IsVisible = true;
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
            if (_settingsBox.SelectedItem == null) return;
            _use.IsVisible = true;

            if (_new == null) return;

            _delete.IsVisible = true;
        }

        private void _back_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.LoadNextScreen(this);
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            _delete.Draw(gameTime, sB);
            _use.Draw(gameTime, sB);
            _back.Draw(gameTime, sB);
            _settingsBox.Draw(gameTime, sB, 3);

            _new?.Draw(gameTime, sB);

            if (_deletedPrompt.IsVisible)
            {
                _deletedPrompt.Draw(sB, gameTime);
            }

            if (_deletePrompt.IsVisible)
            {
                _deletePrompt.Draw(sB, gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_deletedPrompt.IsVisible)
            {
                _deletedPrompt.Update(gameTime);
                return;
            }

            if (_deletePrompt.IsVisible)
            {
                _deletePrompt.Update(gameTime);
                return;
            }

            _delete.Update(gameTime);
            _use.Update(gameTime);
            _back.Update(gameTime);
            _settingsBox.Update(gameTime);

            _new?.Update(gameTime);
        }
    }
}