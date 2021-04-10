using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cuivre.Code
{
    class Timeline
    {
        public List<Day> Days { get; set; }

        private int currentDay;
        private int daysNumber;

        public Timeline()
        {
            currentDay = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < currentDay; i++)
            {
                Days[i].Draw(spriteBatch, i);
            }

            Days[currentDay].DrawCurrent(spriteBatch, currentDay);

            for (int i = currentDay + 1; i < daysNumber; i++)
            {
                Days[i].Draw(spriteBatch, i);
            }
        }
    }
}
