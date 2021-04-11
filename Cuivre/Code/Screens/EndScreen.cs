using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Cuivre.Code.Screens
{
    class EndScreen : Screen
    {
        private bool victory;
        private string highGauge;

        private bool hoverMenu = false;
        private bool hoverAgain = false;
        private Rectangle menuRect;
        private Rectangle againRect;

        public EndScreen(bool victory, string highGauge = "")
        {
            this.victory = victory;
            this.highGauge = highGauge;
        }
        public override void Init(Game1 game)
        {
            base.Init(game);

            MediaPlayer.Play(Game1.Musics["M_Final"]);
            MediaPlayer.IsRepeating = false;

            float ratio = Game1.Textures["bouton_jouer"].Height / (float)Game1.Textures["bouton_rejouer"].Width;
            int h = (int)(ratio * Game1.WIDTH / 4);
            againRect = new Rectangle(Game1.WIDTH / 2 - 8 * Game1.HEIGHT / 36, Game1.HEIGHT / 2 + GameScreen.leftOffset, Game1.WIDTH / 4, h);

            ratio = Game1.Textures["bouton_quitter"].Height / (float)Game1.Textures["bouton_menu"].Width;
            menuRect = new Rectangle(Game1.WIDTH / 2 - 7 * Game1.HEIGHT / 36, Game1.HEIGHT / 2 + h + GameScreen.leftOffset, Game1.WIDTH / 5, (int)(ratio * Game1.WIDTH / 5));

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string texture = "menu_";
            if (victory) texture += "victoire";
            else texture += "DEFAITE";
            spriteBatch.Draw(Game1.Textures[texture], new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);

            Rectangle menu = new Rectangle(menuRect.Location, menuRect.Size);
            if (hoverMenu) menu = new Rectangle(menuRect.Location + new Point(0, -10), menuRect.Size + new Point(20, 20));
            spriteBatch.Draw(Game1.Textures["bouton_menu"], menu, Color.White);

            Rectangle again = new Rectangle(againRect.Location, againRect.Size);
            if (hoverAgain) again = new Rectangle(againRect.Location + new Point(0, -10), againRect.Size + new Point(20, 20));
            spriteBatch.Draw(Game1.Textures["bouton_rejouer"], again, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            hoverAgain = false;
            hoverMenu = false;

            if (menuRect.Contains(mouseState.X, mouseState.Y))
            {
                hoverMenu = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    MenuScreen screen = new MenuScreen();
                    screen.Init(gameInstance);
                    gameInstance.ChangeScreen(screen);
                }
            }

            if (againRect.Contains(mouseState.X, mouseState.Y))
            {
                hoverAgain = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    GameScreen screen = new GameScreen();
                    screen.Init(gameInstance);
                    Gauges.gameEnd = false;
                    Miracle.ResetMiracleChance();
                    gameInstance.ChangeScreen(screen);
                }
            }
        }
    }
}
