#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Credits.cs
// Date - created: 2016.05.22 - 11:49
// Date - current: 2016.05.22 - 16:48

#endregion

#region Usings

using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Manager;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    class Credits : Screen
    {
        private readonly ListBox _namingList;

        private float waiting = 1000;

        public Credits(GraphicsDeviceManager gdm) : base(gdm)
        {
            _namingList =
                new ListBox(new Rectangle(0, 0, Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height),
                    Game1.GhostPixel, new[]
                    {
                        "Audio Analysis Framework - Created by Stephen Hoult. (\"http://bit.ly/1U6BtBU\")",
                        "Resolution calculations - Copyright (c) 2010 David Amador (\"http://www.david-amador.com\")",
                        "All other stuff - Copyright (c) Vira"
                    }.ToList(), Game1.GhostPixel, Game1.InformationFont);

            _namingList.Scroll(Game1.VIRTUAL_RESOLUTION.Height/2);
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            GDM.GraphicsDevice.Clear(Color.Black);
            _namingList.Draw(gameTime, sB);
        }

        public override void Update(GameTime gameTime)
        {
            if (waiting > 0)
            {
                waiting -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                return;
            }

            _namingList.Scroll(-1);

            if (_namingList.AllItemsAreOutSideTheBounding())
            {
                ScreenManager.Delete(this);
            }
        }
    }
}