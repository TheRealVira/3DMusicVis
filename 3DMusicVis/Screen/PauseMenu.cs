#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: PauseMenu.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis.Manager;
using _3DMusicVis.VisualControls;

#endregion

namespace _3DMusicVis.Screen
{
    internal class PauseMenu : Screen
    {
        private readonly Button _exit;
        private readonly Label _hadder;
        private readonly Button _menu;
        private readonly Button _resume;
        public bool IsVisible;

        public PauseMenu(GraphicsDeviceManager gdm) : base(gdm, "Pause Menu")
        {
            _hadder =
                new Label(
                        new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width / 2 - 80,
                            ResolutionManager.VIRTUAL_RESOLUTION.Height / 2 - 280, 160,
                            50), Game1.FamouseOnePixel, Game1.InformationFont, "Menu")
                    {DrawColor = Color.Transparent};

            _resume =
                new Button(
                    new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width / 2 - 80,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height / 2, 160, 50),
                    Game1.FamouseOnePixel, Game1.InformationFont, "Resume");

            _menu =
                new Button(
                    new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width / 2 - 80,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height / 2 - 100, 160,
                        50), Game1.FamouseOnePixel, Game1.InformationFont, "Main Menu");

            _exit =
                new Button(
                    new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width / 2 - 80,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height / 2 + 100, 160,
                        50),
                    Game1.FamouseOnePixel, Game1.InformationFont, "Exit");

            _menu.MousePressed += Menu_MousePressed;
            _resume.MousePressed += Resume_MousePressed;
            _exit.MousePressed += _exit_MousePressed;
        }

        private void _exit_MousePressed(object sender, EventArgs e)
        {
            Game1.FreeBeer.Exit();
        }

        private void Resume_MousePressed(object sender, EventArgs e)
        {
            IsVisible = false;
            Game1.FreeBeer.IsMouseVisible = IsVisible;
        }

        private void Menu_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.UnloadAll();
            ScreenManager.TempLoadScreen(new MainMenu(GDM));
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            // sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, null);
            sB.Draw(Game1.FamouseOnePixel, ResolutionManager.VIRTUAL_RESOLUTION, new Color(50, 50, 50, 1));
            sB.Draw(Game1.FamouseOnePixel,
                new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width / 2 - 150,
                    ResolutionManager.VIRTUAL_RESOLUTION.Height / 2 - 300, 300, 600),
                new Color(10, 10, 10));

            _hadder.Draw(gameTime, sB);
            _menu.Draw(gameTime, sB);
            _resume.Draw(gameTime, sB);
            _exit.Draw(gameTime, sB);

            // sB.End();
        }

        public override void Update(GameTime gameTime)
        {
            _hadder.Update(gameTime);
            _menu.Update(gameTime);
            _resume.Update(gameTime);
            _exit.Update(gameTime);

            if (Keys.Escape.KeyWasClicked())
                ScreenManager.Unload(this);
        }
    }
}