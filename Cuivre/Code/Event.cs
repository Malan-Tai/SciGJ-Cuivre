using System;
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

        public void Draw(SpriteBatch spriteBatch, int x)
        {
            spriteBatch.Draw(Game1.white, new Rectangle(x - 5, 55, 10, 10), Color.Green);
        }

        public void TakePlace()
        {
            Gauges.ReinitDictionaries();

            if (Miracle.MiracleRoll())
            {
                //Miracle effect
                //Play sound
                foreach(string key in Gauges.gaugesItems.Keys)
                {
                    Gauges.IncrementGaugeValue(key, Miracle.gainedSatisfaction);
                }
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
