#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Credits.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.10.17 - 20:43

#endregion

#region Usings

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Manager;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class Credits : Screen
    {
        //private float waiting = 1000;

        private readonly Button _back;
        private readonly ListBox _namingList;

        public Credits(GraphicsDeviceManager gdm) : base(gdm, "Credits")
        {
            _namingList =
                new ListBox(new Rectangle(0, 0, Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height),
                    Game1.GhostPixel, new[]
                    {
                        "Audio Analysis Framework - Created by Stephen Hoult. (\"http://bit.ly/1U6BtBU\")",
                        "All other stuff - Copyright (c) 2015 Vira"
                    }.ToList(), Game1.GhostPixel, Game1.InformationFont);

            _namingList.Scroll(Game1.VIRTUAL_RESOLUTION.Height/4);

            _back = new Button(new Rectangle(100, Game1.VIRTUAL_RESOLUTION.Height - 150, 200, 50), Game1.FamouseOnePixel,
                Game1.InformationFont, "Back");
            _back.MousePressed += Back_MousePressed;
        }

        private void Back_MousePressed(object sender, EventArgs e)
        {
            ScreenManager.LoadNextScreen(this);
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            GDM.GraphicsDevice.Clear(Color.Black);
            _namingList.Draw(gameTime, sB);

            _back.Draw(gameTime, sB);
        }

        public override void Update(GameTime gameTime)
        {
            _back.Update(gameTime);
            //if (waiting > 0)
            //{
            //    waiting -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
            //    return;
            //}

            //_namingList.Scroll(-1);

            //if (_namingList.AllItemsAreOutSideTheBounding())
            //{
            //    ScreenManager.LoadNextScreen(this);
            //}
        }
    }
}