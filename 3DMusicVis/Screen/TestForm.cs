#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: TestForm.cs
// Date - created:2016.12.10 - 09:43
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis.Manager;
using _3DMusicVis.RecordingType;
using _3DMusicVis.RenderFrame;
using _3DMusicVis.Shader;
using _3DMusicVis.VisualControls;
using Console = System.Console;

#endregion

namespace _3DMusicVis.Screen
{
    internal class TestForm : Screen
    {
        private readonly Camera _cam;
        private readonly Button MyButton;
        private readonly Label MyLabel;
        private readonly ListBox MyListBox;
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
                RealTimeRecording.StopRecording();
            else
                RealTimeRecording.StartRecording();
        }

        public override void LoadedUp()
        {
            base.LoadedUp();
            Console.WriteLine("Loaded me!");
            Game1.FreeBeer.IsMouseVisible = true;
            //MediaPlayerManager.Play();
            RealTimeRecording.StartRecording();
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            ////sB.Draw(_3DCirclularWaveRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam), Game1.VIRTUAL_RESOLUTION, new Color(100, 100, 100, 100));
            ////sB.Draw(temp, new Rectangle(Game1.VIRTUAL_RESOLUTION.Width/2,0, Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height), new Color(0,0,255,100));
            //_2DFrequencyRenderer.Dashed = false;
            //var temp = _2DFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam);
            //_2DFrequencyRenderer.Dashed = true;
            //sB.Draw(temp, Vector2.Zero, Color.White);
            ////sB.Draw(temp,Game1.VIRTUAL_RESOLUTION,temp.Bounds,Color.White,0f,Vector2.Zero, SpriteEffects.FlipVertically, 0f);
            ////var sampleTexture = ((RenderTarget2D)_2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam));
            ////sB.Draw(sampleTexture, new Rectangle(0,(int)-((1/3f-1/2f) * Game1.VIRTUAL_RESOLUTION.Height), Game1.VIRTUAL_RESOLUTION.Width,Game1.VIRTUAL_RESOLUTION.Height), Color.White);
            ////sB.Draw(sampleTexture, new Rectangle(0, (int)+((1 / 3f - 1 / 2f)*Game1.VIRTUAL_RESOLUTION.Height), Game1.VIRTUAL_RESOLUTION.Width, Game1.VIRTUAL_RESOLUTION.Height), Color.White);
            //_2DSampleRenderer.Dashed = true;
            //var sampleTexturePun = _2DSampleRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam);
            //sB.Draw(sampleTexturePun, Vector2.Zero, Color.White);
            //sB.Draw(sampleTexturePun, new Vector2(0, -200), Color.White);
            //sB.Draw(sampleTexturePun, new Vector2(0, 200), Color.White);

            //if (DrawVisualControls)
            //{
            //    MyButton.Draw(gameTime, sB, 2);
            //    MyLabel.Draw(gameTime, sB);
            //    MyListBox.Draw(gameTime, sB, 2);
            //}

            //GDM.GraphicsDevice.Clear(Color.Black);

            //var dashedFrequ = _3DLinearFrequencyRenderer.Draw(GDM.GraphicsDevice, gameTime, _cam);
            //sB.Begin();
            //sB.Draw(dashedFrequ, Game1.VIRTUAL_RESOLUTION, Color.White);
            //sB.End();

            BloomManager.Bloom.BeginDraw();
            sB.GraphicsDevice.Clear(Color.Transparent);
            // Applying shader
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            sB.Draw(Game1.ViraLogo, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();
            BloomManager.Bloom.EndDraw();

            var temp = GaussianBlurManager.Compute(BloomManager.Bloom.FinalRenderTarget, sB);
            var _alphaDeletionRendertarget = new RenderTarget2D(GDM.GraphicsDevice,
                ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height);

            GDM.GraphicsDevice.SetRenderTarget(_alphaDeletionRendertarget);
            sB.GraphicsDevice.Clear(Color.Transparent);
            sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, Game1.LiquifyEffect);
            Game1.LiquifyEffect.Parameters["width"].SetValue(.5f);
            sB.Draw(temp, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();

            sB.GraphicsDevice.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
            sB.GraphicsDevice.Clear(Color.White);
            sB.Begin();
            sB.Draw(_alphaDeletionRendertarget, ResolutionManager.VIRTUAL_RESOLUTION, Color.White);
            sB.End();
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