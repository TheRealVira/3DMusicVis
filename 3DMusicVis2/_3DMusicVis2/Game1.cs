#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Game1.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.11.11 - 09:50

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.Manager;
using _3DMusicVis2.RecordingType;
using _3DMusicVis2.RenderFrame;
using _3DMusicVis2.Shader;
using Color = Microsoft.Xna.Framework.Color;
using Console = _3DMusicVis2.VisualControls.Console;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using MainMenu = _3DMusicVis2.Screen.MainMenu;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

#endregion

namespace _3DMusicVis2
{
    public class Game1 : Game
    {
        public const float PAUSEINFORMATIONFLOATMAXCOUNTER = 20000;
        public const int FIELD_WIDTH = 100;
        public const float INITAIL_HEIGHT = 0;
        public const float SplashMaxCount = 1000;
        public static Rectangle VIRTUAL_RESOLUTION = new Rectangle(0, 0, 1920, 1080);

        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;

        // A public instance of this class. (This is a nice work arround if you want to access public-non-static members)
        public static Game1 FreeBeer;

        public static SpriteFont InformationFont;
        public static SpriteFont ConsoleFont;

        public static MouseState NewMouseState;
        public static MouseState OldMouseState;
        public static KeyboardState NewKeyboardState;
        public static KeyboardState OldKeyboardState;
        public static Random Rand;
        public static AudioAnalysisXNAClass AudioAnalysis;
        public static BasicEffect BasicEffect;

        public static Console MyConsole;

        public static Texture2D ViraLogo;
        public static Texture2D FamouseOnePixel;
        public static Texture2D GhostPixel;
        public static Effect ScanlinEffect;
        public static Effect LiquifyEffect;
        public static RenderTarget2D DEFAULT_RENDERTARGET;

        private Texture2D _3DMusicVisLogo;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            FreeBeer = this;
            Exiting += Game1_Exiting;
        }

        private void Game1_Exiting(object sender, EventArgs e)
        {
            ScreenManager.UnloadAll();
            RealTimeRecording.UnloadMe();
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //ModeProb = 2;

            //Graphics.IsFullScreen = false;
            //Graphics.PreferredBackBufferHeight = Graphics.GraphicsDevice.Viewport.Bounds.Height;
            //Graphics.PreferredBackBufferWidth = Graphics.GraphicsDevice.Viewport.Bounds.Width;
            InactiveSleepTime = new TimeSpan(0);
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60);
            //graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.ApplyChanges();

            Rand = new Random(DateTime.Now.Millisecond);
            FamouseOnePixel = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            FamouseOnePixel.SetData(new[] {Color.White});
            GhostPixel = new Texture2D(GraphicsDevice, 1, 1);
            GhostPixel.SetData(new[] {Color.Transparent});
            InformationFont = Content.Load<SpriteFont>("Fonts/InformationFont");
            ConsoleFont = Content.Load<SpriteFont>("Fonts/Console");
            ScanlinEffect = Content.Load<Effect>("Shader/Random");
            LiquifyEffect = Content.Load<Effect>("Shader/Liquify");

            MyConsole = new Console(Console.ConsoleBoundings, FamouseOnePixel);

            System.Console.WriteLine("IsInitialised the OutputManager...");

            //Resolution.Init(ref Graphics);
            //Resolution.SetVirtualResolution(VIRTUAL_RESOLUTION.Width, VIRTUAL_RESOLUTION.Height);
            //Resolution.SetResolution(Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
            //    Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height, false);
            Graphics.PreferredBackBufferWidth = Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            //var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            //form.Location = new System.Drawing.Point(10, 10);

            System.Console.WriteLine("IsInitialised the Resolution...");

            AudioAnalysis = new AudioAnalysisXNAClass();
            BasicEffect = new BasicEffect(GraphicsDevice)
            {
                World = Matrix.Identity,
                VertexColorEnabled = true,
                TextureEnabled = false
            };

            MediaPlayerManager.Initialise();
            System.Console.WriteLine("Initialized the MediaPlayerManager...");

            ImageManager.Initialise();
            System.Console.WriteLine("Initialized the ImageManager...");

            _3DCirclularWaveRenderer.Initialise(GraphicsDevice);
            System.Console.WriteLine("Initialized the _3DCirclularWaveRenderer...");
            _3DLinearWaveRenderer.Initialise(GraphicsDevice);
            System.Console.WriteLine("Initialized the _3DLinearWaveRenderer...");
            _2DSampleRenderer.Initialise(GraphicsDevice);
            System.Console.WriteLine("Initialized the _2DSampleRenderer...");
            _2DFrequencyRenderer.Initialise(GraphicsDevice);
            System.Console.WriteLine("Initialized the _2DFrequencyRenderer...");
            RealTimeRecording.Initialize();
            System.Console.WriteLine("Initialized the RealTimeRecorder...");
            GaussianBlurManager.Initialize(GraphicsDevice, this);
            System.Console.WriteLine("Initialized the gaussianblur...");
            BloomManager.Initialize(this);
            System.Console.WriteLine("Initialized the bloom...");
            _3DLinearFrequencyRenderer.Initialise(GraphicsDevice);
            System.Console.WriteLine("Initialized the _3DLinearFrecuencyRenderer...");
            DEFAULT_RENDERTARGET = new RenderTarget2D(GraphicsDevice, VIRTUAL_RESOLUTION.Width,
                VIRTUAL_RESOLUTION.Height, true,
                GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            System.Console.WriteLine("Initialized the default rendertarget...");

            base.Initialize();
            System.Console.WriteLine("Finished initialising 3DMusicVis2!");

            (Control.FromHandle(FreeBeer.Window.Handle) as Form).MinimizeBox = false;

            TemplateManager.SaveDefaultTemplates();

            //var test = SettingsManager.Load();
            //for (int i = 0; i < test[0].Bundles.Count; i++)
            //{
            //    test[0].Bundles[i]=new SettingsBundle()
            //    {
            //        Color=test[0].Bundles[i].Color,
            //        Trans = test[0].Bundles[i].Trans,
            //        Is3D = test[0].Bundles[i].Is3D,
            //        IsDashed = true
            //    };
            //}
            //SettingsManager.Save(test[0]);
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ViraLogo = Content.Load<Texture2D>("Splashscreens/Vira");
            _3DMusicVisLogo = Content.Load<Texture2D>("Splashscreens/3DMusicVisLogo");

            ScreenManager.Initialise(new List<Screen.Screen>
            {
                //new SplashScreen(Graphics, ViraLogo),
                //new SplashScreen(Graphics, _3DMusicVisLogo),
                //new Credits(Graphics),
                //new TestForm(Graphics)
                new MainMenu(Graphics)
            });

            MediaPlayerManager.LoadContent(Content, @"3DMusicVis2\Music", true, GraphicsDevice);
            System.Console.WriteLine("Finished loading the content!");

            GraphicsDevice.Present();
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
            NewKeyboardState = Keyboard.GetState();
            NewMouseState = Mouse.GetState();

            MyConsole.Update(gameTime);

            if (NewKeyboardState.IsKeyDown(Keys.RightAlt) && NewKeyboardState.IsKeyUp(Keys.Enter) &&
                OldKeyboardState.IsKeyDown(Keys.Enter))
            {
                var window = Control.FromHandle(FreeBeer.Window.Handle) as Form;
                var formPosition = new Point(window.Location.X, window.Location.Y);
                var dispayXMulitplikator =
                    (int) Math.Round(formPosition.X/(decimal) Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
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

                if (window.FormBorderStyle == FormBorderStyle.FixedSingle) // If fullscreen
                {
                    window.FormBorderStyle = FormBorderStyle.None;
                    window.Location =
                        new System.Drawing.Point(
                            Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width*displayXMultiplikatorForLocation, 0);
                    window.Size = new Size(VIRTUAL_RESOLUTION.Width, VIRTUAL_RESOLUTION.Height);
                }
                else
                {
                    window.FormBorderStyle = FormBorderStyle.FixedSingle;
                    window.Location =
                        new System.Drawing.Point(
                            Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width*displayXMultiplikatorForLocation,
                            -16);
                    window.Size = new Size(VIRTUAL_RESOLUTION.Width, VIRTUAL_RESOLUTION.Height + 16);
                }
            }

            ScreenManager.Update(gameTime);

            OldKeyboardState = NewKeyboardState;
            OldMouseState = NewMouseState;

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //SpriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());

            //Resolution.BeginDraw();

            Graphics.GraphicsDevice.SetRenderTarget(DEFAULT_RENDERTARGET);
            SpriteBatch.GraphicsDevice.Clear(Color.Black);
            ScreenManager.Draw(SpriteBatch, gameTime);
            //MyConsole.Draw(gameTime, SpriteBatch, 2);

            //SpriteBatch.End();

            Graphics.GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();

            SpriteBatch.Draw(DEFAULT_RENDERTARGET, GraphicsDevice.Viewport.Bounds, Color.White);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}