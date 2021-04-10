using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cuivre.Code.Screens;

namespace Cuivre.Code
{
    class CollapseButton : Button
    {
        private List<Button> collapsedButtons;

        private bool focused = false;

        private Rectangle quitRectangle;

        public bool FreezesTime { get; set; }

        public CollapseButton(int x, int y, int w, int h, Texture2D texture, bool freeze, List<Button> buttons, Action<Screen> action) : base(x, y, w, h, texture, action)
        {
            FreezesTime = freeze;
            collapsedButtons = buttons;
            quitRectangle = new Rectangle(x + w + 10, y, 20, 20);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (focused)
            {
                foreach (Button b in collapsedButtons)
                {
                    b.Draw(gameTime, spriteBatch);
                }
                spriteBatch.Draw(Game1.white, quitRectangle, Color.Red);
            }
            else
            {
                base.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime, MouseState prevMouseState, MouseState mouseState, Screen screen)
        {
            if (focused)
            {
                foreach (Button b in collapsedButtons)
                {
                    b.Update(gameTime, prevMouseState, mouseState, screen);
                }

                if (quitRectangle.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    focused = false;
                    ((GameScreen)screen).Focused = null;
                    clickAction(screen);
                }
            }

            base.Update(gameTime, prevMouseState, mouseState, screen);
        }

        public override void Click(Screen screen)
        {
            focused = true;
            ((GameScreen)screen).Focused = this;
        }

        public override void Reset()
        {
            focused = false;
            foreach (Button b in collapsedButtons)
            {
                b.Reset();
            }
            base.Reset();
        }
    }
}
