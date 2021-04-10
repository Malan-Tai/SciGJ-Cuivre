using System;
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

        public static void ShowGaugesValues()
        {
            foreach(string name in gaugesItems.Keys)
            {
                if(name != "Peuple")
                {
                    System.Diagnostics.Debug.WriteLine("Les " + name.ToLower() + " ont maintenant une satisfaction de " + gaugesItems[name]);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Le " + name.ToLower() + " a maintenant une satisfaction de " + gaugesItems[name]);
                }
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

        public static string GetLowestGauge()
        {
            string res = "";
            int min = 101;
            foreach (string key in gaugesItems.Keys)
            {
                int val = gaugesItems[key];
                if (val < min)
                {
                    min = val;
                    res = key;
                }
            }

            return res;
        }
    }
}
