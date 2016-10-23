#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: ListBox.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.10.23 - 18:25

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

        private readonly Texture2D _induvBack;
        private readonly SpriteFont _myFont;

        public List<Label> Items;

        public ListBox(Rectangle bounding, Texture2D texture, List<string> textList, Texture2D induvidualBackground,
            SpriteFont font) : base(bounding, texture, DefaultDrawColor, Color.White, Color.White)
        {
            Items = BakeLabels(textList, induvidualBackground, font);
            _induvBack = induvidualBackground;
            _myFont = font;

            ScrolledUp += ListBox_ScrolledUp;
            ScrolledDown += ListBox_ScrolledDown;
        }

        public Label SelectedItem { get; private set; }
        public int SelectedIndex { get; private set; }

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
            if (Items.Count == 0) return;

            if (value + Items[0].Bounding.Y + Items[0].Bounding.Height > Bounding.Y + Bounding.Height)
            {
                value = Bounding.Y + Bounding.Height - (Items[0].Bounding.Y + Items[0].Bounding.Height);
            }
            else if (value + Items[Items.Count - 1].Bounding.Y < Bounding.Y)
            {
                value = Bounding.Y - Items[Items.Count - 1].Bounding.Y;
            }

            foreach (var label in Items)
            {
                label.Bounding.Y += value;
                label.IsVisible = Bounding.Intersects(label.Bounding);
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
            var temp = (Label) sender;

            if (!temp.IsVisible) return;

            if (SelectedItem != null)
            {
                SelectedItem.DrawColor = DefaultDrawColor;
            }

            for (var i = 0; i < Items.Count; i++)
            {
                if (temp.Bounding.Y == Items[i].Bounding.Y)
                {
                    SelectedIndex = i;
                    break;
                }
            }

            SelectedItem = temp;
            SelectedItem.DrawColor = Color.Blue;
            ItemWasSelected?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            base.Draw(gameTime, spriteBatch);

            foreach (var label in Items)
            {
                if (label.IsVisible)
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
                if (label.IsVisible)
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

        public void RemoveItem(Label l)
        {
            Items.Remove(l);
            if (l == SelectedItem)
            {
                SelectedItem = null;
            }

            Items = BakeLabels(Items.Select(x => x.Text).ToList(), _induvBack, _myFont);
        }
    }
}