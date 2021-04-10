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

        const int decay = 5;

        private static Dictionary<string, bool> stuckStats = new Dictionary<string, bool>();
        private static Dictionary<string, int> decayStats = new Dictionary<string, int>();

        public static void ReinitDictionaries()
        {
            List<string> keys = new List<string>(gaugesItems.Keys);
            foreach (string key in keys)
            {
                stuckStats[key] = false;
                decayStats[key] = decay;
            }
        }

        public static void InitializeGauges(List<string> names)
        {
            foreach (string name in names)
            {
                gaugesItems.Add(name, startValues);
                stuckStats.Add(name, false);
                decayStats.Add(name, decay);
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
            if (gaugesItems.TryGetValue(index, out int tempAmount) && !stuckStats[index])
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

        public static void NaturalDecay()
        {
            List<string> keys = new List<string>(gaugesItems.Keys);
            foreach (string key in keys)
            {
                if (!stuckStats[key]) gaugesItems[key] -= decayStats[key];
            }
        }

        public static void HandleEvent(Event ev)
        {
            List<string> keys = new List<string>() { "Senateurs", "Philosophes", "Peuple", "Militaires", "Amants" };

            for (int i = 0; i < 5; i++)
            {
                gaugesItems[keys[i]] += ev.RawStats[i];
                stuckStats[keys[i]] = ev.StuckStats[i];
                decayStats[keys[i]] = ev.DecayStats[i];
            }

            Dictionary<string, int> swapValues = new Dictionary<string, int>(); //stocks values the gauges will have after the swap
            for (int i = 0; i < 5; i++)
            {
                if (!ev.SwapStats[i].Equals(""))
                {
                    swapValues.Add(keys[i], gaugesItems[ev.SwapStats[i]]);
                }
                else
                {
                    swapValues.Add(keys[i], gaugesItems[keys[i]]);
                }
            }
            foreach (string k in keys)
            {
                gaugesItems[k] = swapValues[k];
            }
        }
    }
}
