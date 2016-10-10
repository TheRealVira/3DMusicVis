#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Label.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.10.10 - 19:36

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.VisualControls
{
    internal class Label : VisualControl
    {
        private readonly SpriteFont _font;

        public string Text;

        public Label(Rectangle bounding, Texture2D texture, SpriteFont font, string text)
            : base(bounding, texture, DefaultDrawColor, Color.White, Color.White)
        {
            _font = font;
            Text = text;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            base.Draw(gameTime, spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, Text,
                new Vector2(Bounding.Center.X - _font.MeasureString(Text).X/2,
                    Bounding.Center.Y - _font.MeasureString(Text).Y/2), FontColor);
            spriteBatch.End();
        }
    }
}