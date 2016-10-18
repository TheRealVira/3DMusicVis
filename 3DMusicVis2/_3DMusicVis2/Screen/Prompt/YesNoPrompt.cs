#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: YesNoPrompt.cs
// Date - created:2016.10.10 - 19:16
// Date - current: 2016.10.18 - 18:21

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.Manager;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen.Prompt
{
    internal class YesNoPrompt : Screen, IPrompt
    {
        private readonly Label _myText;
        public readonly Button No;
        public readonly Button Yes;
        public bool IsVisible;

        public YesNoPrompt(string prompt, GraphicsDeviceManager gdm) : base(gdm, "YesNoPrompt")
        {
            Game1.FreeBeer.IsMouseVisible = true;

            _myText =
                new Label(
                    new Rectangle(Game1.VIRTUAL_RESOLUTION.Width/2 - 80, Game1.VIRTUAL_RESOLUTION.Height/2 - 130, 160,
                        50), Game1.FamouseOnePixel, Game1.InformationFont, prompt)
                {DrawColor = Color.Transparent};

            Yes =
                new Button(
                    new Rectangle(Game1.VIRTUAL_RESOLUTION.Width/2 - 170, Game1.VIRTUAL_RESOLUTION.Height/2 + 80, 160,
                        50),
                    Game1.FamouseOnePixel, Game1.InformationFont, "Yes");

            No =
                new Button(
                    new Rectangle(Game1.VIRTUAL_RESOLUTION.Width/2 + 10, Game1.VIRTUAL_RESOLUTION.Height/2 + 80, 160,
                        50),
                    Game1.FamouseOnePixel, Game1.InformationFont, "No");
        }

        public void SetPrompt(string text)
        {
            _myText.Text = text;
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            sB.Draw(Game1.FamouseOnePixel, Game1.VIRTUAL_RESOLUTION, new Color(50, 50, 50, 1));
            sB.Draw(Game1.FamouseOnePixel,
                new Rectangle(Game1.VIRTUAL_RESOLUTION.Width/2 - 300, Game1.VIRTUAL_RESOLUTION.Height/2 - 150, 600, 300),
                new Color(10, 10, 10));
            sB.End();

            _myText.Draw(gameTime, sB);
            No.Draw(gameTime, sB);
            Yes.Draw(gameTime, sB);
        }

        public override void Update(GameTime gameTime)
        {
            _myText.Update(gameTime);
            No.Update(gameTime);
            Yes.Update(gameTime);

            if (Keys.Escape.KeyWasClicked())
            {
                ScreenManager.Unload(this);
            }
        }
    }
}