﻿#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ListBox.cs
// Date - created:2016.07.02 - 17:05
// Date - current: 2016.09.18 - 13:12

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace _3DMusicVis2.VisualControls
{
    internal class ListBox : VisualControl
    {
        private const int SCROLL_SPEED = 10;

        public List<Label> Items;
        public Label SelectedItem;

        public ListBox(Rectangle bounding, Texture2D texture, List<string> textList, Texture2D induvidualBackground,
            SpriteFont font) : base(bounding, texture, DefaultDrawColor, Color.White, Color.White)
        {
            Items = BakeLabels(textList, induvidualBackground, font);

            ScrolledUp += ListBox_ScrolledUp;
            ScrolledDown += ListBox_ScrolledDown;
        }

        public event ItemSelected ItemWasSelected;

        private void ListBox_ScrolledDown(object sender, EventArgs e)
        {
            Scroll(SCROLL_SPEED);
        }

        private void ListBox_ScrolledUp(object sender, EventArgs e)
        {
            Scroll(-SCROLL_SPEED);
        }

        public void Scroll(int value)
        {
            if (Items.Count == Bounding.Y) return;

            if (value + Items[0].Bounding.Y < Bounding.Y)
            {
                value = Bounding.Y - Items[0].Bounding.Y;
            }

            foreach (var label in Items)
            {
                label.Bounding.Y += value;
            }
        }

        private List<Label> BakeLabels(List<string> toBeConverted, Texture2D background, SpriteFont font,
            int labelHeight = 100)
        {
            var toRet = new List<Label>();

            var y = Bounding.Y;
            foreach (var text in toBeConverted)
            {
                var temp = new Label(new Rectangle(Bounding.X, y, Bounding.Width, labelHeight), background, font, text);
                temp.MousePressed += OnItem_MousePressed;
                toRet.Add(temp);
                y += labelHeight;
            }

            return toRet;
        }

        private void OnItem_MousePressed(object sender, EventArgs e)
        {
            if (SelectedItem != null)
            {
                SelectedItem.DrawColor = DefaultDrawColor;
            }

            SelectedItem = (Label) sender;
            SelectedItem.DrawColor = Color.Blue;
            ItemWasSelected?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            base.Draw(gameTime, spriteBatch);

            foreach (var label in Items)
            {
                if (Bounding.Contains(label.Bounding))
                {
                    label.Draw(gameTime, spriteBatch);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, int borderWidth)
        {
            if (!IsVisible) return;

            base.Draw(gameTime, spriteBatch, borderWidth);

            foreach (var label in Items)
            {
                if (Bounding.Intersects(label.Bounding))
                {
                    label.Draw(gameTime, spriteBatch, borderWidth);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsVisible) return;

            base.Update(gameTime);
            foreach (var label in Items)
            {
                label.Update(gameTime);
            }
        }

        public void Add(string value, Texture2D background, SpriteFont font,
            int labelHeight = 100)
        {
            if (Items.Count > 0)
            {
                Items.Add(
                    new Label(
                        new Rectangle(Bounding.X,
                            Items[Items.Count - 1].Bounding.Y + labelHeight, Bounding.Width,
                            labelHeight), background, font, value));
            }
            else
            {
                Items.Add(
                    new Label(
                        new Rectangle(Bounding.X,
                            Bounding.Y, Bounding.Width,
                            labelHeight), background, font, value));
            }
        }

        public bool AllItemsAreOutSideTheBounding()
        {
            return Items.All(label => !Bounding.Contains(label.Bounding));
        }
    }
}