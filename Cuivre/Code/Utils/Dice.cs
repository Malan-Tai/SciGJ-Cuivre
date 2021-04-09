using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuivre.Code.Utils
{
    static class Dice
    {
        static Random rand = new Random();
        public static int GetRandint(int min, int max)
        // max is inclusive
        {
            return rand.Next(min, max + 1);
        }
    }
}