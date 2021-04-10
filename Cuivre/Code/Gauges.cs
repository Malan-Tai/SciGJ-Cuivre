﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cuivre.Code
{
    static class Gauges
    {
        static public Dictionary<string, int> gaugesItems = new Dictionary<string, int>();

        const int startValues = 60;
        const int maxValue = 100;

        public static void InitializeGauges(List<string> names)
        {
            foreach (string name in names)
            {
                gaugesItems.Add(name, startValues);
            }
        }

        public static void IncrementGaugeValue(string index, int amount)
        {
            if (gaugesItems.TryGetValue(index, out int tempAmount))
            {
                gaugesItems[index] = tempAmount + amount;
                if(gaugesItems[index] > maxValue) { gaugesItems[index] = maxValue; }
                if (gaugesItems[index] < 0) { gaugesItems[index] = 0; }
            }
        }

    }
}
