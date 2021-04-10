using System;
using System.Collections.Generic;
using System.Text;

namespace Cuivre.Code
{
    static class Miracle
    {
        const int baseMiracleChance = 0;

        public const int gainedMiracleChanceWithLowSatisfaction = 10;

        static int currentMiracleChance = baseMiracleChance;

        const int maxMiracleChance = 100;

        public const int gainedSatisfaction = 20;

        public static int GetCurrentMiracleChance() { return currentMiracleChance; }

        public static void AddMiracleChance(int additionnalChance)
        {
            currentMiracleChance += additionnalChance;
            if(currentMiracleChance > maxMiracleChance) { currentMiracleChance = maxMiracleChance; }
        }

        public static void ResetMiracleChance() { currentMiracleChance = baseMiracleChance; }

        public static bool MiracleRoll()
        {
            foreach(int gaugeValue in Gauges.gaugesItems.Values)
            {
                if(gaugeValue <= 30)
                {
                    AddMiracleChance(10);
                }
            }

            int roll = Utils.Dice.GetRandint(1, 100);

            return (roll <= currentMiracleChance);
        }

    }
}
