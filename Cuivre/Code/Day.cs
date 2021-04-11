using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

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
            spriteBatch.Draw(Game1.white, new Rectangle(x, 50, w, 20), color);
            if (dayEvent != null) dayEvent.Draw(spriteBatch, x + w / 2);
        }

        public void DrawCurrent(SpriteBatch spriteBatch, int x, int dayWidth, int apWidth)
        {
            Color color = Color.Gray;

            for (int i = dailyActionpoints; i > 0; i--)
            {
                if (i == ActionPoints) color = Color.White;
                spriteBatch.Draw(Game1.white, new Rectangle(x, 10, apWidth, 100), color);
                x += apWidth;
            }

            if (dayEvent != null) dayEvent.Draw(spriteBatch, x - dayWidth / 2);
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
