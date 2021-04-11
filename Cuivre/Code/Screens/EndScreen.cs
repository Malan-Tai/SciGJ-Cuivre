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
        private string lostGauge;

        private bool hoverMenu = false;
        private bool hoverAgain = false;
        private Rectangle menuRect = new Rectangle(350, 200, 100, 50);
        private Rectangle againRect = new Rectangle(350, 300, 100, 50);

        public EndScreen(bool victory, string lostGauge = "")
        {
            this.victory = victory;
            this.lostGauge = lostGauge;
        }
        public override void Init(Game1 game)
        {
            base.Init(game);

            MediaPlayer.Play(Game1.Musics["M_Final"]);
            MediaPlayer.IsRepeating = false;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string text = "Victoire !";
            if (!victory) text = "Defaite...";

            spriteBatch.DrawString(Game1.font, text, new Vector2(350, 100), Color.Black, 0f, new Vector2(), 2f, new SpriteEffects(), 0f);
            if (!victory) spriteBatch.DrawString(Game1.font, "Les " + lostGauge + " sont trop insatisfaits", new Vector2(300, 130), Color.Black);

            Rectangle menu = new Rectangle(menuRect.Location, menuRect.Size);
            if (hoverMenu) menu = new Rectangle(menuRect.Location + new Point(-5, -5), menuRect.Size + new Point(10, 10));
            spriteBatch.Draw(Game1.white, menu, Color.Green);

            Rectangle again = new Rectangle(againRect.Location, againRect.Size);
            if (hoverAgain) again = new Rectangle(againRect.Location + new Point(-5, -5), againRect.Size + new Point(10, 10));
            spriteBatch.Draw(Game1.white, again, Color.Blue);
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
