#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Console.cs
// Date - created:2016.12.10 - 09:41
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;
using _3DMusicVis.OutputConsole;
using _3DMusicVis.OutPutConsole;

#endregion

namespace _3DMusicVis.VisualControls
{
    public class Console : VisualControl
    {
        public static Rectangle ConsoleBoundings = new Rectangle(0, ResolutionManager.VIRTUAL_RESOLUTION.Height - 250,
            500, 250);

        private readonly Label _consoleSignLabel;

        private readonly ListBox _lines;
        private readonly int MAX_CONSOLE_TEXT = 7;

        public Console(Rectangle bounding, Texture2D texture)
            : base(bounding, texture, DefaultDrawColor, Color.White, Color.White)
        {
            var writer = new OutputManagerEventHandeling();

            writer.Writing += Write;
            writer.Writlineing += WriteLine;
            System.Console.SetOut(writer);

            _lines =
                new ListBox(
                    new Rectangle(Bounding.X, Bounding.Y, Bounding.Width, Bounding.Height - 20),
                    Game1.FamouseOnePixel, new List<string>(), Game1.FamouseOnePixel, Game1.ConsoleFont);
            //ConsoleSignLabel =new Label(new Rectangle(this.Bounding.X+1,this.Bounding.Y+this.Bounding.Height-200,10,10), Game1.FamouseOnePixel,Game1.ConsoleFont,">_");
            _consoleSignLabel = new Label(new Rectangle(Bounding.X + 1, Bounding.Y + Bounding.Height - 20, 20, 20),
                Game1.FamouseOnePixel, Game1.ConsoleFont, ">_");
        }

        public void WriteLine(object sender, OutputManagerEventArgs e)
        {
            if (_lines.Items.Count == 0 || _lines.Items[_lines.Items.Count - 1].Text.EndsWith("\n"))
                _lines.Add(e.Text + "\n", Game1.FamouseOnePixel, Game1.ConsoleFont);
            else
                _lines.Items[_lines.Items.Count - 1].Text += e.Text + "\n";

            if (_lines.Items.Count > MAX_CONSOLE_TEXT)
                _lines.Items.RemoveAt(0);
        }

        public void Write(object sender, OutputManagerEventArgs e)
        {
            if (_lines.Items.Count == 0 || _lines.Items[_lines.Items.Count - 1].Text.EndsWith("\n"))
                _lines.Add(e.Text, Game1.FamouseOnePixel, Game1.ConsoleFont);
            else
                _lines.Items[_lines.Items.Count - 1].Text += e.Text;

            if (_lines.Items.Count > MAX_CONSOLE_TEXT)
                _lines.Items.RemoveAt(0);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            base.Draw(gameTime, spriteBatch);

            _lines.Draw(gameTime, spriteBatch, 2);
            _consoleSignLabel.Draw(gameTime, spriteBatch, 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsVisible) return;

            base.Update(gameTime);

            if (!(_lines.Items.Count > 0)) return;

            _lines.Scroll(-1);
            _lines.Update(gameTime);
        }
    }
}