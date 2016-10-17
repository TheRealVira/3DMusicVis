#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: OldScreen.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.10.17 - 20:43

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using _3DMusicVis2.TileHelper;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

#endregion

namespace _3DMusicVis2.Screen
{
    internal class OldScreen : Screen
    {
        private readonly ColorDialog _chooseColor;
        private readonly FolderBrowserDialog _chooseDirectory;
        private readonly TileField MyField;
        private readonly Texture2D OnePixelTexture;

        private readonly VisualizationData visData = new VisualizationData();
        private Camera _cam;
        private bool Automated; // Some kind of disco move you could say...

        private int CircleWaveZoom;

        private Color ClearColor;

        /// <summary>
        ///     The color mode:
        ///     0: Normal
        ///     1: Rainbow
        ///     2: Random
        ///     (3: Disco mode)
        /// </summary>
        private byte ColorGenerater;

        private Color edgeColor;
        private Color FadeOutColor = Color.Black;

        private bool flatModeInCircle;
        private bool flatModeInWave;
        private float heightMultiplier = 1.5f;
        private bool Information = true;

        private float InformationOffset;
        private bool IsNowSelectingAColor;
        private bool IsNowSelectingADirectory;
        private bool IsPlaying;
        private bool IsStopped; //Dont get confused by the name! It's for the STOPPING not for PAUSING!!!!!!
        private bool lockMovement = true;
        private ColorMode myColorMode;
        private bool MyIsFullScreen;

        private RasterizerState MyRastState;

        private float orbitSpeed = 0.001f;
        private float PauseInformationFloatCounter;
        private bool PauseInformationFromFlowingIn = true;
        private float RainbowPointer;
        private string SongDirectory;
        private SpecialSong[] Songs;
        private float splashcounter;

        private bool TogglePauseWhenSelectingColor;
        private bool TopMost;

        private bool wave_2D;
        private bool wave_3D = true;

        private Color waveColor;

        private int WaveWaveZoom;

        public OldScreen(GraphicsDeviceManager gdm) : base(gdm, "OldScreen")
        {
            MyField = new TileField(gdm.GraphicsDevice, Vector3.Zero, 20, Game1.INITAIL_HEIGHT, 20, Game1.FIELD_WIDTH,
                Game1.FIELD_WIDTH,
                Color.Black);

            gdm.GraphicsDevice.BlendState = BlendState.Opaque;
            gdm.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            gdm.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            MediaPlayer.IsVisualizationEnabled = true;
            MediaPlayer.Volume = 0.4f;
            //Game1.AudioAnalysis = new AudioAnalysisXNAClass();

            ClearColor = Color.Black;

            //this._basicEffect.LightingEnabled = true;
            //this._basicEffect.EnableDefaultLighting();

            Game1.FreeBeer.IsMouseVisible = true;
            _chooseColor = new ColorDialog();
            _chooseDirectory = new FolderBrowserDialog();
            _chooseDirectory.RootFolder = Environment.SpecialFolder.Desktop;
            waveColor = Color.Red;
            WaveWaveZoomProb = 86;
            CircleWaveZoomProb = 70;
            ModeProb = 2;
            MyRastState = GDM.GraphicsDevice.RasterizerState;

            OnePixelTexture = new Texture2D(gdm.GraphicsDevice, 1, 1);
            OnePixelTexture.SetData(new[] {Color.White});

            Songs = new SpecialSong[0];

            _cam = new Camera(gdm.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);

            if (!Directory.Exists("3DMusicVis2"))
            {
                Directory.CreateDirectory("3DMusicVis2");
            }

            if (!File.Exists(@"3DMusicVis2\Keybindings.txt"))
            {
                using (var sw = new StreamWriter(@"3DMusicVis2\Keybindings.txt"))
                {
                    sw.WriteLine("Vira's 3dMusicVis2 KEYBINDINGS|");
                    sw.WriteLine("------------------------------|");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine("- Movementcontrols:");
                    sw.WriteLine();
                    sw.WriteLine("  (Mouse -> Looking around");
                    sw.WriteLine("  [Spaaaace] Flying upwards");
                    sw.WriteLine("  [Left Control] Flying downwards");
                    sw.WriteLine("  'W' Walking forward");
                    sw.WriteLine("  'A' -||- to the left");
                    sw.WriteLine("  'S' -||- back");
                    sw.WriteLine("  'D' -||- to the right");
                    sw.WriteLine("  'O' Orbit the camera (works only in locked mode)");
                    sw.WriteLine("  'L' Lock movement (includes mouse)");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine("- Soundcontrols:");
                    sw.WriteLine();
                    sw.WriteLine("  [Right Arrow Key] Plays next song");
                    sw.WriteLine("  [Left Arror Key] Plays last song");
                    sw.WriteLine("  '+' Volume up");
                    sw.WriteLine("  '-' Volume down");
                    sw.WriteLine("  'P' Pauses/Resumes the song");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine("- Opticalcontrols:");
                    sw.WriteLine();
                    sw.WriteLine("  [NumPad0] Toggle FlatMode");
                    sw.WriteLine("  [NumPad1] Toggle 'TopMost'");
                    sw.WriteLine("  [NumPad2] Lower height of waves");
                    sw.WriteLine("  [NumPad3] Speedup orbit");
                    sw.WriteLine("  [NumPad4] Lower the orbit speed");
                    sw.WriteLine("  [NumPad7] Toggle 'Pauses when selecting color'");
                    sw.WriteLine("  [NumPad8] Heigher height of waves");
                    sw.WriteLine("  [NumPad9] Reset Camera");
                    sw.WriteLine("  [RightShift] Select folder");
                    sw.WriteLine("  [Back] Select folder");
                    sw.WriteLine("  [Tab] Change 'WaveMode'");
                    sw.WriteLine("  [Alt] + [Enter] Toggle Fullscreen");
                    sw.WriteLine("  'B' Choose boreground color");
                    sw.WriteLine("  'C' Switch Colormode");
                    sw.WriteLine("  'E' Choose edge color");
                    sw.WriteLine("  'F' Choose 'FadeIn' (wave) color");
                    sw.WriteLine("  'H' Choose 'FadeOut' (wave) color");
                    sw.WriteLine("  'I' Toggle Information");
                    sw.WriteLine("  'M' Toggle Wireframemode");
                    sw.WriteLine("  'N' Negate orbit direction");
                    sw.WriteLine("  'R' Toggle RainbowMode");
                    sw.WriteLine("  'T' Toggle RandomMode");
                    sw.WriteLine("  'V' Toggle AutoMode (Disco moves)");
                    sw.WriteLine("  'X' Toggle 3D-Wave");
                    sw.WriteLine("  'Y' Toggle 2D-Wave");
                    sw.WriteLine("  'Z' + 'I' Zoom Waves in");
                    sw.WriteLine("  'Z' + 'O' Zoom Waves out");
                    sw.Close();
                    sw.Dispose();
                }
            }

            if (Directory.Exists(@"3DMusicVis2\Music"))
            {
                SongDirectory = @"3DMusicVis2\Music";

                ReloadSongs();

                if (Songs.Length != 0)
                {
                    SongPointerProb = 1;
                    MediaPlayer.Play(Songs[SongPointerProb - 1].MySong);
                    MediaPlayer.Pause();
                    IsPlaying = false;
                    IsStopped = false;
                }
            }
            else
            {
                Directory.CreateDirectory(@"3DMusicVis2\Music");

                if (!File.Exists(@"3DMusicVis2\Music\ReadMe.txt"))
                {
                    using (var sw = new StreamWriter(@"3DMusicVis2\Music\ReadMe.txt"))
                    {
                        sw.WriteLine("Vira's 3dMusicVis2|");
                        sw.WriteLine("------------------------------|");
                        sw.WriteLine(
                            "Put your music files in here! (Only *.mp3, *.wav, *.aif (*.aiff) and *.wma files will be accepted and played with the 3dMusicVis2!)");

                        sw.Close();
                        sw.Dispose();
                    }
                }
            }

            /*if (this.Songs.Count == 0)
            {
                this.Exit();
            }*/
        }

        /// <summary>
        ///     Gets or sets the mode prob.
        ///     1 = Circle
        ///     2 = Linear
        /// </summary>
        /// <value>
        ///     The mode prob.
        /// </value>
        public int ModeProb
        {
            get { return Mode; }
            set { Mode = 2 - value%2; }
        }

        public int Mode { get; private set; }

        public float RainbowPointerProb
        {
            get { return RainbowPointer; }
            set { RainbowPointer = value*100%100/100; }
        }

        public int WaveWaveZoomProb
        {
            get { return WaveWaveZoom; }
            set { WaveWaveZoom = (int) MathHelper.Clamp(value, 86, 255); }
        }

        public int CircleWaveZoomProb
        {
            get { return CircleWaveZoom; }
            set { CircleWaveZoom = (int) MathHelper.Clamp(value, 70, 255); }
        }

        public int SongPointerProb
        {
            get { return SongPointer; }
            set
            {
                if (Songs.Length > 0)
                {
                    value = Math.Abs(value);
                    SongPointer = (value %= Songs.Length) == 0 ? Songs.Length : value;
                }
            }
        }

        public int SongPointer { get; private set; }

        private void ReloadSongs()
        {
            MediaPlayer.Stop();
            SongPointerProb = 1;
            var temp = new List<SpecialSong>();

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.wma")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GDM.GraphicsDevice));

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.aif")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GDM.GraphicsDevice));

            /*foreach (var item in Directory.GetFiles(@"3DMusicVis2\Music", "*.m4p"))
            {
                //this.Songs.Add(Song.FromUri(item, new Uri(item, UriKind.RelativeOrAbsolute)));
                string songName = item.Substring(18, item.Length - 22);
                this.Songs.Add(new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GraphicsDevice));
            }*/

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.mp3")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GDM.GraphicsDevice));

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.wav")
                let songName = item.Substring(18, item.Length - 22)
                select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GDM.GraphicsDevice));

            Songs = temp.OrderBy(x => x.MySong.Name).ToArray();
            if (Songs.Length > 0)
            {
                MediaPlayer.Play(Songs[SongPointerProb].MySong);
                SongPointerProb++;
                IsPlaying = true;
                IsStopped = false;
            }
        }

        private void DrawInCenter(SpriteBatch spriteBatch, Rectangle rectangle, Texture2D texture, Vector2 leftTop,
            float transparancy = 1f)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int) leftTop.X + GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width/2 - rectangle.Width/2,
                    (int) leftTop.Y + GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Height/2 - rectangle.Height/2,
                    rectangle.Width,
                    rectangle.Height),
                Color.White*transparancy);
        }

        public override void Draw(SpriteBatch sB, GameTime gameTime)
        {
            sB.End();

            if (Songs.Length > 0)
            {
                //GraphicsDevice.Clear(this.waveColor.DarkenColor(225));
                GDM.GraphicsDevice.Clear(ClearColor);
                GDM.GraphicsDevice.RasterizerState = MyRastState;

                GDM.GraphicsDevice.BlendState = BlendState.Opaque;
                GDM.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GDM.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                if (!IsStopped && wave_3D)
                {
                    Game1.BasicEffect.Projection = _cam.Projektion;
                    Game1.BasicEffect.View = _cam.View;

                    MyField.Draw(Game1.BasicEffect, GDM.GraphicsDevice);
                }

                sB.Begin();
                if (wave_2D)
                {
                    var drawColor = Color.Black;
                    if (ColorGenerater == 0)
                    {
                        drawColor = waveColor;
                    }
                    else if (ColorGenerater == 1)
                    {
                        drawColor = GetRainbowColor(RainbowPointerProb);
                    }
                    else if (ColorGenerater == 2)
                    {
                        drawColor = GetRandomColor();
                    }

                    int x, y, width, height;

                    for (var s = 0; s < visData.Samples.Count; s++)
                    {
                        x = GDM.GraphicsDevice.Viewport.Width*s/visData.Samples.Count;
                        width = 8;
                        y =
                            (int)
                                (GDM.GraphicsDevice.Viewport.Height/3f -
                                 visData.Samples[s]*GDM.GraphicsDevice.Viewport.Height/4);
                        height =
                            (int)
                                ((visData.Samples[s] > 0.0f ? 1 : -1f)*visData.Samples[s]*
                                 GDM.GraphicsDevice.Viewport.Height/4f);

                        sB.Draw(OnePixelTexture, new Rectangle(x, y, width, height),
                            drawColor);
                    }

                    for (var f = 0; f < visData.Frequencies.Count; f++)
                    {
                        x = GDM.GraphicsDevice.Viewport.Width*f/visData.Frequencies.Count;
                        y =
                            (int)
                                (GDM.GraphicsDevice.Viewport.Height -
                                 visData.Frequencies[f]*GDM.GraphicsDevice.Viewport.Height/2);
                        width = 1;
                        height = (int) (visData.Frequencies[f]*GDM.GraphicsDevice.Viewport.Height/2);
                        sB.Draw(OnePixelTexture, new Rectangle(x + width*2, y, width, height),
                            drawColor.Negate());
                        sB.Draw(OnePixelTexture, new Rectangle(x, y, width*2, height), drawColor);
                    }
                }

                if (Information)
                {
                    if (!IsPlaying)
                    {
                        sB.Draw(OnePixelTexture, new Rectangle(252, 52, 104, 50), Color.White);
                        sB.Draw(OnePixelTexture, new Rectangle(254, 54, 100, 46), Color.Black);
                        sB.DrawString(Game1.InformationFont, "Paused", new Vector2(261, 63), Color.Red);
                    }

                    if (lockMovement)
                    {
                        sB.Draw(OnePixelTexture, new Rectangle(252, 100, 104, 50), Color.White);
                        sB.Draw(OnePixelTexture, new Rectangle(254, 102, 100, 46), Color.Black);
                        sB.DrawString(Game1.InformationFont, "Locked", new Vector2(261, 111), Color.Red);
                    }

                    if (TopMost)
                    {
                        sB.Draw(OnePixelTexture, new Rectangle(252, 148, 130, 50), Color.White);
                        sB.Draw(OnePixelTexture, new Rectangle(254, 150, 126, 46), Color.Black);
                        sB.DrawString(Game1.InformationFont, "Top most", new Vector2(261, 156), Color.Red);
                    }

                    sB.Draw(OnePixelTexture,
                        new Rectangle(0, 0, GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width, 54),
                        Color.White);
                    sB.Draw(OnePixelTexture,
                        new Rectangle(2, 2, GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width - 4, 50),
                        Color.Black);

                    if (PauseInformationFromFlowingIn)
                    {
                        sB.DrawString(Game1.InformationFont,
                            "  Song:  " + Songs[SongPointerProb - 1].TagLibFile.Tag.Title + " || Album:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Tag.Album + " || Artist:  " +
                            Songs[SongPointerProb - 1].MySong.Artist.Name + " || Duration:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Properties.Duration + " || Genre:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre, new Vector2(255, 20), Color.White);
                    }
                    else
                    {
                        sB.DrawString(Game1.InformationFont,
                            "  Song:  " + Songs[SongPointerProb - 1].TagLibFile.Tag.Title + " || Album:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Tag.Album + " || Artist:  " +
                            Songs[SongPointerProb - 1].MySong.Artist.Name + " || Duration:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Properties.Duration + " || Genre:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre + " || " + "  Song:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Tag.Title + " || Album:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Tag.Album + " || Artist:  " +
                            Songs[SongPointerProb - 1].MySong.Artist.Name + " || Duration:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Properties.Duration + " || Genre:  " +
                            Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre,
                            new Vector2(255 - InformationOffset, 20), Color.White);
                    }

                    sB.Draw(OnePixelTexture, new Rectangle(0, 0, 258, 258), Color.White);
                    sB.Draw(OnePixelTexture, new Rectangle(2, 2, 254, 254), Color.Black);

                    if (Songs[SongPointerProb - 1].TagLibFile.Tag.Pictures.Length >= 1)
                    {
                        sB.Draw(Songs[SongPointerProb - 1].Thumbnail, new Rectangle(4, 4, 250, 250),
                            Color.White);
                    }
                    else
                    {
                        sB.DrawString(Game1.InformationFont,
                            "There is \nno thumbnail\nto display here!\n                ", new Vector2(20, 20),
                            Color.Red);
                    }
                }

                sB.End();
                GDM.GraphicsDevice.RasterizerState = MyRastState;
            }
            else
            {
                if (Information)
                {
                    sB.Begin();

                    if (!IsPlaying)
                    {
                        sB.Draw(OnePixelTexture, new Rectangle(252, 52, 104, 50), Color.White);
                        sB.Draw(OnePixelTexture, new Rectangle(254, 54, 100, 46), Color.Black);
                        sB.DrawString(Game1.InformationFont, "Paused", new Vector2(261, 63), Color.Red);
                    }

                    if (lockMovement)
                    {
                        sB.Draw(OnePixelTexture, new Rectangle(252, 100, 104, 50), Color.White);
                        sB.Draw(OnePixelTexture, new Rectangle(254, 102, 100, 46), Color.Black);
                        sB.DrawString(Game1.InformationFont, "Locked", new Vector2(261, 111), Color.Red);
                    }

                    if (TopMost)
                    {
                        sB.Draw(OnePixelTexture, new Rectangle(252, 148, 130, 50), Color.White);
                        sB.Draw(OnePixelTexture, new Rectangle(254, 150, 126, 46), Color.Black);
                        sB.DrawString(Game1.InformationFont, "Top most", new Vector2(261, 156), Color.Red);
                    }

                    sB.Draw(OnePixelTexture,
                        new Rectangle(0, 0, GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width, 54), Color.White);
                    sB.Draw(OnePixelTexture,
                        new Rectangle(2, 2, GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width - 4, 50),
                        Color.Black);

                    sB.Draw(OnePixelTexture, new Rectangle(0, 0, 258, 258), Color.White);
                    sB.Draw(OnePixelTexture, new Rectangle(2, 2, 254, 254), Color.Black);
                    sB.DrawString(Game1.InformationFont,
                        "There are \nno music file\nin your folder!\n                ", new Vector2(20, 20), Color.Red);

                    sB.End();
                }
            }

            sB.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            if (Songs.Length > 0)
            {
                //if (!this.ShouldStayFocused)
                //{
                //    if (!this.IsActive)
                //    {
                //        if (MediaPlayer.State == MediaState.IsPlaying)
                //        {
                //            MediaPlayer.Pause();
                //        }
                //        return;
                //    }
                //    else
                //    {
                //        if (!this.splash && MediaPlayer.State == MediaState.Paused)
                //        {
                //            MediaPlayer.Resume();
                //        }
                //    }
                //}
                //else
                //{
                //    if (!this.IsActive)
                //    {
                //        var window = System.Windows.Forms.Form.FromHandle(this.Window.Handle) as Form;
                //        window.Activate();
                //    }
                //}

                IsStopped = MediaPlayer.State == MediaState.Stopped;

                if (Game1.NewKeyboardState.IsKeyDown(Keys.RightAlt) && Game1.NewKeyboardState.IsKeyUp(Keys.Enter) &&
                    Game1.OldKeyboardState.IsKeyDown(Keys.Enter))
                {
                    MyIsFullScreen = !MyIsFullScreen;

                    var window = Control.FromHandle(Game1.FreeBeer.Window.Handle) as Form;
                    var formPosition = new Point(window.Location.X, window.Location.Y);
                    var dispayXMulitplikator =
                        (int) Math.Round(formPosition.X/(decimal) GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
                    if (dispayXMulitplikator == 0)
                    {
                        if (formPosition.X < -100)
                        {
                            dispayXMulitplikator = -1;
                        }
                        else
                        {
                            dispayXMulitplikator = 1;
                        }
                    }
                    var displayXMultiplikatorForLocation = dispayXMulitplikator;
                    //int dispayYMulitplikator = this.GraphicsDevice.Adapter.CurrentDisplayMode.Height / formPosition.Y;

                    if (displayXMultiplikatorForLocation > 0)
                    {
                        displayXMultiplikatorForLocation--;
                    }

                    if (MyIsFullScreen)
                    {
                        window.FormBorderStyle = FormBorderStyle.None;
                        window.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        window.FormBorderStyle = FormBorderStyle.FixedSingle;
                        window.WindowState = FormWindowState.Normal;
                        window.Location =
                            new System.Drawing.Point(
                                GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width*displayXMultiplikatorForLocation,
                                0);
                        window.Size = new Size(GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                            GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
                    }
                }

                if (Game1.NewKeyboardState.IsKeyUp(Keys.Tab) && Game1.OldKeyboardState.IsKeyDown(Keys.Tab))
                {
                    ModeProb++;
                    //MyField = new TileField(Vector3.Zero, 20, 2, 20, FIELD_WIDTH, FIELD_WIDTH, edgeColor);
                    MyField.ResetHeight(Game1.INITAIL_HEIGHT);
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.I) && Game1.OldKeyboardState.IsKeyDown(Keys.I))
                {
                    Information = !Information;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.Y) && Game1.OldKeyboardState.IsKeyDown(Keys.Y))
                {
                    wave_2D = !wave_2D;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.X) && Game1.OldKeyboardState.IsKeyDown(Keys.X))
                {
                    wave_3D = !wave_3D;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.V) && Game1.OldKeyboardState.IsKeyDown(Keys.V))
                {
                    Automated = !Automated;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.N) && Game1.OldKeyboardState.IsKeyDown(Keys.N))
                {
                    _cam.NegateOrbit = !_cam.NegateOrbit;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.OemPlus) && Game1.OldKeyboardState.IsKeyDown(Keys.OemPlus))
                {
                    MediaPlayer.Volume += 0.02f;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.OemMinus) && Game1.OldKeyboardState.IsKeyDown(Keys.OemMinus))
                {
                    MediaPlayer.Volume -= 0.02f;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad2) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad2))
                {
                    heightMultiplier -= 0.2f;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad8) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad8))
                {
                    heightMultiplier += 0.2f;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad3) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad3))
                {
                    orbitSpeed *= Math.Abs(orbitSpeed - 1) < 0.00001f ? 1f : 10f;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad4) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad4))
                {
                    orbitSpeed *= Math.Abs(orbitSpeed - 0.00001f) < 0.00001f ? 1f : .1f;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad7) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad7))
                {
                    TogglePauseWhenSelectingColor = !TogglePauseWhenSelectingColor;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad1) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad1))
                {
                    var window = Control.FromHandle(Game1.FreeBeer.Window.Handle) as Form;
                    window.TopMost = !window.TopMost;
                    TopMost = window.TopMost;
                    Game1.FreeBeer.IsMouseVisible = !TopMost;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.R) && Game1.OldKeyboardState.IsKeyDown(Keys.R))
                {
                    ColorGenerater = ColorGenerater == 1 ? (byte) 0 : (byte) 1;
                    //RandomMode = false;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.T) && Game1.OldKeyboardState.IsKeyDown(Keys.T))
                {
                    ColorGenerater = ColorGenerater == 2 ? (byte) 0 : (byte) 2;
                    //RainbowMode = false;
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.C) && Game1.OldKeyboardState.IsKeyDown(Keys.C))
                {
                    switch (myColorMode)
                    {
                        case ColorMode.OnlyCenter:
                            myColorMode = ColorMode.SideEqualsCenter;
                            MyField.FillEdgeColors(edgeColor);
                            break;

                        case ColorMode.SideEqualsCenter:
                            myColorMode = ColorMode.DynamicSideColorShiat;
                            break;

                        case ColorMode.DynamicSideColorShiat:
                            myColorMode = ColorMode.OnlyCenter;
                            break;
                    }

                    if (myColorMode == ColorMode.OnlyCenter)
                    {
                        MyField.FillEdgeColors(edgeColor);
                    }
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.M) && Game1.OldKeyboardState.IsKeyDown(Keys.M))
                {
                    MyRastState = new RasterizerState
                    {
                        FillMode =
                            GDM.GraphicsDevice.RasterizerState.FillMode == FillMode.Solid
                                ? FillMode.WireFrame
                                : FillMode.Solid,
                        CullMode = CullMode.CullCounterClockwiseFace
                    };
                }
                if (Game1.NewKeyboardState.IsKeyDown(Keys.Z))
                {
                    if (Game1.NewKeyboardState.IsKeyUp(Keys.I) && Game1.OldKeyboardState.IsKeyDown(Keys.I))
                    {
                        if (ModeProb == 1)
                        {
                            WaveWaveZoomProb++;
                        }
                        else if (ModeProb == 2)
                        {
                            CircleWaveZoomProb++;
                        }
                    }
                    if (Game1.NewKeyboardState.IsKeyUp(Keys.O) && Game1.OldKeyboardState.IsKeyDown(Keys.O))
                    {
                        if (ModeProb == 1)
                        {
                            WaveWaveZoomProb--;
                        }
                        else if (ModeProb == 2)
                        {
                            CircleWaveZoomProb--;
                        }
                    }
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad0) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad0))
                {
                    //MyField = new TileField(Vector3.Zero, 20, 2, 20, 100, 100, edgeColor);
                    MyField.ResetHeight(Game1.INITAIL_HEIGHT);
                    if (ModeProb == 1)
                    {
                        flatModeInWave = !flatModeInWave;
                    }
                    else if (ModeProb == 2)
                    {
                        flatModeInCircle = !flatModeInCircle;
                    }
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.RightShift) && Game1.OldKeyboardState.IsKeyDown(Keys.RightShift))
                {
                    ReloadSongs();
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.NumPad9) && Game1.OldKeyboardState.IsKeyDown(Keys.NumPad9))
                {
                    _cam = new Camera(GDM.GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.Left) && Game1.OldKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (!IsNowSelectingAColor)
                    {
                        MediaPlayer.Stop();
                        IsStopped = true;
                        SongPointerProb--;
                        MediaPlayer.Play(Songs[SongPointerProb - 1].MySong);
                        IsPlaying = true;
                        IsStopped = false;
                    }
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.Right) && Game1.OldKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (!IsNowSelectingAColor)
                    {
                        MediaPlayer.Stop();
                        IsStopped = true;
                    }
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.P) && Game1.OldKeyboardState.IsKeyDown(Keys.P))
                {
                    if (IsPlaying)
                    {
                        MediaPlayer.Pause();
                        IsPlaying = false;
                    }
                    else if (!IsPlaying)
                    {
                        MediaPlayer.Resume();
                        IsPlaying = true;
                    }
                }
                if (Game1.NewKeyboardState.IsKeyUp(Keys.Back) && Game1.OldKeyboardState.IsKeyDown(Keys.Back))
                {
                    if (!IsNowSelectingADirectory)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            IsNowSelectingADirectory = true;

                            if (_chooseDirectory.ShowDialog() == DialogResult.OK)
                            {
                                SongDirectory = _chooseDirectory.SelectedPath;
                            }

                            ReloadSongs();

                            IsNowSelectingADirectory = false;
                        });
                    }
                }

                if (lockMovement)
                {
                    if (Game1.NewKeyboardState.IsKeyUp(Keys.F) && Game1.OldKeyboardState.IsKeyDown(Keys.F))
                    {
                        if (!IsNowSelectingAColor)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                var paused = !IsPlaying;

                                IsNowSelectingAColor = true;
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Pause();
                                    IsPlaying = false;
                                }
                                if (_chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK)
                                {
                                    waveColor = new Color(_chooseColor.Color.R, _chooseColor.Color.G,
                                        _chooseColor.Color.B,
                                        _chooseColor.Color.A);
                                }
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Resume();
                                    IsPlaying = true;
                                }
                                IsNowSelectingAColor = false;
                                var myForm = (Form) Control.FromHandle(Game1.FreeBeer.Window.Handle);
                                myForm.Activate();
                            });
                        }
                    }
                    if (Game1.NewKeyboardState.IsKeyUp(Keys.H) && Game1.OldKeyboardState.IsKeyDown(Keys.H))
                    {
                        if (!IsNowSelectingAColor)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                var paused = !IsPlaying;

                                IsNowSelectingAColor = true;
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Pause();
                                    IsPlaying = false;
                                }
                                if (_chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK)
                                {
                                    FadeOutColor = new Color(_chooseColor.Color.R, _chooseColor.Color.G,
                                        _chooseColor.Color.B, _chooseColor.Color.A);
                                }
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Resume();
                                    IsPlaying = true;
                                }
                                IsNowSelectingAColor = false;
                                var myForm = (Form) Control.FromHandle(Game1.FreeBeer.Window.Handle);
                                myForm.Activate();
                            });
                        }
                    }
                    if (Game1.NewKeyboardState.IsKeyUp(Keys.B) && Game1.OldKeyboardState.IsKeyDown(Keys.B))
                    {
                        if (!IsNowSelectingAColor)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                var paused = !IsPlaying;

                                IsNowSelectingAColor = true;
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Pause();
                                    IsPlaying = false;
                                }
                                if (_chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK)
                                {
                                    ClearColor = new Color(_chooseColor.Color.R, _chooseColor.Color.G,
                                        _chooseColor.Color.B,
                                        _chooseColor.Color.A);
                                }
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Resume();
                                    IsPlaying = true;
                                }
                                IsNowSelectingAColor = false;
                                var myForm = (Form) Control.FromHandle(Game1.FreeBeer.Window.Handle);
                                myForm.Activate();
                            });
                        }
                    }
                    if (Game1.NewKeyboardState.IsKeyUp(Keys.E) && Game1.OldKeyboardState.IsKeyDown(Keys.E))
                    {
                        if (!IsNowSelectingAColor)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                var paused = !IsPlaying;

                                IsNowSelectingAColor = true;
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Pause();
                                    IsPlaying = false;
                                }
                                if (_chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK &&
                                    edgeColor !=
                                    new Color(_chooseColor.Color.R, _chooseColor.Color.G, _chooseColor.Color.B,
                                        _chooseColor.Color.A))
                                {
                                    edgeColor = new Color(_chooseColor.Color.R, _chooseColor.Color.G,
                                        _chooseColor.Color.B,
                                        _chooseColor.Color.A);
                                    //MyField = new TileField(Vector3.Zero, 20, 2, 20, 100, 100, edgeColor);
                                    MyField.ResetHeight(Game1.INITAIL_HEIGHT);
                                }
                                if (!paused && !TogglePauseWhenSelectingColor)
                                {
                                    MediaPlayer.Resume();
                                    IsPlaying = true;
                                }
                                IsNowSelectingAColor = false;
                                var myForm = (Form) Control.FromHandle(Game1.FreeBeer.Window.Handle);
                                myForm.Activate();
                            });
                        }
                    }
                }

                if (Game1.NewKeyboardState.IsKeyUp(Keys.L) && Game1.OldKeyboardState.IsKeyDown(Keys.L))
                {
                    lockMovement = !lockMovement;
                    Game1.FreeBeer.IsMouseVisible =
                        !(Control.FromHandle(Game1.FreeBeer.Window.Handle) as Form).TopMost && lockMovement;
                }

                if (!IsStopped)
                {
                    if (IsPlaying)
                    {
                        if (wave_3D || wave_2D)
                        {
                            MediaPlayer.GetVisualizationData(visData);
                            Game1.AudioAnalysis.Update();
                            Game1.AudioAnalysis.updateAverageLowFrequencyData();
                            Game1.AudioAnalysis.updateAverageMidFrequencyData();
                            Game1.AudioAnalysis.updateAverageHighFrequencyData();
                        }

                        if (wave_3D)
                        {
                            if (ColorGenerater == 0)
                            {
                                switch (ModeProb)
                                {
                                    case 1:
                                        MyField.Update(visData.Frequencies,
                                            visData.Frequencies.Count/WaveWaveZoomProb, waveColor, ModeProb,
                                            FadeOutColor,
                                            myColorMode, flatModeInWave, heightMultiplier);
                                        break;
                                    case 2:
                                        MyField.Update(visData.Frequencies,
                                            visData.Frequencies.Count/CircleWaveZoomProb, waveColor, ModeProb,
                                            FadeOutColor,
                                            myColorMode, flatModeInCircle, heightMultiplier);
                                        break;
                                }
                            }
                            else if (ColorGenerater == 1)
                            {
                                switch (ModeProb)
                                {
                                    case 1:
                                        MyField.Update(visData.Frequencies,
                                            visData.Frequencies.Count/WaveWaveZoomProb,
                                            GetRainbowColor(RainbowPointerProb),
                                            ModeProb, FadeOutColor, myColorMode, flatModeInWave, heightMultiplier);
                                        break;
                                    case 2:
                                        MyField.Update(visData.Frequencies,
                                            visData.Frequencies.Count/CircleWaveZoomProb,
                                            GetRainbowColor(RainbowPointerProb), ModeProb, FadeOutColor, myColorMode,
                                            flatModeInCircle, heightMultiplier);
                                        break;
                                }
                            }
                            else if (ColorGenerater == 2)
                            {
                                switch (ModeProb)
                                {
                                    case 1:
                                        MyField.Update(visData.Frequencies,
                                            visData.Frequencies.Count/WaveWaveZoomProb, GetRandomColor(), ModeProb,
                                            FadeOutColor, myColorMode, flatModeInWave, heightMultiplier);
                                        break;
                                    case 2:
                                        MyField.Update(visData.Frequencies,
                                            visData.Frequencies.Count/CircleWaveZoomProb, GetRandomColor(), ModeProb,
                                            FadeOutColor, myColorMode, flatModeInCircle, heightMultiplier);
                                        break;
                                }
                            }
                        }

                        if (_cam.Orbit)
                        {
                            if (Automated)
                            {
                                if (Game1.AudioAnalysis.getBool_2StepLowFrq(0.54f))
                                {
                                    _cam.NegateOrbit = !_cam.NegateOrbit;
                                }

                                _cam.Position =
                                    Vector3.Transform(_cam.Position - new Vector3(10, 0, 10),
                                        Matrix.CreateFromAxisAngle(new Vector3(0, _cam.NegateOrbit ? -1 : 1, 0),
                                            Game1.AudioAnalysis.getFloat_2StepLowFrq(orbitSpeed, .1f, 0.45f))) +
                                    new Vector3(10, 0, 10);
                                _cam.View = Matrix.CreateLookAt(_cam.Position, new Vector3(10, 0, 10), Vector3.Up);
                            }
                            else
                            {
                                _cam.Position =
                                    Vector3.Transform(_cam.Position - new Vector3(10, 0, 10),
                                        Matrix.CreateFromAxisAngle(new Vector3(0, _cam.NegateOrbit ? -1 : 1, 0),
                                            orbitSpeed)) +
                                    new Vector3(10, 0, 10);
                                _cam.View = Matrix.CreateLookAt(_cam.Position, new Vector3(10, 0, 10), Vector3.Up);
                            }

                            //Cam.Position = Vector3.Transform(Cam.Position, Matrix.CreateRotationY(Game1.AudioAnalysis.getFloat_2StepLowFrq(orbitSpeed, .1f, 0.45f)));
                            //Cam.Target = new Vector3(10, 0, 10);
                            //Cam.View = Matrix.CreateLookAt(Cam.Position, Cam.Target, Vector3.Up);
                        }
                    }
                }
                else
                {
                    if (Songs.Length == 0)
                    {
                        Game1.FreeBeer.Exit();
                        return;
                    }
                    SongPointerProb++;
                    MediaPlayer.Play(Songs[SongPointerProb - 1].MySong);
                    IsStopped = false;
                    IsPlaying = true;
                }

                if (!lockMovement)
                {
                    _cam.Update(gameTime);
                    Mouse.SetPosition(GDM.GraphicsDevice.Viewport.Width/2,
                        GDM.GraphicsDevice.Viewport.Height/2);
                }
                else
                {
                    if (Game1.NewKeyboardState.IsKeyUp(Keys.O) && Game1.OldKeyboardState.IsKeyDown(Keys.O))
                    {
                        _cam.Position = new Vector3(10, 14.5f, -9.5f);
                        _cam.Rotation = new Vector3(0.65f, 0, 0);
                        _cam.Orbit = !_cam.Orbit;
                    }
                }

                if (ColorGenerater == 1)
                {
                    RainbowPointerProb += (float) gameTime.ElapsedGameTime.TotalMilliseconds/5000f;
                }

                if (Information)
                {
                    if (!PauseInformationFromFlowingIn)
                    {
                        InformationOffset += (float) gameTime.ElapsedGameTime.TotalMilliseconds/10;
                        if (-InformationOffset +
                            Game1.InformationFont.MeasureString("  Song:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Tag.Title +
                                                                " || Album:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Tag.Album +
                                                                " || Artist:  " +
                                                                Songs[SongPointerProb - 1].MySong.Artist.Name +
                                                                " || Duration:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Properties
                                                                    .Duration +
                                                                " || Genre:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre +
                                                                " || " + "  Song:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Tag.Title +
                                                                " || Album:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Tag.Album +
                                                                " || Artist:  " +
                                                                Songs[SongPointerProb - 1].MySong.Artist.Name +
                                                                " || Duration:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Properties
                                                                    .Duration +
                                                                " || Genre:  " +
                                                                Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre).X <
                            0)
                        {
                            InformationOffset = -GDM.GraphicsDevice.Adapter.CurrentDisplayMode.Width + 254;
                            //this.InformationOffset = 0;
                        }
                    }
                    PauseInformationFloatCounter += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (PauseInformationFloatCounter >= Game1.PAUSEINFORMATIONFLOATMAXCOUNTER)
                    {
                        PauseInformationFromFlowingIn = !PauseInformationFromFlowingIn;
                        PauseInformationFloatCounter = 0;
                    }
                }
            }
            else
            {
                if (Game1.NewKeyboardState.IsKeyUp(Keys.U) && Game1.OldKeyboardState.IsKeyDown(Keys.U))
                {
                    if (!IsNowSelectingADirectory)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            IsNowSelectingADirectory = true;

                            if (_chooseDirectory.ShowDialog() == DialogResult.OK)
                            {
                                SongDirectory = _chooseDirectory.SelectedPath;
                            }

                            ReloadSongs();

                            IsNowSelectingADirectory = false;
                        });
                    }
                }

                if (Game1.NewKeyboardState.IsKeyUp(Keys.RightShift) && Game1.OldKeyboardState.IsKeyDown(Keys.RightShift))
                {
                    ReloadSongs();
                }
            }
        }

        private Color GetRainbowColor(float progress)
        {
            var div = Math.Abs(progress%1)*6;
            var ascending = (int) (div%1*255);
            var descending = 255 - ascending;

            switch ((int) div)
            {
                case 0:
                    return new Color(255, 255, ascending, 0);
                case 1:
                    return new Color(255, descending, 255, 0);
                case 2:
                    return new Color(255, 0, 255, ascending);
                case 3:
                    return new Color(255, 0, descending, 255);
                case 4:
                    return new Color(255, ascending, 0, 255);
                default: // case 5:
                    return new Color(255, 255, 0, descending);
            }
        }

        private Color GetRandomColor()
        {
            return new Color(Game1.Rand.Next(0, 256), Game1.Rand.Next(0, 256), Game1.Rand.Next(0, 256));
        }
    }
}