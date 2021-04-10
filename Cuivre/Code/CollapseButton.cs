using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code
{
    class CollapseButton : Button
    {
        private List<Button> collapsedButtons;

        public CollapseButton(int x, int y, int w, int h, Texture2D texture, List<Button> buttons) : base(x, y, w, h, texture, null)
        {
            collapsedButtons = buttons;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (hovered)
            {
                foreach (Button b in collapsedButtons)
                {
                    b.Draw(gameTime, spriteBatch);
                }
            }
            else
            {
                base.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime, MouseState prevMouseState, MouseState mouseState, Screen screen)
        {
            hovered = Rectangle.Contains(mouseState.X, mouseState.Y);

            if (hovered)
            {
                foreach (Button b in collapsedButtons)
                {
                    b.Update(gameTime, prevMouseState, mouseState, screen);
                }
            }
        }

        public override void Reset()
        {
            foreach (Button b in collapsedButtons)
            {
                b.Reset();
            }
        }
    }
}
