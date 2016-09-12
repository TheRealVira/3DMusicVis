#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Button.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.12 - 21:23

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.VisualControls
{
    internal class Button : VisualControl
    {
        private readonly SpriteFont _font;

        public string Text;

        public Button(Rectangle bounding, Texture2D texture, SpriteFont font, string text)
            : base(bounding, texture, DefaultDrawColor, Color.White, Color.White)
        {
            _font = font;
            Text = text;
            MouseHover += Button_MouseHover;
            MouseExits += Button_MouseExits;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, Text,
                new Vector2(Bounding.Center.X - _font.MeasureString(Text).X/2,
                    Bounding.Center.Y - _font.MeasureString(Text).Y/2), FontColor);
        }

        public void Button_MouseExits(object sender, EventArgs e)
        {
            DrawColor = DefaultDrawColor;
        }

        public void Button_MouseHover(object sender, EventArgs e)
        {
            DrawColor = Color.Blue;
        }
    }
}