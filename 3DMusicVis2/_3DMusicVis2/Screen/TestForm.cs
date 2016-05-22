#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: TestForm.cs
// Date - created: 2016.05.22 - 11:30
// Date - current: 2016.05.22 - 12:52

#endregion

#region Usings

using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    class TestForm : Screen
    {
        private readonly Button MyButton;
        private readonly Label MyLabel;
        private readonly ListBox MyListBox;

        public TestForm(GraphicsDeviceManager gdm) : base(gdm)
        {
            MyButton = new Button(new Rectangle(100, 100, 200, 50), Game1.GhostPixel, Game1.InformationFont, "MyButton");
            MyLabel = new Label(new Rectangle(100, 200, 200, 50), Game1.GhostPixel, Game1.InformationFont, "MyLabel");
            MyListBox = new ListBox(new Rectangle(100, 300, 200, 500), Game1.GhostPixel,
                new[] {"MyListBox_1", "MyListBox_2", "MyListBox_3", "MyListBox_4", "MyListBox_5", "MyListBox_6"}.ToList(),
                Game1.GhostPixel, Game1.InformationFont);

            Game1.FreeBeer.IsMouseVisible = true;
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            MyButton.Draw(gameTime, sB, 2);
            MyLabel.Draw(gameTime, sB);
            MyListBox.Draw(gameTime, sB, 2);
        }

        public override void Update(GameTime gameTime)
        {
            MyButton.Update(gameTime);
            MyLabel.Update(gameTime);
            MyListBox.Update(gameTime);
        }
    }
}