#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: OkPrompt.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.11.26 - 14:25

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
    internal class OkPrompt : Screen, IPrompt
    {
        private readonly Label _myText;
        public readonly Button Okay;
        public bool IsVisible;

        public OkPrompt(string prompt, GraphicsDeviceManager gdm) : base(gdm, "OkPrompt")
        {
            Game1.FreeBeer.IsMouseVisible = true;

            _myText =
                new Label(
                    new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width/2 - 80,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height/2 - 130, 160,
                        50), Game1.FamouseOnePixel, Game1.InformationFont, prompt)
                {DrawColor = Color.Transparent};

            Okay =
                new Button(
                    new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width/2 - 80,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height/2 + 80, 160,
                        50),
                    Game1.FamouseOnePixel, Game1.InformationFont, "Okay");
        }

        public void SetPrompt(string text)
        {
            _myText.Text = text;
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            sB.Draw(Game1.FamouseOnePixel, ResolutionManager.VIRTUAL_RESOLUTION, new Color(50, 50, 50, 1));
            sB.Draw(Game1.FamouseOnePixel,
                new Rectangle(ResolutionManager.VIRTUAL_RESOLUTION.Width/2 - 300,
                    ResolutionManager.VIRTUAL_RESOLUTION.Height/2 - 150, 600, 300),
                new Color(10, 10, 10));

            _myText.Draw(gameTime, sB);
            Okay.Draw(gameTime, sB);

            sB.End();
        }

        public override void Update(GameTime gameTime)
        {
            _myText.Update(gameTime);
            Okay.Update(gameTime);

            if (Keys.Escape.KeyWasClicked())
            {
                ScreenManager.Unload(this);
            }
        }
    }
}