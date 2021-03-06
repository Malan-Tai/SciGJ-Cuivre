using System;
using System.Collections.Generic;
using System.Text;
using Cuivre.Code.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code
{
    class Button
    {
        private int x;
        private int y;
        private int w;
        private int h;

        protected bool reclickable = false;
        protected bool clickedToday = false;
        public bool Hovered { get; set; } = false;

        private Texture2D texture;

        protected Action<Screen> clickAction;

        protected Rectangle Rectangle
        {
            get
            {
                int off = GameScreen.betweenOffset;
                int yOff = (int)((texture.Height / (float)texture.Width) * off);
                if (Hovered) return new Rectangle(x - off, y - yOff, w + 2 * off, h + 2 * yOff);
                return new Rectangle(x, y, w, h);
            }
        }

        public Button(int x, int y, int w, int h, Texture2D texture, Action<Screen> action, bool reclickable = false)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.texture = texture;
            clickAction = action;
            this.reclickable = reclickable;
        }

        //public bool Hovered() { return hovered; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            //if (hovered) color = Color.Red;
            if (clickedToday && !reclickable) color = Color.Gray;

            spriteBatch.Draw(texture, Rectangle, color);
        }

        public virtual void Update(GameTime gameTime, MouseState prevMouseState, MouseState mouseState, Screen screen)
        {
            if ((!clickedToday || reclickable) && Rectangle.Contains(mouseState.X, mouseState.Y))
            {
                Hovered = true;
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    clickedToday = true;
                    Click(screen);
                }
            }
            else
            {
                Hovered = false;
            }
        }

        public virtual void Update(GameTime gameTime, MouseState prevMouseState, MouseState mouseState, Screen screen, Quad quad)
        {
            if ((!clickedToday || reclickable) && quad.Contains(mouseState.X, mouseState.Y))
            {
                Hovered = true;
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    clickedToday = true;
                    Click(screen);
                }
            }
            else
            {
                Hovered = false;
            }
        }

        public virtual void Click(Screen screen)
        {
            if (clickAction != null) clickAction(screen);
        }

        public virtual void Reset()
        {
            clickedToday = false;
        }

        public void LockButton()
        {
            clickedToday = true;
        }
    }
}
