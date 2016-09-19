#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: EditForm.cs
// Date - created:2016.09.19 - 14:54
// Date - current: 2016.09.19 - 15:38

#endregion

#region Usings

using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class EditForm : Screen
    {
        private readonly Camera _cam;
        private readonly Setting.Setting _loaded;
        private readonly PauseMenu _menu;

        private ListBox _currentItems;

        public EditForm(GraphicsDeviceManager gdm) : base(gdm, "Editform")
        {
            _menu = new PauseMenu(gdm);
            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
            _loaded = new Setting.Setting();

            Initialize();
        }

        public EditForm(GraphicsDeviceManager gdm, Setting.Setting loaded) : base(gdm, "Editform")
        {
            _menu = new PauseMenu(gdm);
            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
            _loaded = loaded;

            Initialize();
        }

        private void Initialize()
        {
            _currentItems = new ListBox(new Rectangle(20, 20, 200, Game1.VIRTUAL_RESOLUTION.Height - 40),
                Game1.FamouseOnePixel, _loaded.Bundles.Select(x => x.ToString()).ToList(), Game1.FamouseOnePixel,
                Game1.InformationFont);
        }

        public override void LoadedUp()
        {
            base.LoadedUp();
            Game1.FreeBeer.IsMouseVisible = true;
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            _currentItems.Draw(gameTime, sB);

            if (_menu.IsVisible)
            {
                _menu.Draw(sB, gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keys.Escape.KeyWasClicked())
            {
                _menu.IsVisible = !_menu.IsVisible;
                Game1.FreeBeer.IsMouseVisible = _menu.IsVisible;
            }

            _currentItems.Update(gameTime);

            if (_menu.IsVisible)
            {
                _menu.Update(gameTime);
            }
        }
    }
}