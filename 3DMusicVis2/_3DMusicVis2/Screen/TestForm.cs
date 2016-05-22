#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: TestForm.cs
// Date - created: 2016.05.22 - 11:30
// Date - current: 2016.05.22 - 16:48

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using _3DMusicVis2.RenderFrame;
using _3DMusicVis2.VisualControls;

#endregion

namespace _3DMusicVis2.Screen
{
    class TestForm : Screen
    {
        private readonly Button MyButton;
        private readonly Label MyLabel;
        private readonly ListBox MyListBox;
        private readonly VisualizationData visData = new VisualizationData();
        private readonly Camera _cam;

        public TestForm(GraphicsDeviceManager gdm) : base(gdm)
        {
            MyButton = new Button(new Rectangle(100, 100, 200, 50), Game1.GhostPixel, Game1.InformationFont, "MyButton");
            MyLabel = new Label(new Rectangle(100, 200, 200, 50), Game1.GhostPixel, Game1.InformationFont, "MyLabel");
            MyListBox = new ListBox(new Rectangle(100, 300, 200, 500), Game1.GhostPixel,
                new[] {"MyListBox_1", "MyListBox_2", "MyListBox_3", "MyListBox_4", "MyListBox_5", "MyListBox_6"}.ToList(),
                Game1.GhostPixel, Game1.InformationFont);

            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
            Game1.FreeBeer.IsMouseVisible = true;

            var temp = new List<SpecialSong>();
            temp.AddRange(from item in Directory.GetFiles(@"3DMusicVis2\Music", "*.wav")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GDM.GraphicsDevice));
            MediaPlayer.Play(temp[0].MySong);
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            //GDM.GraphicsDevice.Clear(Color.Black);
            var temp = _2DFrequencyRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            sB.Draw(temp, Game1.VIRTUAL_RESOLUTION, Color.White);
            //sB.Draw(temp, new Rectangle(Game1.VIRTUAL_RESOLUTION.Width/2,0, Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height), new Color(0,0,255,100));
            sB.Draw(_2DSampleRenderer.Target(GDM.GraphicsDevice, gameTime, _cam), Game1.VIRTUAL_RESOLUTION, Color.White);
            //sB.Draw(_3DCirclularWaveRenderer.Target(GDM.GraphicsDevice, gameTime, _cam), Game1.VIRTUAL_RESOLUTION, new Color(100,100,100,100));

            MyButton.Draw(gameTime, sB, 2);
            MyLabel.Draw(gameTime, sB);
            MyListBox.Draw(gameTime, sB, 2);
            GDM.GraphicsDevice.Clear(Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            MediaPlayer.GetVisualizationData(visData);
            Game1.AudioAnalysis.Update();
            Game1.AudioAnalysis.updateAverageLowFrequencyData();
            Game1.AudioAnalysis.updateAverageMidFrequencyData();
            Game1.AudioAnalysis.updateAverageHighFrequencyData();
            _2DSampleRenderer.UpdateRenderer(visData.Samples);
            _2DFrequencyRenderer.UpdateRenderer(visData.Frequencies);
            //_3DCirclularWaveRenderer.UpdateRenderer(visData.Frequencies);

            MyButton.Update(gameTime);
            MyLabel.Update(gameTime);
            MyListBox.Update(gameTime);
        }
    }
}