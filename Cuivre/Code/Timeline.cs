using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Cuivre.Code
{
    class Timeline
    {
        private List<Day> days;

        private int currentDay = 0;
        private int totalDays = 0;

        private int timelineWidth = 780;
        
        private const int miracleDelay = 5;
        private int miracleCurrentDelay = 0;

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
        public int SpendActionPoints(int amount)
        {
            Day day = days[currentDay];

            if (amount > day.ActionPoints) return -1;

            if(amount < 0 && miracleCurrentDelay > 0)
            {
                System.Diagnostics.Debug.WriteLine("Impossible de faire une offrande pour l'instant !");
            }
            else if(amount < 0 && miracleCurrentDelay == 0)
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

            
            if (day.ActionPoints <= 0)
            {
                currentDay++;
            }

            return day.ActionPoints;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x = 10;
            int w = timelineWidth / (totalDays + Day.dailyActionpoints);
            Color color = Color.Gray;

            for (int i = 0; i < totalDays; i++)
            {
                if (i == currentDay)
                {
                    days[i].DrawCurrent(spriteBatch, x, Day.dailyActionpoints * w, w);
                    x += Day.dailyActionpoints * w;
                    color = Color.White;
                }
                else
                {
                    days[i].Draw(spriteBatch, x, w, color);
                    x += w;
                }
            }

            if (called)
            {
                spriteBatch.Draw(Game1.white, new Rectangle(50, 300, 500, 50), Color.Wheat);

                string hint = "";
                for (int i = currentDay; i < totalDays; i++)
                {
                    string temp = days[i].GetHint();
                    if (temp != null) hint = temp;
                }

                int y = 310;
                foreach (string line in Utils.TextWrap.Wrap(hint, 480, Game1.font))
                {
                    spriteBatch.DrawString(Game1.font, line, new Vector2(60, y), Color.Black);
                    y += (int)Game1.font.MeasureString("l").Y + 5;
                }
            }
        }

        public bool Update(GameTime gameTime, MouseState mouseState, MouseState prevMouseState)
        {
            bool nextDay = false;
            if (called && curDelay <= 0 && mouseState.LeftButton == ButtonState.Pressed)
            {
                called = false;
            }
            curDelay -= gameTime.ElapsedGameTime.TotalMilliseconds;

            foreach (Day d in days) nextDay = nextDay || d.Update(gameTime, mouseState, prevMouseState);

            if (nextDay)
            {
                currentDay++;
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

        public void CallEvent()
        {
            days[currentDay].CallEvent();
        }
    }
}
