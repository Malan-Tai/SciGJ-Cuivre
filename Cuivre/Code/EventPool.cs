using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Cuivre.Code
{
    static class EventPool
    {
        public const int eventAmountInTimeLine = 7; //Sujet à changement

        public const int eventAmountInPool = 3; //Aussi peut-être

        public const int dayPerEvent = 3;

        //On sélectionne un event au hasard par pool et on l'ajoute à notre liste pour la timeline
        public static List<Event> PickEventsFromLists()
        {
            List<Event> temp = JsonConvert.DeserializeObject<List<Event>>(File.ReadAllText("Content\\Design\\events.json", Encoding.GetEncoding(28591)));

            List<List<Event>> eventPoolsList = new List<List<Event>>();
            for (int i = 0; i < eventAmountInTimeLine; i++)
            {
                eventPoolsList.Add(new List<Event>());
                for (int j = 0; j < eventAmountInPool; j++)
                {
                    eventPoolsList[i].Add(temp[j + i * eventAmountInPool]);
                }
            }

            List<Event> eventList = new List<Event>();
            for(int i = 0; i < eventPoolsList.Count; i++)
            {
                Event chosenEvent = eventPoolsList[i][Utils.Dice.GetRandint(0, eventPoolsList[i].Count - 1)];
                eventList.Add(chosenEvent);
            }
            return eventList;
        }
    }
}
