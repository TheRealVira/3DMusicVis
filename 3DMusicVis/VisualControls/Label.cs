#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Label.cs
// Date - created:2016.12.10 - 09:41
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis.VisualControls
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
            spriteBatch.DrawString(_font, Text,
                new Vector2(Bounding.Center.X - _font.MeasureString(Text).X / 2,
                    Bounding.Center.Y - _font.MeasureString(Text).Y / 2), FontColor);
        }
    }
}