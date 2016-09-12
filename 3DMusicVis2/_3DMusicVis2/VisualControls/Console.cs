#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Console.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.12 - 21:23

#endregion

#region Usings

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.OutputConsole;
using _3DMusicVis2.OutPutConsole;

#endregion

namespace _3DMusicVis2.VisualControls
{
    public class Console : VisualControl
    {
        public static Rectangle ConsoleBoundings = new Rectangle(0, Game1.VIRTUAL_RESOLUTION.Height - 250, 500, 250);
        private readonly ListBox _lines;

        private readonly Label ConsoleSignLabel;
        private readonly int MAX_CONSOLE_TEXT = 7;

        private readonly OutputManagerEventHandeling Writer;
        private float _timer;
        private float VANISH = 1000;

        public Console(Rectangle bounding, Texture2D texture)
            : base(bounding, texture, DefaultDrawColor, Color.White, Color.White)
        {
            Writer = new OutputManagerEventHandeling();

            Writer.Writing += Write;
            Writer.Writlineing += WriteLine;
            System.Console.SetOut(Writer);

            _lines =
                new ListBox(
                    new Rectangle(Bounding.X, Bounding.Y, Bounding.Width, Bounding.Height - 20),
                    Game1.FamouseOnePixel, new List<string>(), Game1.FamouseOnePixel, Game1.ConsoleFont);
            //ConsoleSignLabel =new Label(new Rectangle(this.Bounding.X+1,this.Bounding.Y+this.Bounding.Height-200,10,10), Game1.FamouseOnePixel,Game1.ConsoleFont,">_");
            ConsoleSignLabel = new Label(new Rectangle(Bounding.X + 1, Bounding.Y + Bounding.Height - 20, 20, 20),
                Game1.FamouseOnePixel, Game1.ConsoleFont, ">_");
        }

        public void WriteLine(object sender, OutputManagerEventArgs e)
        {
            if (_lines.Items.Count == 0 || _lines.Items[_lines.Items.Count - 1].Text.EndsWith("\n"))
            {
                _lines.Add(e.Text + "\n", Game1.FamouseOnePixel, Game1.ConsoleFont);
            }
            else
            {
                _lines.Items[_lines.Items.Count - 1].Text += e.Text + "\n";
            }

            if (_lines.Items.Count > MAX_CONSOLE_TEXT)
            {
                _lines.Items.RemoveAt(0);
            }
        }

        public void Write(object sender, OutputManagerEventArgs e)
        {
            if (_lines.Items.Count == 0 || _lines.Items[_lines.Items.Count - 1].Text.EndsWith("\n"))
            {
                _lines.Add(e.Text, Game1.FamouseOnePixel, Game1.ConsoleFont);
            }
            else
            {
                _lines.Items[_lines.Items.Count - 1].Text += e.Text;
            }

            if (_lines.Items.Count > MAX_CONSOLE_TEXT)
            {
                _lines.Items.RemoveAt(0);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            //int y = Game1.VIRTUAL_RESOLUTION.Height - (int)Game1.ConsoleFont.MeasureString("Test").Y * MAX_CONSOLE_TEXT + Spacing * 2;
            //spriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle(0, y, (int)Game1.ConsoleFont.MeasureString("TestTestTestTestTestTestTestTestTestTestTestTestTestTestTest").X + Spacing * 3, Game1.VIRTUAL_RESOLUTION.Height - y), new Color(100, 100, 100, 255));
            //spriteBatch.Draw(Game1.FamouseOnePixel, new Rectangle(Spacing, y + Spacing, (int)Game1.ConsoleFont.MeasureString("TestTestTestTestTestTestTestTestTestTestTestTestTestTestTest").X + Spacing, Game1.VIRTUAL_RESOLUTION.Height - y - Spacing * 2), new Color(0, 0, 0, 200));

            //int y = this.Bounding.Y+Spacing*2;

            //foreach (var text in Lines.Items)
            //{
            //    spriteBatch.DrawString(Game1.ConsoleFont, text.Text, new Vector2(10, y), FontColor);
            //    y += (int)(Game1.ConsoleFont.MeasureString(text.Text).Y + Spacing);
            //}

            _lines.Draw(gameTime, spriteBatch, 2);
            ConsoleSignLabel.Draw(gameTime, spriteBatch, 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (!(_lines.Count > 0)) return;

            //_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //if (_timer > VANISH)
            //{
            //    _timer = 0;
            //    _lines.RemoveAt(0);
            //}

            _lines.Update(gameTime);
        }
    }
}