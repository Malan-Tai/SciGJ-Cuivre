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

        private bool Hidden { get; set; }

        public string Hint { get; set; }

        public void Draw(SpriteBatch spriteBatch, int x)
        {

        }

        public void TakePlace()
        {
            if (Miracle.MiracleRoll())
            {
                //Do something cool
            }
            else
            {
                //Do something less cool
            }

            Miracle.ResetMiracleChance();
        }
    }
}
