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

        private Texture2D focusTexture;

        private Quad[] quads;

        public bool FreezesTime { get; set; }

        public CollapseButton(int x, int y, int w, int h, Texture2D texture, Texture2D focusTexture, bool freeze, List<Button> buttons, Action<Screen> action, bool reclickable = false) : base(x, y, w, h, texture, action, reclickable)
        {
            this.focusTexture = focusTexture;
            FreezesTime = freeze;
            collapsedButtons = buttons;
            int off = w / 20;
            quitRectangle = new Rectangle(x + w + off, y - 4 * off, 4 * off, 4 * off);
            quads = new Quad[]
            {
                new Quad(new List<Point>
                {
                    new Point(x, GameScreen.leftCoords[0]), new Point(x + w, GameScreen.rightCoords[0]), new Point(x, GameScreen.leftCoords[1]), new Point(x + w, GameScreen.rightCoords[1])
                }),
                new Quad(new List<Point>
                {
                    new Point(x, GameScreen.leftCoords[1]), new Point(x + w, GameScreen.rightCoords[1]), new Point(x, GameScreen.leftCoords[2]), new Point(x + w, GameScreen.rightCoords[2])
                }),
                new Quad(new List<Point>
                {
                    new Point(x, GameScreen.leftCoords[2]), new Point(x + w, GameScreen.rightCoords[2]), new Point(x, GameScreen.leftCoords[3]), new Point(x + w, GameScreen.rightCoords[3])
                }),
                new Quad(new List<Point>
                {
                    new Point(x, GameScreen.leftCoords[3]), new Point(x + w, GameScreen.rightCoords[3]), new Point(x, GameScreen.leftCoords[4]), new Point(x + w, GameScreen.rightCoords[4])
                }),
                new Quad(new List<Point>
                {
                    new Point(x, GameScreen.leftCoords[4]), new Point(x + w, GameScreen.rightCoords[4]), new Point(x, GameScreen.leftCoords[5]), new Point(x + w, GameScreen.rightCoords[5])
                })
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (focused)
            {
                spriteBatch.Draw(focusTexture, new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);
                foreach (Button b in collapsedButtons)
                {
                    if (b.Hovered) b.Draw(gameTime, spriteBatch);
                }
                //spriteBatch.Draw(Game1.white, quitRectangle, Color.Red);
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
                int i = 0;
                foreach (Button b in collapsedButtons)
                {
                    if (quads[i].Contains(mouseState.X, mouseState.Y)) b.Update(gameTime, prevMouseState, mouseState, screen);
                    else b.Hovered = false;
                    i++;
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

        public int GetHovered()
        {
            int res = 0;
            foreach (Button b in collapsedButtons)
            {
                if (b.Hovered) res++;
            }
            return res;
        }
    }
}
