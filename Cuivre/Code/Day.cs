using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Cuivre.Code.Screens;

namespace Cuivre.Code
{
    class Day
    {
        public const int dailyActionpoints = 3;

        public int ActionPoints { get; set; } = dailyActionpoints;

        private Event dayEvent;

        public Day()
        {
            dayEvent = null;
        }
        public Day(Event e)
        {
            dayEvent = e;
            ActionPoints = 0;
        }

        public bool HasEvent()
        {
            return dayEvent != null;
        }

        public string GetHint()
        {
            if (dayEvent != null) return dayEvent.Hint;
            return null;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int w, Color color)
        {
            spriteBatch.Draw(Game1.white, new Rectangle(x, GameScreen.leftOffset, w, w / 3), color);
            spriteBatch.Draw(Game1.Textures["baton_frise"], new Rectangle(x + 2 * w / 3, GameScreen.leftOffset - w / 6, 2 * w / 3, 2 * w / 3), Color.White);
            if (dayEvent != null) dayEvent.Draw(spriteBatch, x + w / 2, w / 3);
        }

        public void DrawCurrent(SpriteBatch spriteBatch, int x, int dayWidth, int apWidth, int pulseActions, float gamma)
        {
            Color color = new Color(120, 176, 222);
            int h = 2 * (GameScreen.leftOffset - GameScreen.betweenOffset) + apWidth / 3;

            spriteBatch.Draw(Game1.Textures["baton_frise"], new Rectangle(x - apWidth / 3, GameScreen.betweenOffset - h / 2, 2 * apWidth / 3, 2 * h), Color.White);

            for (int i = dailyActionpoints; i > 0; i--)
            {
                if (i == ActionPoints) color = new Color(229, 199, 80);
                spriteBatch.Draw(Game1.white, new Rectangle(x, GameScreen.betweenOffset, apWidth, h), color);
                if (i <= ActionPoints && i > ActionPoints - pulseActions)
                {
                    spriteBatch.Draw(Game1.white, new Rectangle(x, GameScreen.betweenOffset, apWidth, h), new Color(120, 176, 222) * gamma);
                }

                spriteBatch.Draw(Game1.Textures["baton_frise"], new Rectangle(x + 2 * apWidth / 3, GameScreen.betweenOffset - h / 2, 2 * apWidth / 3, 2 * h), Color.White);

                x += apWidth;
            }

            if (dayEvent != null) dayEvent.Draw(spriteBatch, x - dayWidth / 2, h);
        }

        public bool Update(GameTime gameTime, MouseState mouseState, MouseState prevMouseState)
        {
            if (dayEvent != null) return dayEvent.Update(gameTime, mouseState, prevMouseState);
            return false;
        }

        public void CallEvent(Screen screen)
        {
            if (dayEvent != null) dayEvent.TakePlace(screen);
        }
    }
}
