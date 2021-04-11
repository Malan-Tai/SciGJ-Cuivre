using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cuivre.Code.Screens;

namespace Cuivre.Code
{
    class Event
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Hint { get; set; }

        public string Explanation { get; set; }

        //Event effects : Decay Stats and StuckStats remain until next event
        public Dictionary<string, int> RawStats { get; set; } // -x to some gauges, +y to others, etc
        public Dictionary<string, string> SwapStats { get; set; } // a gauge with a real string receives the value of the gauge represented by said string
        public Dictionary<string, int> DecayStats { get; set; } // changes how much a gauge decays each day
        public Dictionary<string, bool> StuckStats { get; set; } // determines if a stat is stuck, ie. cannot increase or decrease

        private bool called = false;
        private bool explain = false;
        private double curDelay = 0;
        private const double explainDelay = 200;
        private int y = 400;
        private int speed = 1;
        private int minY = 300;
        private int maxY = 400;
        
        public void Draw(SpriteBatch spriteBatch, int x, int h)
        {
            if (!called && !explain)
            {
                spriteBatch.Draw(Game1.Textures["icone_evenement"], new Rectangle(x - h, GameScreen.leftOffset - h / 2, 2 * h, 2 * h), Color.Green);
            }
            else
            {
                spriteBatch.Draw(Game1.Textures["icone_evenement"], new Rectangle(x - h, GameScreen.betweenOffset - h / 2, 2 * h, 2 * h), Color.Green);
            }
            
            if (called || explain)
            {
                DrawDialogue(spriteBatch);
            }
        }

        public void DrawDialogue(SpriteBatch spriteBatch)
        {
            Texture2D bubble = Game1.Textures["stele_evenements"];
            float ratio = bubble.Height / (float)bubble.Width;

            int textW = 4 * GameScreen.cardWidth + 3 * GameScreen.betweenOffset;
            int textH = (int)(ratio * textW);
            int y = Game1.HEIGHT - textH - GameScreen.leftOffset;

            spriteBatch.Draw(bubble, new Rectangle(GameScreen.leftOffset, y, textW, textH), Color.White);

            string text = "";
            if (called)
            {
                text = Description;
            }
            else if (explain)
            {
                text = Explanation;
            }

            y += textH / 3;
            foreach (string line in Utils.TextWrap.Wrap(text, 9 * textW / 10, Game1.font))
            {
                spriteBatch.DrawString(Game1.font, line, new Vector2(GameScreen.leftOffset + textW / 20, y), Color.Black);
                y += (int)Game1.font.MeasureString("l").Y + 5;
            }
        }

        public bool Update(GameTime gameTime, MouseState mouseState, MouseState prevMouseState)
        {
            if (explain) curDelay -= gameTime.ElapsedGameTime.TotalMilliseconds;
            bool nextDay = false;
            if (called && y <= minY && mouseState.LeftButton == ButtonState.Pressed)
            {
                called = false;
                explain = true;
                curDelay = explainDelay;
            }
            if (explain && curDelay <= 0 && y <= minY && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                explain = false;
                nextDay = true;
            }

            if (called || explain)
            {
                y = Math.Max(minY, y - (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }
            else if (y != maxY)
            {
                y = Math.Min(maxY, y + (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }

            return nextDay;
        }

        public void TakePlace(Screen screen)
        {
            called = true;
            Gauges.ReinitDictionaries();

            Gauges.HandleEvent(this, screen);
        }
    }
}
