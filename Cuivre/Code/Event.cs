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
        //The lists are ordered this way : [Senateurs, Philosophes, Peuple, Militaires, Amants]
        public List<int> RawStats { get; set; } // -x to some gauges, +y to others, etc
        public List<string> SwapStats { get; set; } // a gauge with a real string receives the value of the gauge represented by said string
        public List<int> DecayStats { get; set; } // changes how much a gauge decays each day
        public List<bool> StuckStats { get; set; } // determines if a stat is stuck, ie. cannot increase or decrease

        private bool called = false;
        private int y = 400;
        private int speed = 1;
        private int minY = 300;
        private int maxY = 400;
        
        public void Draw(SpriteBatch spriteBatch, int x)
        {
            if (!called) spriteBatch.Draw(Game1.white, new Rectangle(x - 5, 55, 10, 10), Color.Green);
            else spriteBatch.Draw(Game1.white, new Rectangle(x - 20, 40, 40, 40), Color.Green); 
            
            if (y < maxY) spriteBatch.Draw(Game1.white, new Rectangle(x, 50, 100, 300), Color.White);
            if (y <= minY)
            {
                DrawDialogue(spriteBatch);
            }
        }

        public void DrawDialogue(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.white, new Rectangle(50, 300, 500, 50), Color.Wheat);
            spriteBatch.DrawString(Game1.font, Description, new Vector2(60, 310), Color.Black);
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            if (called && y <= minY && mouseState.LeftButton == ButtonState.Pressed)
            {
                called = false;
            }

            if (called)
            {
                y = Math.Max(minY, y - (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }
            else if (y != maxY)
            {
                y = Math.Min(maxY, y + (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }
        }

        public void TakePlace()
        {
            Gauges.ReinitDictionaries();

            if (Miracle.MiracleRoll())
            {
                //Do something cool
            }
            else
            {
                //Do something less cool
            }

            Gauges.HandleEvent(this);
            Miracle.ResetMiracleChance();
        }
    }
}
