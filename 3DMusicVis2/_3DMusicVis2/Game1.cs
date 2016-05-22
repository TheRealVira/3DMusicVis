#region License

// Copyright (c) 2015, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Game1.cs
// Date - created: 2015.08.26 - 14:45
// Date - current: 2016.05.22 - 12:52

#endregion

#region Usings

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3DMusicVis2.Manager;
using _3DMusicVis2.Screen;

#endregion

namespace _3DMusicVis2
{
    public class Game1 : Game
    {
        public const float SplashMaxCount = 1000;
        public static Rectangle VIRTUAL_RESOLUTION;

        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static Game1 FreeBeer;
        public static SpriteFont InformationFont;
        public static MouseState NewMouseState;
        public static MouseState OldMouseState;
        public static KeyboardState NewKeyboardState;
        public static KeyboardState OldKeyboardState;

        public static Texture2D ViraLogo;
        public static Texture2D FamouseOnePixel;
        public static Texture2D GhostPixel;
        private Texture2D _3DMusicVisLogo;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            FreeBeer = this;
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

            FamouseOnePixel = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            FamouseOnePixel.SetData(new[] {Color.White});
            GhostPixel = new Texture2D(GraphicsDevice, 1, 1);
            GhostPixel.SetData(new[] {Color.Transparent});

            VIRTUAL_RESOLUTION = new Rectangle(0, 0, 1920, 1080);

            Resolution.Init(ref Graphics);
            Resolution.SetVirtualResolution(VIRTUAL_RESOLUTION.Width, VIRTUAL_RESOLUTION.Height);
            Resolution.SetResolution(Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height, true);


            base.Initialize();
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
            InformationFont = Content.Load<SpriteFont>("InformationFont");

            ScreenManager.Initialise(new List<Screen.Screen>
            {
                new SplashScreen(Graphics, ViraLogo),
                new SplashScreen(Graphics, _3DMusicVisLogo),
                new Credits(Graphics),
                new TestForm(Graphics)
            });

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

            if (NewKeyboardState.IsKeyUp(Keys.Escape) && OldKeyboardState.IsKeyDown(Keys.Escape))
            {
                FreeBeer.Exit();
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
            SpriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());

            ScreenManager.Draw(SpriteBatch, gameTime);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}