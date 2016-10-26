#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: VisualControl.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.26 - 18:31

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

    public delegate void ItemSelected(object sender, EventArgs e);

    public abstract class VisualControl
    {
        public static Color DefaultDrawColor = new Color(50, 50, 50, 200);
        protected Color BorderColor;
        public Rectangle Bounding;
        public Color DrawColor;
        protected Color FontColor;
        public bool IsVisible = true;
        protected Texture2D Texture;

        protected VisualControl(Rectangle bounding, Texture2D texture, Color drawColor, Color borderColor,
            Color fontColor)
        {
            Bounding = bounding;
            Texture = texture;
            DrawColor = drawColor;
            BorderColor = borderColor;
            FontColor = fontColor;
        }

        public bool WasHovering { protected get; set; }
        public bool WasPressing { protected get; set; }
        public bool WasReleasing { protected get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsVisible) return;

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
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Bounding, DrawColor);
            spriteBatch.End();
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

            spriteBatch.Begin();
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Left, Bounding.Top, borderWidth, Bounding.Height), BorderColor); // Left
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Right, Bounding.Top, borderWidth, Bounding.Height + borderWidth), BorderColor);
            // Right
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Left, Bounding.Top, Bounding.Width, borderWidth), BorderColor); // Top
            spriteBatch.Draw(Game1.FamouseOnePixel,
                new Rectangle(Bounding.Left, Bounding.Bottom, Bounding.Width, borderWidth), BorderColor); // Bottom
            spriteBatch.End();
        }

        public event MouseHoverEventHandler MouseHover;
        public event MouseExitsEventHandler MouseExits;
        public event MouseButtonUpHandler MouseButtonUp;
        public event MouseButtonDownHandler MouseButtonDown;
        public event MousePressedHandler MousePressed;
        public event ScrolledUpHandler ScrolledUp;
        public event ScrolledDownHandler ScrolledDown;

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