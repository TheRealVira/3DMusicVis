#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Game1.cs
// Date - created:2016.12.10 - 09:36
// Date - current: 2017.04.13 - 14:32

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis.Manager;
using _3DMusicVis.RecordingType;
using _3DMusicVis.RenderFrame;
using _3DMusicVis.Shader;
using Console = _3DMusicVis.VisualControls.Console;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using MainMenu = _3DMusicVis.Screen.MainMenu;

#endregion

namespace _3DMusicVis
{
    public class Game1 : Game
    {
        public const float PAUSEINFORMATIONFLOATMAXCOUNTER = 20000;
        public const int FIELD_WIDTH = 100;
        public const float INITAIL_HEIGHT = 0;
        public const float SplashMaxCount = 1000;

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
        public static BasicEffect BasicEffect;

        public static Console MyConsole;

        public static Texture2D ViraLogo;
        public static Texture2D FamouseOnePixel;
        public static Texture2D GhostPixel;
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

            _3DFrequencyRenderer.Dispose();
            _3DSampleRenderer.Dispose();
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

            Rand = new Random(DateTime.Now.Millisecond);
            FamouseOnePixel = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            FamouseOnePixel.SetData(new[] {Color.White});
            GhostPixel = new Texture2D(GraphicsDevice, 1, 1);
            GhostPixel.SetData(new[] {Color.Transparent});
            InformationFont = Content.Load<SpriteFont>("Fonts/InformationFont");
            ConsoleFont = Content.Load<SpriteFont>("Fonts/Console");

            ShadersManager.Initialize();
            System.Console.WriteLine("Initialized the Shaders...");

            MyConsole = new Console(Console.ConsoleBoundings, FamouseOnePixel);

            System.Console.WriteLine("Initialized the OutputManager...");

            ResolutionManager.ApplyResolution(Graphics);

            System.Console.WriteLine("Initialized the Resolution...");

            BasicEffect = new BasicEffect(GraphicsDevice)
            {
                World = Matrix.Identity,
                VertexColorEnabled = true,
                TextureEnabled = false
            };

            //MediaPlayerManager.Initialise();
            //System.Console.WriteLine("Initialized the MediaPlayerManager...");

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

            _3DFrequencyRenderer.Initialise(GraphicsDevice, Content);
            System.Console.WriteLine("Initialized the _3DFrequencyRenderer...");
            _3DSampleRenderer.Initialise(GraphicsDevice, Content);
            System.Console.WriteLine("Initialized the _3DSampleRenderer...");

            RealTimeRecording.Initialize();
            System.Console.WriteLine("Initialized the RealTimeRecorder...");
            GaussianBlurManager.Initialize(GraphicsDevice, this);
            System.Console.WriteLine("Initialized the gaussianblur...");
            BloomManager.Initialize(this);
            System.Console.WriteLine("Initialized the bloom...");
            _3DLinearFrequencyRenderer.Initialise(GraphicsDevice);
            System.Console.WriteLine("Initialized the _3DLinearFrecuencyRenderer...");
            DEFAULT_RENDERTARGET = new RenderTarget2D(GraphicsDevice, ResolutionManager.VIRTUAL_RESOLUTION.Width,
                ResolutionManager.VIRTUAL_RESOLUTION.Height, true,
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

            //MediaPlayerManager.LoadContent(Content, @"3DMusicVis2\Music", true, GraphicsDevice);
            //System.Console.WriteLine("Finished loading the content!");

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

            InputManager.UpdateMousePosition(NewMouseState, Graphics);

            MyConsole.Update(gameTime);

            if (NewKeyboardState.IsKeyDown(Keys.RightAlt) && Keys.Enter.KeyWasClicked())
                ResolutionManager.ToggleFullScreen(Graphics);

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

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            SpriteBatch.Draw(DEFAULT_RENDERTARGET, GraphicsDevice.Viewport.Bounds, Color.White);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}