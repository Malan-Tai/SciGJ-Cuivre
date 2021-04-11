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
        private Rectangle newRect;
        private Rectangle quitRect; 

        public override void Init(Game1 game)
        {
            base.Init(game);
            float ratio = Game1.Textures["bouton_jouer"].Height / (float)Game1.Textures["bouton_jouer"].Width;
            int h = (int)(ratio * Game1.WIDTH / 4);
            newRect = new Rectangle(Game1.WIDTH / 4, 7 * Game1.HEIGHT / 18, Game1.WIDTH / 4, h);

            ratio = Game1.Textures["bouton_quitter"].Height / (float)Game1.Textures["bouton_quitter"].Width;
            quitRect = new Rectangle(Game1.WIDTH / 4 + Game1.WIDTH / 40, 7 * Game1.HEIGHT / 18 + h, Game1.WIDTH / 5, (int)(ratio * Game1.WIDTH / 5));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.Textures["menu_debut"], new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);

            Rectangle newR = new Rectangle(newRect.Location, newRect.Size);
            if (hoverNew) newR = new Rectangle(newRect.Location + new Point(-10, -10), newRect.Size + new Point(20, 20));
            spriteBatch.Draw(Game1.Textures["bouton_jouer"], newR, Color.White);

            Rectangle quit = new Rectangle(quitRect.Location, quitRect.Size);
            if (hoverQuit) quit = new Rectangle(quitRect.Location + new Point(-10, -10), quitRect.Size + new Point(20, 20));
            spriteBatch.Draw(Game1.Textures["bouton_quitter"], quit, Color.White);
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
                    screen.Init(gameInstance);

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
