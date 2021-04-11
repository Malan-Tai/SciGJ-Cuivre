using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Cuivre.Code.Screens;

namespace Cuivre.Code
{
    class Timeline
    {
        private List<Day> days;

        private int currentDay = 0;
        private int totalDays = 0;

        private int timelineWidth = Game1.WIDTH - 2 * GameScreen.leftOffset;
        
        private const int miracleDelay = 5;
        public int miracleCurrentDelay = 0;

        private bool called = false;
        private const double oracleDelay = 500;
        private double curDelay = 0;

        public Timeline()
        {
            days = new List<Day>();
            List<Event> eventList = EventPool.PickEventsFromLists();

            for (int i = 0; i < EventPool.eventAmountInTimeLine; i++)
            {
                for (int j = 0; j < EventPool.dayPerEvent - 1; j++)
                {
                    days.Add(new Day());
                }
                days.Add(new Day(eventList[i]));
            }

            totalDays = EventPool.eventAmountInTimeLine * EventPool.dayPerEvent;
        }

        public void DecayMiracleDelay()
        {
            if(miracleCurrentDelay > 0)
            {
                miracleCurrentDelay -= 1;
            }
        }


        //renvoie -1 si on ne peut pas depenser ce nombre de points, le nombre de points restant sinon (0 si on change de jour)
        public int SpendActionPoints(int amount, bool freeze, GameScreen screen)
        {
            Day day = days[currentDay];

            if (amount > day.ActionPoints) return -1;

            if (amount < 0)
            {
                Miracle.AddMiracleChance(day.ActionPoints * Miracle.gainedMiracleChanceWithLowSatisfaction);
                System.Diagnostics.Debug.WriteLine("Chance de miracle augmentée : " + Miracle.GetCurrentMiracleChance() + "%");
                day.ActionPoints = 0;
                miracleCurrentDelay = miracleDelay;
            }
            else
            {
                day.ActionPoints -= amount;
            }

            
            if (day.ActionPoints <= 0 && !freeze)
            {
                currentDay++;
                if (currentDay == totalDays)
                {
                    screen.ChangeScreen(true);
                }
            }

            return day.ActionPoints;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x = GameScreen.leftOffset;
            int w = timelineWidth / (totalDays + Day.dailyActionpoints);
            Color color = new Color(120, 176, 222);

            for (int i = 0; i < totalDays; i++)
            {
                if (i == currentDay)
                {
                    days[i].DrawCurrent(spriteBatch, x, Day.dailyActionpoints * w, w);
                    x += Day.dailyActionpoints * w;
                    color = new Color(229, 199, 80);
                }
                else
                {
                    days[i].Draw(spriteBatch, x, w, color);
                    x += w;
                }
            }

            spriteBatch.Draw(Game1.Textures["frise_ornements_cotes"], new Rectangle(GameScreen.leftOffset - w / 3, GameScreen.leftOffset - w / 5, 2 * w / 3, 2 * w / 3), Color.White);
            spriteBatch.Draw(Game1.Textures["frise_ornements_cotes"], new Rectangle(x - w / 4, GameScreen.leftOffset - w / 5, 2 * w / 3, 2 * w / 3), null, Color.White, 0,
                             new Vector2(), SpriteEffects.FlipHorizontally, 0);

            if (called)
            {
                Texture2D bubble = Game1.Textures["bulle_poete"];
                float ratio = bubble.Height / (float)bubble.Width;

                int textW = Game1.WIDTH - 2 * GameScreen.leftOffset - GameScreen.cardWidth;
                int textH = (int)(ratio * textW);
                int y = Game1.HEIGHT - textH - GameScreen.betweenOffset;

                string hint = "";
                for (int i = currentDay; i < totalDays; i++)
                {
                    string temp = days[i].GetHint();
                    if (temp != null)
                    {
                        hint = temp;
                        break;
                    }
                }

                spriteBatch.Draw(bubble, new Rectangle(GameScreen.leftOffset, y, textW, textH), Color.White);

                List<string> lines = Utils.TextWrap.Wrap(hint, textW - 2 * GameScreen.betweenOffset, Game1.font);

                y += textH / 5;
                foreach (string line in lines)
                {
                    spriteBatch.DrawString(Game1.font, line, new Vector2(textW / 10 + GameScreen.leftOffset, y), Color.Black);
                    y += (int)Game1.font.MeasureString("l").Y + 5;
                }
            }
        }

        public bool Update(GameTime gameTime, MouseState mouseState, MouseState prevMouseState, GameScreen screen)
        {
            bool nextDay = false;
            if (called && curDelay <= 0 && mouseState.LeftButton == ButtonState.Pressed)
            {
                called = false;
                nextDay = days[currentDay].ActionPoints <= 0;
            }
            if (called) curDelay -= gameTime.ElapsedGameTime.TotalMilliseconds;

            foreach (Day d in days) nextDay = nextDay || d.Update(gameTime, mouseState, prevMouseState);

            if (nextDay)
            {
                currentDay++;
                if (currentDay == totalDays)
                {
                    screen.ChangeScreen(true);
                }
            }

            return nextDay;
        }

        public void CallOracle()
        {
            called = true;
            curDelay = oracleDelay;
        }

        public bool TodayHasEvent()
        {
            return days[currentDay].HasEvent();
        }

        public void CallEvent(Screen screen)
        {
            days[currentDay].CallEvent(screen);
        }

        public int GetLeftActionPoints()
        {
            return days[currentDay].ActionPoints;
        }
    }
}
