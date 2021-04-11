using System;
using System.Collections.Generic;
using System.Text;
using Cuivre.Code.Screens;

namespace Cuivre.Code
{
    static class Gauges
    {
        static public Dictionary<string, int> gaugesItems = new Dictionary<string, int>();

        const int startValues = 60;
        const int maxValue = 100;

        static public bool gameEnd = false;

        const int decay = 5;

        private static Dictionary<string, bool> stuckStats = new Dictionary<string, bool>();
        private static Dictionary<string, int> decayStats = new Dictionary<string, int>();

        private static bool initialized = false;

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
            if (!initialized)
            {
                initialized = true;
                foreach (string name in names)
                {
                    gaugesItems.Add(name, startValues);
                    stuckStats.Add(name, false);
                    decayStats.Add(name, decay);
                }
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

        public static void IncrementGaugeValue(string index, int amount, Screen screen)
        {
            if (gaugesItems.TryGetValue(index, out int tempAmount) && !stuckStats[index])
            {
                gaugesItems[index] = tempAmount + amount;
                if(gaugesItems[index] > maxValue) { gaugesItems[index] = maxValue; }
                if (gaugesItems[index] < 0) { gaugesItems[index] = 0; }
            }

            foreach (string key in gaugesItems.Keys)
            {
                if (gaugesItems[key] <= 0)
                {
                    ((GameScreen)screen).ChangeScreen(false, key);
                    gameEnd = true;
                }
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

        public static void HandleEvent(Event ev, Screen screen)
        {
            List<string> keys = new List<string>() { "Senateurs", "Philosophes", "Peuple", "Militaires", "Amants" };

            foreach (string key in keys)
            {
                //gaugesItems[key] += ev.RawStats[key];
                IncrementGaugeValue(key, ev.RawStats[key], screen);
                stuckStats[key] = ev.StuckStats[key];
                decayStats[key] = ev.DecayStats[key];
            }

            Dictionary<string, int> swapValues = new Dictionary<string, int>(); //stocks values the gauges will have after the swap
            foreach (string key in keys)
            {
                if (!ev.SwapStats[key].Equals("null"))
                {
                    swapValues.Add(key, gaugesItems[ev.SwapStats[key]]);
                }
                else
                {
                    swapValues.Add(key, gaugesItems[key]);
                }
            }
            foreach (string k in keys)
            {
                gaugesItems[k] = swapValues[k];
            }
        }
    }
}
