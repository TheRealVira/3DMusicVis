#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: TestForm.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.12 - 21:23

#endregion

#region Usings

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using _3DMusicVis2.RecordingType;
using _3DMusicVis2.RenderFrame;
using _3DMusicVis2.VisualControls;
using Console = System.Console;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class TestForm : Screen
    {
        private readonly Camera _cam;
        private readonly Button MyButton;
        private readonly Label MyLabel;
        private readonly ListBox MyListBox;
        private readonly VisualizationData visData = new VisualizationData();
        private bool DrawVisualControls = true;

        public TestForm(GraphicsDeviceManager gdm) : base(gdm, "TestForm")
        {
            MyButton = new Button(new Rectangle(100, 100, 200, 50), Game1.FamouseOnePixel, Game1.InformationFont,
                "Play/Pause");
            MyButton.MousePressed += MyButton_MousePressed;
            MyLabel = new Label(new Rectangle(100, 200, 200, 50), Game1.GhostPixel, Game1.InformationFont, "MyLabel");
            MyListBox = new ListBox(new Rectangle(100, 300, 200, 500), Game1.GhostPixel,
                new[] {"MyListBox_1", "MyListBox_2", "MyListBox_3", "MyListBox_4", "MyListBox_5", "MyListBox_6"}.ToList(),
                Game1.GhostPixel, Game1.InformationFont);

            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
            Game1.FreeBeer.IsMouseVisible = true;
        }

        private void MyButton_MousePressed(object sender, EventArgs e)
        {
            //if (MediaPlayerManager.IsPlaying)
            //{
            //    MediaPlayerManager.Pause();
            //}
            //else
            //{
            //    MediaPlayerManager.Resume();
            //}

            if (RealTimeRecording.IsRecording)
            {
                RealTimeRecording.StopRecording();
            }
            else
            {
                RealTimeRecording.StartRecording();
            }
        }

        public override void LoadedUp()
        {
            base.LoadedUp();
            Console.WriteLine("Loaded me!");
            //MediaPlayerManager.Play();
            RealTimeRecording.StartRecording();
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            //sB.Draw(_3DCirclularWaveRenderer.Target(GDM.GraphicsDevice, gameTime, _cam), Game1.VIRTUAL_RESOLUTION, new Color(100, 100, 100, 100));
            //sB.Draw(temp, new Rectangle(Game1.VIRTUAL_RESOLUTION.Width/2,0, Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height), new Color(0,0,255,100));
            _2DFrequencyRenderer.PunctionalDrawing = false;
            var temp = _2DFrequencyRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            _2DFrequencyRenderer.PunctionalDrawing = true;
            sB.Draw(temp, Game1.VIRTUAL_RESOLUTION, Color.White);
            //sB.Draw(temp,Game1.VIRTUAL_RESOLUTION,temp.Bounds,Color.White,0f,Vector2.Zero, SpriteEffects.FlipVertically, 0f);
            //var sampleTexture = ((RenderTarget2D)_2DSampleRenderer.Target(GDM.GraphicsDevice, gameTime, _cam));
            //sB.Draw(sampleTexture, new Rectangle(0,(int)-((1/3f-1/2f) * Game1.VIRTUAL_RESOLUTION.Height), Game1.VIRTUAL_RESOLUTION.Width,Game1.VIRTUAL_RESOLUTION.Height), Color.White);
            //sB.Draw(sampleTexture, new Rectangle(0, (int)+((1 / 3f - 1 / 2f)*Game1.VIRTUAL_RESOLUTION.Height), Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height), Color.White);
            _2DSampleRenderer.PunctionalDrawing = true;
            var sampleTexturePun = _2DSampleRenderer.Target(GDM.GraphicsDevice, gameTime, _cam);
            sB.Draw(sampleTexturePun, Vector2.Zero, Color.White);
            sB.Draw(sampleTexturePun, new Vector2(0, -200), Color.White);
            sB.Draw(sampleTexturePun, new Vector2(0, 200), Color.White);

            if (DrawVisualControls)
            {
                MyButton.Draw(gameTime, sB, 2);
                MyLabel.Draw(gameTime, sB);
                MyListBox.Draw(gameTime, sB, 2);
            }

            GDM.GraphicsDevice.Clear(Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            //MediaPlayerManager.Update();

            if (Keys.N.KeyWasClicked())
            {
                //MediaPlayerManager.Next();
            }

            if (Keys.P.KeyWasClicked())
            {
                //MediaPlayerManager.Previous();
            }

            if (Keys.I.KeyWasClicked())
            {
                DrawVisualControls = !DrawVisualControls;
                Game1.FreeBeer.IsMouseVisible = DrawVisualControls;
            }

            //if (MediaPlayerManager.IsPlaying)
            //{
            //    MediaPlayer.GetVisualizationData(visData);
            //    Game1.AudioAnalysis.Update();
            //    Game1.AudioAnalysis.updateAverageLowFrequencyData();
            //    Game1.AudioAnalysis.updateAverageMidFrequencyData();
            //    Game1.AudioAnalysis.updateAverageHighFrequencyData();
            //    _2DSampleRenderer.UpdateRenderer(visData.Samples);
            //    _2DFrequencyRenderer.UpdateRenderer(visData.Frequencies);
            //    //_3DCirclularWaveRenderer.UpdateRenderer(visData.Frequencies);
            //}

            if (RealTimeRecording.IsRecording)
            {
                _2DFrequencyRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.FrequencySpectrum));
                _2DSampleRenderer.UpdateRenderer(new ReadOnlyCollection<float>(RealTimeRecording.CurrentSamples));
            }

            if (!DrawVisualControls) return;
            MyButton.Update(gameTime);
            MyLabel.Update(gameTime);
            MyListBox.Update(gameTime);
        }
    }
}