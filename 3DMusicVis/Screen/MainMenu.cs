#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: MainMenu.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis.Manager;
using _3DMusicVis.Screen.LoadSetting;
using _3DMusicVis.VisualControls;

#endregion

namespace _3DMusicVis.Screen
{
    internal class MainMenu : Screen
    {
        private readonly Button _credits;
        private readonly Button _edit;
        private readonly Button _exit;
        private readonly Button _load;

        public MainMenu(GraphicsDeviceManager gdm) : base(gdm, "Main Menu")
        {
            _load = new Button(new Rectangle(100, 200, 200, 50), Game1.FamouseOnePixel, Game1.InformationFont, "Load");
            _edit = new Button(new Rectangle(100, 300, 200, 50), Game1.FamouseOnePixel, Game1.InformationFont, "Edit");
            _credits = new Button(new Rectangle(100, 400, 200, 50), Game1.FamouseOnePixel, Game1.InformationFont,
                "Credits");
            _exit = new Button(new Rectangle(100, 500, 200, 50), Game1.FamouseOnePixel, Game1.InformationFont, "Exit");

            _load.MousePressed += _load_MousePressed;
            _edit.MousePressed += _edit_MousePressed;
            _credits.MousePressed += _credits_MousePressed;
            _exit.MousePressed += _exit_MousePressed;
        }

        private void _edit_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.TempLoadScreen(new LoadFromSetting(Game1.Graphics, KindOfLoadingSettingScreen.LoadOrCreate));
        }

        private void _load_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.TempLoadScreen(new LoadFromSetting(Game1.Graphics, KindOfLoadingSettingScreen.OnlyLoad));
        }

        private void _credits_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.TempLoadScreen(new Credits(Game1.Graphics));
        }

        private void _exit_MousePressed(object sender, EventArgs e)
        {
            Game1.FreeBeer.Exit();
        }

        public override void LoadedUp()
        {
            base.LoadedUp();

            Game1.FreeBeer.IsMouseVisible = true;
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            sB.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null);
            _load.Draw(gameTime, sB);
            _edit.Draw(gameTime, sB);
            _credits.Draw(gameTime, sB);
            _exit.Draw(gameTime, sB);
            sB.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keys.Escape.KeyWasClicked())
            {
                ScreenManager.UnloadAll();
                Game1.FreeBeer.Exit();
                return;
            }

            _load.Update(gameTime);
            _edit.Update(gameTime);
            _credits.Update(gameTime);
            _exit.Update(gameTime);
        }
    }
}