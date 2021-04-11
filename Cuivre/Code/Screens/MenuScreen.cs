using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code.Screens
{
    class MenuScreen : Screen
    {
        private bool hoverNew = false;
        private bool hoverQuit = false;
        private Rectangle newRect = new Rectangle(350, 200, 100, 50);
        private Rectangle quitRect = new Rectangle(350, 300, 100, 50);

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.font, "ROMANIZER", new Vector2(350, 100), Color.Black, 0f, new Vector2(), 2f, new SpriteEffects(), 0f);

            Rectangle newR = new Rectangle(newRect.Location, newRect.Size);
            if (hoverNew) newR = new Rectangle(newRect.Location + new Point(-5, -5), newRect.Size + new Point(10, 10));
            spriteBatch.Draw(Game1.white, newR, Color.Green);

            Rectangle quit = new Rectangle(quitRect.Location, quitRect.Size);
            if (hoverQuit) quit = new Rectangle(quitRect.Location + new Point(-5, -5), quitRect.Size + new Point(10, 10));
            spriteBatch.Draw(Game1.white, quit, Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            hoverNew = false;
            hoverQuit = false;

            if (newRect.Contains(mouseState.X, mouseState.Y))
            {
                hoverNew = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    GameScreen screen = new GameScreen();
                    screen.Init(content, gameInstance);

                    gameInstance.ChangeScreen(screen);
                }
            }

            if (quitRect.Contains(mouseState.X, mouseState.Y))
            {
                hoverQuit = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    gameInstance.Exit();
                }
            }
        }
    }
}
