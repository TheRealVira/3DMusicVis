#region License

// Copyright (c) 2015, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Game1.cs
// Date - created: 2015.08.26 - 14:45
// Date - current: 2016.05.19 - 20:03

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
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static Game1 FreeBeer;
        public static SpriteFont InformationFont;
        public static Texture2D SplashScreen;
        public static MouseState NewMouseState;
        public static MouseState OldMouseState;
        public static KeyboardState NewKeyboardState;
        public static KeyboardState OldKeyboardState;

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

            Resolution.Init(ref Graphics);
            Resolution.SetVirtualResolution(1920, 1080);
            Resolution.SetResolution(Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width,
                Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height, true);

            ScreenManager.Initialise(new List<Screen.Screen>
            {
                new SplashScreen(Graphics, Content.Load<Texture2D>("Vira")),
#if(DEBUG)
                new OldScreen(Graphics)
#endif
            });


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

            SplashScreen = Content.Load<Texture2D>("Vira");
            InformationFont = Content.Load<SpriteFont>("InformationFont");

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