#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: VisualControl.cs
// Date - created: 2016.05.22 - 11:03
// Date - current: 2016.05.22 - 16:48

#endregion

#region Usings

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace _3DMusicVis2.VisualControls
{
    public delegate void MouseHoverEventHandler(object sender, EventArgs e);

    public delegate void MouseExitsEventHandler(object sender, EventArgs e);

    public delegate void MouseButtonUpHandler(object sender, EventArgs e);

    public delegate void MouseButtonDownHandler(object sender, EventArgs e);

    public delegate void MousePressedHandler(object sender, EventArgs e);

    public delegate void ScrolledUpHandler(object sender, EventArgs e);

    public delegate void ScrolledDownHandler(object sender, EventArgs e);

    abstract class VisualControl
    {
        public Rectangle Bounding;
        protected Color DrawColor;
        protected Texture2D Texture;

        protected VisualControl(Rectangle bounding, Texture2D texture, Color drawColor)
        {
            Bounding = bounding;
            Texture = texture;
            DrawColor = drawColor;
        }

        public bool WasHovering { protected get; set; }
        public bool WasPressing { protected get; set; }
        public bool WasReleasing { protected get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if (Bounding.Contains(new Point(Game1.NewMouseState.X, Game1.NewMouseState.Y)))
            {
                if (!WasHovering)
                {
                    WasHovering = true;
                    OnMouseHover();
                }

                if (WasHovering)
                {
                    if (Game1.OldMouseState.ScrollWheelValue < Game1.NewMouseState.ScrollWheelValue)
                    {
                        OnScrolledUp();
                    }
                    else if (Game1.OldMouseState.ScrollWheelValue > Game1.NewMouseState.ScrollWheelValue)
                    {
                        OnScrolledDown();
                    }
                }

                if (Game1.NewMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (!WasPressing)
                    {
                        WasPressing = true;
                        OnMouseButtonDown();
                    }

                    return;
                }

                WasPressing = false;

                if (Game1.OldMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (!WasReleasing)
                    {
                        WasReleasing = true;
                        OnMousePressed();
                    }

                    return;
                }

                if (WasReleasing)
                {
                    WasReleasing = false;
                    OnMouseButtonUp();
                }

                return;
            }

            if (WasHovering)
            {
                WasHovering = false;
                OnMouseExits();
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounding, DrawColor);
        }

        /// <summary>
        ///     Draws the specified visual control (triggers the Draw Method) and draws the bouding of that control.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="borderWidth">Width of the border.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, int borderWidth)
        {
            Draw(gameTime, spriteBatch);

            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Left, Bounding.Top, borderWidth, Bounding.Height), DrawColor); // Left
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Right, Bounding.Top, borderWidth, Bounding.Height + borderWidth), DrawColor);
            // Right
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Left, Bounding.Top, Bounding.Width, borderWidth), DrawColor); // Top
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Left, Bounding.Bottom, Bounding.Width, borderWidth), DrawColor); // Bottom
        }

        protected event MouseHoverEventHandler MouseHover;
        protected event MouseExitsEventHandler MouseExits;
        protected event MouseButtonUpHandler MouseButtonUp;
        protected event MouseButtonDownHandler MouseButtonDown;
        protected event MousePressedHandler MousePressed;
        protected event ScrolledUpHandler ScrolledUp;
        protected event ScrolledDownHandler ScrolledDown;

        protected virtual void OnMouseHover()
        {
            MouseHover?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMouseButtonDown()
        {
            MouseButtonDown?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMouseButtonUp()
        {
            MouseButtonUp?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMousePressed()
        {
            MousePressed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMouseExits()
        {
            MouseExits?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnScrolledUp()
        {
            ScrolledUp?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnScrolledDown()
        {
            ScrolledDown?.Invoke(this, EventArgs.Empty);
        }
    }
}