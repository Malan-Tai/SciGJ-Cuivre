﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code
{
    class Event
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Hint { get; set; }

        //Event effects : Decay Stats and StuckStats remain until next event
        public Dictionary<string, int> RawStats { get; set; } // -x to some gauges, +y to others, etc
        public Dictionary<string, string> SwapStats { get; set; } // a gauge with a real string receives the value of the gauge represented by said string
        public Dictionary<string, int> DecayStats { get; set; } // changes how much a gauge decays each day
        public Dictionary<string, bool> StuckStats { get; set; } // determines if a stat is stuck, ie. cannot increase or decrease

        private bool called = false;
        private int y = 400;
        private int speed = 1;
        private int minY = 300;
        private int maxY = 400;
        
        public void Draw(SpriteBatch spriteBatch, int x)
        {
            if (!called)
            {
                spriteBatch.Draw(Game1.white, new Rectangle(x - 5, 55, 10, 10), Color.Green);
            }
            else
            {
                spriteBatch.Draw(Game1.white, new Rectangle(x - 20, 40, 40, 40), Color.Green);
            }
            
            if (y < maxY) spriteBatch.Draw(Game1.white, new Rectangle(x, 50, 100, 300), Color.White);
            if (y <= minY)
            {
                DrawDialogue(spriteBatch);
            }
        }

        public void DrawDialogue(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.white, new Rectangle(50, 300, 500, 50), Color.Wheat);

            int y = 310;
            foreach (string line in Utils.TextWrap.Wrap(Description, 480, Game1.font))
            {
                spriteBatch.DrawString(Game1.font, line, new Vector2(60, y), Color.Black);
                y += (int)Game1.font.MeasureString("l").Y + 5;
            }
        }

        public bool Update(GameTime gameTime, MouseState mouseState)
        {
            bool nextDay = false;
            if (called && y <= minY && mouseState.LeftButton == ButtonState.Pressed)
            {
                called = false;
                nextDay = true;
            }

            if (called)
            {
                y = Math.Max(minY, y - (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }
            else if (y != maxY)
            {
                y = Math.Min(maxY, y + (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }

            return nextDay;
        }

        public void TakePlace()
        {
            called = true;
            Gauges.ReinitDictionaries();

            Gauges.HandleEvent(this);
            Miracle.ResetMiracleChance();
        }
    }
}
