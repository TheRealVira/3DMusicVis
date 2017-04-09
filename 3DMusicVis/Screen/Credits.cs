#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Credits.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis.Manager;
using _3DMusicVis.VisualControls;

#endregion

namespace _3DMusicVis.Screen
{
    internal class Credits : Screen
    {
        //private float waiting = 1000;

        private readonly Button _back;
        private readonly ListBox _namingList;

        public Credits(GraphicsDeviceManager gdm) : base(gdm, "Credits")
        {
            _namingList =
                new ListBox(
                    new Rectangle(0, 0, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                        ResolutionManager.VIRTUAL_RESOLUTION.Height),
                    Game1.GhostPixel, new[]
                    {
                        "Copyright (c) 2015 Vira"
                    }.ToList(), Game1.GhostPixel, Game1.InformationFont);

            _namingList.Scroll(ResolutionManager.VIRTUAL_RESOLUTION.Height / 4);

            _back = new Button(new Rectangle(100, ResolutionManager.VIRTUAL_RESOLUTION.Height - 150, 200, 50),
                Game1.FamouseOnePixel,
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

            sB.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null);
            _namingList.Draw(gameTime, sB);
            _back.Draw(gameTime, sB);
            sB.End();
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