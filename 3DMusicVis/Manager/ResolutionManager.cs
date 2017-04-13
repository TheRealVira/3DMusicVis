#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: ResolutionManager.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Point = System.Drawing.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

#endregion

namespace _3DMusicVis.Manager
{
    internal static class ResolutionManager
    {
        public static Rectangle VIRTUAL_RESOLUTION = new Rectangle(0, 0, 1920, 1080);
        public static Rectangle REAL_RESOLUTION = new Rectangle(0, 0, 1920, 1080);

        public static Vector2 ResolutionRatio => new Vector2(VIRTUAL_RESOLUTION.Width, VIRTUAL_RESOLUTION.Height) /
                                                 new Vector2(
                                                     Game1.FreeBeer.GraphicsDevice.PresentationParameters.Bounds.Width,
                                                     Game1.FreeBeer.GraphicsDevice.PresentationParameters.Bounds.Height)
        ;

        public static Point WindowLocation => (Control.FromHandle(Game1.FreeBeer.Window.Handle) as Form).Location;

        public static void ApplyResolution(GraphicsDeviceManager gra)
        {
            Game1.FreeBeer.InactiveSleepTime = new TimeSpan(0);
            gra.PreferredBackBufferWidth = REAL_RESOLUTION.Width;
            gra.PreferredBackBufferHeight = REAL_RESOLUTION.Height;
            gra.IsFullScreen = false;
            var window = Control.FromHandle(Game1.FreeBeer.Window.Handle) as Form;
            gra.ApplyChanges();
            window.Location = new Point(10, 10);
        }

        public static void ToggleFullScreen(GraphicsDeviceManager gra)
        {
            var window = Control.FromHandle(Game1.FreeBeer.Window.Handle) as Form;
            var formPosition = new Microsoft.Xna.Framework.Point(window.Location.X, window.Location.Y);
            var dispayXMulitplikator =
                (int) Math.Round(formPosition.X / (decimal) REAL_RESOLUTION.Width);
            if (dispayXMulitplikator == 0)
                if (formPosition.X < -100)
                    dispayXMulitplikator = -1;
                else
                    dispayXMulitplikator = 1;
            var displayXMultiplikatorForLocation = dispayXMulitplikator;
            //int dispayYMulitplikator = this.GraphicsDevice.Adapter.CurrentDisplayMode.Height / formPosition.Y;

            if (displayXMultiplikatorForLocation > 0)
                displayXMultiplikatorForLocation--;

            if (window.FormBorderStyle == FormBorderStyle.FixedSingle)
            {
                window.FormBorderStyle = FormBorderStyle.None;
                window.Location =
                    new Point(
                        gra.GraphicsDevice.Adapter.CurrentDisplayMode.Width * displayXMultiplikatorForLocation, 0);
                window.Size = new Size(gra.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                    gra.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            }
            else // If fullscreen
            {
                window.FormBorderStyle = FormBorderStyle.FixedSingle;
                window.Location =
                    //new System.Drawing.Point(
                    //    gra.GraphicsDevice.Adapter.CurrentDisplayMode.Width * displayXMultiplikatorForLocation,
                    //    -16);
                    new Point(0, 0);
                window.Size = new Size(REAL_RESOLUTION.Width, REAL_RESOLUTION.Height + 16);
            }
        }
    }
}