#region License

// Copyright (c) 2015, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Game1.cs
// Date - created: 2015.08.26 - 14:45
// Date - current: 2016.05.08 - 11:01

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

namespace _3DMusicVis2
{
    /// <summary>
    /// 3DMusicVis2
    /// </summary>
    public class Game1 : Game
    {
        private const float SplashMaxCount = 1000;
        private const float PAUSEINFORMATIONFLOATMAXCOUNTER = 20000;
        private const int FIELD_WIDTH = 100;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static MouseState NewMouseState, OldMouseState;
        public static KeyboardState NewKeyboardState, OldKeyboardState;
        public static Random rand;
        private BasicEffect @BasicEffect;
        private Camera Cam;
        private ColorDialog chooseColor;
        private FolderBrowserDialog chooseDirectory;

        private int CircleWaveZoom;

        private Color ClearColor;
        private Color edgeColor;
        private Color FadeOutColor = Color.Black;

        private bool flatModeInCircle;
        private bool flatModeInWave;
        private float heightMultiplier = 1.5f;
        private bool Information = true;
        private SpriteFont InformationFont;

        private float InformationOffset;
        private bool IsNowSelectingAColor;
        private bool IsNowSelectingADirectory;
        private bool IsPlaying;
        private bool IsStopped; //Dont get confused by the name! It's for the STOPPING not for PAUSING!!!!!!
        private bool lockMovement = true;
        private ColorMode myColorMode;
        private TileField MyField = new TileField(Vector3.Zero, 20, 0, 20, FIELD_WIDTH, FIELD_WIDTH, Color.Black);
        private bool MyIsFullScreen;
        private Texture2D OnePixelTexture;

        private float orbitSpeed = 0.001f;
        private float PauseInformationFloatCounter;
        private bool PauseInformationFromFlowingIn = true;
        private bool RainbowMode;
        private float RainbowPointer;
        private bool RandomMode;
        private string SongDirectory;
        private SpecialSong[] Songs;
        private bool splash = true;
        private float splashcounter;

        private Texture2D SplashScreen;
        private bool TogglePauseWhenSelectingColor;
        private bool TopMost;

        private readonly VisualizationData visData = new VisualizationData();
        private AudioAnalysisXNAClass audioAnalysis;

        private Color waveColor;

        private int WaveWaveZoom;

        private bool wave_2D;
        private bool wave_3D = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Gets or sets the mode prob.
        /// 1 = Circle
        /// 2 = Linear
        /// </summary>
        /// <value>
        /// The mode prob.
        /// </value>
        public int ModeProb
        {
            get { return Mode; }
            set { Mode = 2 - (value%2); }
        }

        public int Mode { get; private set; }

        public float RainbowPointerProb
        {
            get { return RainbowPointer; }
            set { RainbowPointer = ((value*100)%100)/100; }
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

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Make render work.

            ModeProb = 2;

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            InactiveSleepTime = new TimeSpan(0);
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60);
            //graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            MediaPlayer.IsVisualizationEnabled = true;
            MediaPlayer.Volume = 0.4f;
            audioAnalysis=new AudioAnalysisXNAClass();

            rand = new Random();

            ClearColor = Color.Black;
            BasicEffect = new BasicEffect(GraphicsDevice);

            //this.BasicEffect.LightingEnabled = true;
            //this.BasicEffect.EnableDefaultLighting();

            IsMouseVisible = true;
            chooseColor = new ColorDialog();
            chooseDirectory = new FolderBrowserDialog();
            chooseDirectory.RootFolder = Environment.SpecialFolder.Desktop;
            waveColor = Color.Red;
            WaveWaveZoomProb = 86;
            CircleWaveZoomProb = 70;

            OnePixelTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            OnePixelTexture.SetData(new[] {Color.White});

            Songs = new SpecialSong[0];

            Cam = new Camera(GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);

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
                    sw.WriteLine("  'R' Toggle RainbowMode");
                    sw.WriteLine("  'T' Toggle RandomMode");
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

            base.Initialize();
        }

        private void ReloadSongs()
        {
            MediaPlayer.Stop();
            SongPointerProb = 1;
            var temp = new List<SpecialSong>();

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.wma") let songName = item.Substring(18, item.Length - 22) select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GraphicsDevice));

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.aif") let songName = item.Substring(18, item.Length - 22) select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GraphicsDevice));

            /*foreach (var item in Directory.GetFiles(@"3DMusicVis2\Music", "*.m4p"))
            {
                //this.Songs.Add(Song.FromUri(item, new Uri(item, UriKind.RelativeOrAbsolute)));
                string songName = item.Substring(18, item.Length - 22);
                this.Songs.Add(new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GraphicsDevice));
            }*/

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.mp3") let songName = item.Substring(18, item.Length - 22) select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GraphicsDevice));

            temp.AddRange(from item in Directory.GetFiles(SongDirectory, "*.wav") let songName = item.Substring(18, item.Length - 22) select new SpecialSong(songName, new Uri(item, UriKind.RelativeOrAbsolute), GraphicsDevice));

            this.Songs = temp.OrderBy(x => x.MySong.Name).ToArray();
            if (Songs.Length > 0)
            {
                MediaPlayer.Play(Songs[SongPointerProb].MySong);
                SongPointerProb++;
                IsPlaying = true;
                IsStopped = false;
            }
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SplashScreen = Content.Load<Texture2D>("Vira");
            InformationFont = Content.Load<SpriteFont>("InformationFont");

            GraphicsDevice.Present();
        }

        private void DrawInCenter(SpriteBatch spriteBatch, Rectangle rectangle, Texture2D texture, Vector2 leftTop,
            float transparancy = 1f)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int) leftTop.X + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width/2 - rectangle.Width/2,
                    (int) leftTop.Y + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height/2 - rectangle.Height/2,
                    rectangle.Width,
                    rectangle.Height),
                Color.White*transparancy);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Songs.Length > 0)
            {
                //if (!this.ShouldStayFocused)
                //{
                //    if (!this.IsActive)
                //    {
                //        if (MediaPlayer.State == MediaState.Playing)
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

                if (!splash)
                {
                    NewKeyboardState = Keyboard.GetState();
                    NewMouseState = Mouse.GetState();

                    if (NewKeyboardState.IsKeyUp(Keys.Escape) && OldKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }

                    if (NewKeyboardState.IsKeyDown(Keys.RightAlt) &&
                        NewKeyboardState.IsKeyUp(Keys.Enter) && OldKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        MyIsFullScreen = !MyIsFullScreen;

                        var window = Control.FromHandle(Window.Handle) as Form;
                        var formPosition = new Point(window.Location.X, window.Location.Y);
                        var dispayXMulitplikator =
                            (int) Math.Round(formPosition.X/(decimal) GraphicsDevice.Adapter.CurrentDisplayMode.Width);
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
                                    (GraphicsDevice.Adapter.CurrentDisplayMode.Width*displayXMultiplikatorForLocation),
                                    0);
                            window.Size = new Size(GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                                GraphicsDevice.Adapter.CurrentDisplayMode.Height);
                        }
                    }

                    if (NewKeyboardState.IsKeyUp(Keys.Tab) && OldKeyboardState.IsKeyDown(Keys.Tab))
                    {
                        ModeProb++;
                        MyField = new TileField(Vector3.Zero, 20, 2, 20, FIELD_WIDTH, FIELD_WIDTH, edgeColor);
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.I) && OldKeyboardState.IsKeyDown(Keys.I))
                    {
                        Information = !Information;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.Y) && OldKeyboardState.IsKeyDown(Keys.Y))
                    {
                        wave_2D = !wave_2D;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.X) && OldKeyboardState.IsKeyDown(Keys.X))
                    {
                        wave_3D = !wave_3D;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.OemPlus) && OldKeyboardState.IsKeyDown(Keys.OemPlus))
                    {
                        MediaPlayer.Volume += 0.02f;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.OemMinus) && OldKeyboardState.IsKeyDown(Keys.OemMinus))
                    {
                        MediaPlayer.Volume -= 0.02f;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad2) && OldKeyboardState.IsKeyDown(Keys.NumPad2))
                    {
                        heightMultiplier -= 0.2f;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad8) && OldKeyboardState.IsKeyDown(Keys.NumPad8))
                    {
                        heightMultiplier += 0.2f;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad3) && OldKeyboardState.IsKeyDown(Keys.NumPad3))
                    {
                        orbitSpeed *= Math.Abs(orbitSpeed - 1) < 0.00001f ? 1f : 10f;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad4) && OldKeyboardState.IsKeyDown(Keys.NumPad4))
                    {
                        orbitSpeed *= Math.Abs(orbitSpeed - 0.00001f) < 0.00001f ? 1f : .1f;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad7) && OldKeyboardState.IsKeyDown(Keys.NumPad7))
                    {
                        TogglePauseWhenSelectingColor = !TogglePauseWhenSelectingColor;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad1) && OldKeyboardState.IsKeyDown(Keys.NumPad1))
                    {
                        var window = Control.FromHandle(Window.Handle) as Form;
                        window.TopMost = !window.TopMost;
                        TopMost = window.TopMost;
                        IsMouseVisible = !TopMost;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.R) && OldKeyboardState.IsKeyDown(Keys.R))
                    {
                        RainbowMode = !RainbowMode;
                        RandomMode = false;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.T) && OldKeyboardState.IsKeyDown(Keys.T))
                    {
                        RandomMode = !RandomMode;
                        RainbowMode = false;
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.C) && OldKeyboardState.IsKeyDown(Keys.C))
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
                    if (NewKeyboardState.IsKeyUp(Keys.M) && OldKeyboardState.IsKeyDown(Keys.M))
                    {
                        GraphicsDevice.RasterizerState = new RasterizerState
                        {
                            FillMode =
                                GraphicsDevice.RasterizerState.FillMode == FillMode.Solid
                                    ? FillMode.WireFrame
                                    : FillMode.Solid,
                            CullMode = CullMode.CullCounterClockwiseFace
                        };
                    }
                    if (NewKeyboardState.IsKeyDown(Keys.Z))
                    {
                        if (NewKeyboardState.IsKeyUp(Keys.I) && OldKeyboardState.IsKeyDown(Keys.I))
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
                        if (NewKeyboardState.IsKeyUp(Keys.O) && OldKeyboardState.IsKeyDown(Keys.O))
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
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad0) && OldKeyboardState.IsKeyDown(Keys.NumPad0))
                    {
                        MyField = new TileField(Vector3.Zero, 20, 2, 20, 100, 100, edgeColor);
                        if (ModeProb == 1)
                        {
                            flatModeInWave = !flatModeInWave;
                        }
                        else if (ModeProb == 2)
                        {
                            flatModeInCircle = !flatModeInCircle;
                        }
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.RightShift) && OldKeyboardState.IsKeyDown(Keys.RightShift))
                    {
                        ReloadSongs();
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.NumPad9) && OldKeyboardState.IsKeyDown(Keys.NumPad9))
                    {
                        Cam = new Camera(GraphicsDevice, new Vector3(10, 14.5f, -9.5f), new Vector3(0.65f, 0, 0), 1.5f);
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.Left) && OldKeyboardState.IsKeyDown(Keys.Left))
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
                    if (NewKeyboardState.IsKeyUp(Keys.Right) && OldKeyboardState.IsKeyDown(Keys.Right))
                    {
                        if (!IsNowSelectingAColor)
                        {
                            MediaPlayer.Stop();
                            IsStopped = true;
                        }
                    }
                    if (NewKeyboardState.IsKeyUp(Keys.P) && OldKeyboardState.IsKeyDown(Keys.P))
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
                    if (NewKeyboardState.IsKeyUp(Keys.Back) && OldKeyboardState.IsKeyDown(Keys.Back))
                    {
                        if (!IsNowSelectingADirectory)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                IsNowSelectingADirectory = true;

                                if (chooseDirectory.ShowDialog() == DialogResult.OK)
                                {
                                    SongDirectory = chooseDirectory.SelectedPath;
                                }

                                ReloadSongs();

                                IsNowSelectingADirectory = false;
                            });
                        }
                    }

                    if (this.lockMovement)
                    {
                        if (NewKeyboardState.IsKeyUp(Keys.F) && OldKeyboardState.IsKeyDown(Keys.F))
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
                                    if (chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK)
                                    {
                                        waveColor = new Color(chooseColor.Color.R, chooseColor.Color.G,
                                            chooseColor.Color.B,
                                            chooseColor.Color.A);
                                    }
                                    if (!paused && !TogglePauseWhenSelectingColor)
                                    {
                                        MediaPlayer.Resume();
                                        IsPlaying = true;
                                    }
                                    IsNowSelectingAColor = false;
                                    var myForm = (Form) Control.FromHandle(Window.Handle);
                                    myForm.Activate();
                                });
                            }
                        }
                        if (NewKeyboardState.IsKeyUp(Keys.H) && OldKeyboardState.IsKeyDown(Keys.H))
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
                                    if (chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK)
                                    {
                                        FadeOutColor = new Color(chooseColor.Color.R, chooseColor.Color.G,
                                            chooseColor.Color.B, chooseColor.Color.A);
                                    }
                                    if (!paused && !TogglePauseWhenSelectingColor)
                                    {
                                        MediaPlayer.Resume();
                                        IsPlaying = true;
                                    }
                                    IsNowSelectingAColor = false;
                                    var myForm = (Form) Control.FromHandle(Window.Handle);
                                    myForm.Activate();
                                });
                            }
                        }
                        if (NewKeyboardState.IsKeyUp(Keys.B) && OldKeyboardState.IsKeyDown(Keys.B))
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
                                    if (chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK)
                                    {
                                        ClearColor = new Color(chooseColor.Color.R, chooseColor.Color.G,
                                            chooseColor.Color.B,
                                            chooseColor.Color.A);
                                    }
                                    if (!paused && !TogglePauseWhenSelectingColor)
                                    {
                                        MediaPlayer.Resume();
                                        IsPlaying = true;
                                    }
                                    IsNowSelectingAColor = false;
                                    var myForm = (Form) Control.FromHandle(Window.Handle);
                                    myForm.Activate();
                                });
                            }
                        }
                        if (NewKeyboardState.IsKeyUp(Keys.E) && OldKeyboardState.IsKeyDown(Keys.E))
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
                                    if (chooseColor.ShowDialog(new Form {TopMost = true}) == DialogResult.OK &&
                                        edgeColor !=
                                        new Color(chooseColor.Color.R, chooseColor.Color.G, chooseColor.Color.B,
                                            chooseColor.Color.A))
                                    {
                                        edgeColor = new Color(chooseColor.Color.R, chooseColor.Color.G,
                                            chooseColor.Color.B,
                                            chooseColor.Color.A);
                                        MyField = new TileField(Vector3.Zero, 20, 2, 20, 100, 100, edgeColor);
                                    }
                                    if (!paused && !TogglePauseWhenSelectingColor)
                                    {
                                        MediaPlayer.Resume();
                                        IsPlaying = true;
                                    }
                                    IsNowSelectingAColor = false;
                                    var myForm = (Form) Control.FromHandle(Window.Handle);
                                    myForm.Activate();
                                });
                            }
                        }
                    }

                    if (NewKeyboardState.IsKeyUp(Keys.L) && OldKeyboardState.IsKeyDown(Keys.L))
                    {
                        lockMovement = !lockMovement;
                        IsMouseVisible = !(Control.FromHandle(Window.Handle) as Form).TopMost&&this.lockMovement;
                    }

                    if (!IsStopped)
                    {
                        if ((wave_3D || wave_2D)&&IsPlaying)
                        {
                            MediaPlayer.GetVisualizationData(visData);
                        }

                        if (IsPlaying&&wave_3D)
                        {
                            //audioAnalysis.Update();
                            //audioAnalysis.updateAverageLowFrequencyData();
                            //audioAnalysis.updateAverageMidFrequencyData();
                            //audioAnalysis.updateAverageHighFrequencyData();
                            //bool shoot = audioAnalysis.getBool_2StepLowFrq(0.49f);

                            float[] mixed=new float[256];
                            for (int i = 0; i < 256; i++)
                            {
                                mixed[i] = visData.Frequencies[i];
                            }

                            if (!RainbowMode && !RandomMode)
                            {
                                if (ModeProb == 1)
                                {
                                    MyField.Update(mixed.ToArray(),
                                        mixed.Length/WaveWaveZoomProb, waveColor, ModeProb, FadeOutColor,
                                        myColorMode, flatModeInWave, heightMultiplier);
                                }
                                else if (ModeProb == 2)
                                {
                                    MyField.Update(mixed.ToArray(),
                                        mixed.Length / CircleWaveZoomProb, waveColor, ModeProb, FadeOutColor,
                                        myColorMode, flatModeInCircle, heightMultiplier);
                                }
                            }
                            else if (RandomMode)
                            {
                                if (ModeProb == 1)
                                {
                                    MyField.Update(mixed.ToArray(),
                                        mixed.Length / WaveWaveZoomProb, GetRandomColor(), ModeProb,
                                        FadeOutColor, myColorMode, flatModeInWave, heightMultiplier);
                                }
                                else if (ModeProb == 2)
                                {
                                    MyField.Update(mixed.ToArray(),
                                        mixed.Length / CircleWaveZoomProb, GetRandomColor(), ModeProb,
                                        FadeOutColor, myColorMode, flatModeInCircle, heightMultiplier);
                                }
                            }
                            else
                            {
                                if (ModeProb == 1)
                                {
                                    MyField.Update(mixed.ToArray(),
                                        mixed.Length / WaveWaveZoomProb, GetRainbowColor(RainbowPointerProb),
                                        ModeProb, FadeOutColor, myColorMode, flatModeInWave, heightMultiplier);
                                }
                                else if (ModeProb == 2)
                                {
                                    MyField.Update(mixed.ToArray(),
                                        mixed.Length / CircleWaveZoomProb,
                                        GetRainbowColor(RainbowPointerProb), ModeProb, FadeOutColor, myColorMode,
                                        flatModeInCircle, heightMultiplier);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Songs.Length == 0)
                        {
                            Exit();
                            return;
                        }
                        SongPointerProb++;
                        MediaPlayer.Play(Songs[SongPointerProb - 1].MySong);
                        IsStopped = false;
                        IsPlaying = true;
                    }

                    if (!lockMovement)
                    {
                        Cam.Update(gameTime);
                        Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Width/2,
                            graphics.GraphicsDevice.Viewport.Height/2);
                    }
                    else
                    {
                        if (NewKeyboardState.IsKeyUp(Keys.O) &&
                            OldKeyboardState.IsKeyDown(Keys.O))
                        {
                            Cam.Orbit = !Cam.Orbit;
                        }

                        if (Cam.Orbit)
                        {
                            Cam.Position = Vector3.Transform(Cam.Position, Matrix.CreateRotationY(orbitSpeed));
                            Cam.Target = new Vector3(10, 0, 10);
                            Cam.View = Matrix.CreateLookAt(Cam.Position, Cam.Target, Vector3.Up);
                        }
                    }

                    if (RainbowMode)
                    {
                        RainbowPointerProb += (float) gameTime.ElapsedGameTime.TotalMilliseconds/5000f;
                    }

                    if (Information)
                    {
                        if (!PauseInformationFromFlowingIn)
                        {
                            InformationOffset += (float) gameTime.ElapsedGameTime.TotalMilliseconds/10;
                            if (-InformationOffset +
                                InformationFont.MeasureString("  Song:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Tag.Title +
                                                              " || Album:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Tag.Album +
                                                              " || Artist:  " +
                                                              Songs[SongPointerProb - 1].MySong.Artist.Name +
                                                              " || Duration:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Properties.Duration +
                                                              " || Genre:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre +
                                                              " || " + "  Song:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Tag.Title +
                                                              " || Album:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Tag.Album +
                                                              " || Artist:  " +
                                                              Songs[SongPointerProb - 1].MySong.Artist.Name +
                                                              " || Duration:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Properties.Duration +
                                                              " || Genre:  " +
                                                              Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre).X <
                                0)
                            {
                                InformationOffset = -graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width + 254;
                                //this.InformationOffset = 0;
                            }
                        }
                        PauseInformationFloatCounter += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (PauseInformationFloatCounter >= PAUSEINFORMATIONFLOATMAXCOUNTER)
                        {
                            PauseInformationFromFlowingIn = !PauseInformationFromFlowingIn;
                            PauseInformationFloatCounter = 0;
                        }
                    }

                    OldKeyboardState = NewKeyboardState;
                    OldMouseState = NewMouseState;
                }
                else
                {
                    splashcounter += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (splashcounter >= SplashMaxCount)
                    {
                        splash = false;
                    }
                }
            }
            else
            {
                NewKeyboardState = Keyboard.GetState();
                NewMouseState = Mouse.GetState();

                if (NewKeyboardState.IsKeyUp(Keys.Escape) && OldKeyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                if (NewKeyboardState.IsKeyUp(Keys.U) && OldKeyboardState.IsKeyDown(Keys.U))
                {
                    if (!IsNowSelectingADirectory)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            IsNowSelectingADirectory = true;

                            if (chooseDirectory.ShowDialog() == DialogResult.OK)
                            {
                                SongDirectory = chooseDirectory.SelectedPath;
                            }

                            ReloadSongs();

                            IsNowSelectingADirectory = false;
                        });
                    }
                }

                if (NewKeyboardState.IsKeyUp(Keys.RightShift) && OldKeyboardState.IsKeyDown(Keys.RightShift))
                {
                    ReloadSongs();
                }

                OldKeyboardState = NewKeyboardState;
                OldMouseState = NewMouseState;
            }

            base.Update(gameTime);
        }

        private Color GetRainbowColor(float progress)
        {
            var div = (Math.Abs(progress%1)*6);
            var ascending = (int) ((div%1)*255);
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
            return new Color(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //if (!this.ShouldStayFocused)
            //{
            //    if (!this.IsActive)
            //    {
            //        return;
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

            GraphicsDevice.Clear(this.ClearColor);

            if (Songs.Length > 0)
            {
                if (splash)
                {
                    spriteBatch.Begin();
                    DrawInCenter(
                        spriteBatch,
                        new Rectangle(
                            0, 0,
                            SplashScreen.Width.Clamp(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 4,
                                graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width),
                            SplashScreen.Height.Clamp(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 4,
                                graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height)),
                        SplashScreen,
                        Vector2.Zero);

                    spriteBatch.End();
                }
                else
                {
                    var temp = GraphicsDevice.RasterizerState = new RasterizerState()
                    {
                        CullMode = CullMode.None,
                        FillMode = GraphicsDevice.RasterizerState.FillMode
                    };
                    GraphicsDevice.BlendState = BlendState.Opaque;
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                    if (!IsStopped&&wave_3D)
                    {
                        BasicEffect.Projection = Cam.Projektion;
                        BasicEffect.View = Cam.View;
                        BasicEffect.World = Matrix.Identity;
                        BasicEffect.VertexColorEnabled = true;
                        BasicEffect.TextureEnabled = false;

                        for (int i = 0; i < BasicEffect.CurrentTechnique.Passes.Count; i++)
                        {
                            BasicEffect.CurrentTechnique.Passes[i].Apply();

                            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, MyField.GetData(), 0,
                                MyField.GetElementCount() / 3);
                        }
                    }

                    spriteBatch.Begin();
                    if (wave_2D)
                    {
                        int x, y, width, height;

                        for (int s = 0; s < visData.Samples.Count; s++)
                        {
                            x = GraphicsDevice.Viewport.Width*s/visData.Samples.Count;
                            width = 8;
                            y =
                                (int)
                                    (GraphicsDevice.Viewport.Height/3f -
                                     visData.Samples[s]*GraphicsDevice.Viewport.Height/4);
                            height =
                                (int)
                                    ((visData.Samples[s] > 0.0f ? 1 : -1f)*visData.Samples[s]*
                                     GraphicsDevice.Viewport.Height/4f);

                            spriteBatch.Draw(this.OnePixelTexture, new Rectangle(x, y, width, height),
                                this.waveColor.HalfNegate());
                        }

                        for (int f = 0; f < visData.Frequencies.Count; f++)
                        {
                            x = GraphicsDevice.Viewport.Width * f / visData.Frequencies.Count;
                            y = (int)(GraphicsDevice.Viewport.Height - visData.Frequencies[f] * GraphicsDevice.Viewport.Height / 2);
                            width = 1;
                            height = (int)(visData.Frequencies[f] * GraphicsDevice.Viewport.Height / 2);
                            spriteBatch.Draw(this.OnePixelTexture, new Rectangle(x+width*2, y, width, height), this.waveColor.Negate());
                            spriteBatch.Draw(this.OnePixelTexture, new Rectangle(x, y, width*2, height), this.waveColor);
                        }
                    }

                    if (Information)
                    {
                        if (!IsPlaying)
                        {
                            spriteBatch.Draw(OnePixelTexture, new Rectangle(252, 52, 104, 50), Color.White);
                            spriteBatch.Draw(OnePixelTexture, new Rectangle(254, 54, 100, 46), Color.Black);
                            spriteBatch.DrawString(InformationFont, "Paused", new Vector2(261, 63), Color.Red);
                        }

                        if (lockMovement)
                        {
                            spriteBatch.Draw(OnePixelTexture, new Rectangle(252, 100, 104, 50), Color.White);
                            spriteBatch.Draw(OnePixelTexture, new Rectangle(254, 102, 100, 46), Color.Black);
                            spriteBatch.DrawString(InformationFont, "Locked", new Vector2(261, 111), Color.Red);
                        }

                        if (TopMost)
                        {
                            spriteBatch.Draw(OnePixelTexture, new Rectangle(252, 148, 130, 50), Color.White);
                            spriteBatch.Draw(OnePixelTexture, new Rectangle(254, 150, 126, 46), Color.Black);
                            spriteBatch.DrawString(InformationFont, "Top most", new Vector2(261, 156), Color.Red);
                        }

                        spriteBatch.Draw(OnePixelTexture,
                            new Rectangle(0, 0, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, 54),
                            Color.White);
                        spriteBatch.Draw(OnePixelTexture,
                            new Rectangle(2, 2, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width - 4, 50),
                            Color.Black);

                        if (PauseInformationFromFlowingIn)
                        {
                            spriteBatch.DrawString(InformationFont,
                                "  Song:  " + Songs[SongPointerProb - 1].TagLibFile.Tag.Title + " || Album:  " +
                                Songs[SongPointerProb - 1].TagLibFile.Tag.Album + " || Artist:  " +
                                Songs[SongPointerProb - 1].MySong.Artist.Name + " || Duration:  " +
                                Songs[SongPointerProb - 1].TagLibFile.Properties.Duration + " || Genre:  " +
                                Songs[SongPointerProb - 1].TagLibFile.Tag.FirstGenre, new Vector2(255, 20), Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(InformationFont,
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

                        spriteBatch.Draw(OnePixelTexture, new Rectangle(0, 0, 258, 258), Color.White);
                        spriteBatch.Draw(OnePixelTexture, new Rectangle(2, 2, 254, 254), Color.Black);

                        if (Songs[SongPointerProb - 1].TagLibFile.Tag.Pictures.Length >= 1)
                        {
                            spriteBatch.Draw(Songs[SongPointerProb - 1].Thumbnail, new Rectangle(4, 4, 250, 250),
                                Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(InformationFont,
                                "There is \nno thumbnail\nto display here!\n                ", new Vector2(20, 20),
                                Color.Red);
                        }
                    }

                    spriteBatch.End();
                    GraphicsDevice.RasterizerState = temp;
                }
            }
            else
            {
                if (Information)
                {
                    spriteBatch.Begin();

                    if (!IsPlaying)
                    {
                        spriteBatch.Draw(OnePixelTexture, new Rectangle(252, 52, 104, 50), Color.White);
                        spriteBatch.Draw(OnePixelTexture, new Rectangle(254, 54, 100, 46), Color.Black);
                        spriteBatch.DrawString(InformationFont, "Paused", new Vector2(261, 63), Color.Red);
                    }

                    if (lockMovement)
                    {
                        spriteBatch.Draw(OnePixelTexture, new Rectangle(252, 100, 104, 50), Color.White);
                        spriteBatch.Draw(OnePixelTexture, new Rectangle(254, 102, 100, 46), Color.Black);
                        spriteBatch.DrawString(InformationFont, "Locked", new Vector2(261, 111), Color.Red);
                    }

                    if (TopMost)
                    {
                        spriteBatch.Draw(OnePixelTexture, new Rectangle(252, 148, 130, 50), Color.White);
                        spriteBatch.Draw(OnePixelTexture, new Rectangle(254, 150, 126, 46), Color.Black);
                        spriteBatch.DrawString(InformationFont, "Top most", new Vector2(261, 156), Color.Red);
                    }

                    spriteBatch.Draw(OnePixelTexture,
                        new Rectangle(0, 0, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, 54), Color.White);
                    spriteBatch.Draw(OnePixelTexture,
                        new Rectangle(2, 2, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width - 4, 50),
                        Color.Black);

                    spriteBatch.Draw(OnePixelTexture, new Rectangle(0, 0, 258, 258), Color.White);
                    spriteBatch.Draw(OnePixelTexture, new Rectangle(2, 2, 254, 254), Color.Black);
                    spriteBatch.DrawString(InformationFont,
                        "There are \nno music file\nin your folder!\n                ", new Vector2(20, 20), Color.Red);

                    spriteBatch.End();
                }
            }

            base.Draw(gameTime);
        }
    }
}