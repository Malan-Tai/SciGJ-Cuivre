using System;
using System.Collections.Generic;
using System.Text;

namespace Cuivre.Code
{
    static class EventPool
    {
        const int eventAmountInTimeLine = 7; //Sujet à changement

        const int eventAmountInPool = 4; //Aussi peut-être

        static List<Event> eventList; //Liste finale à retourner à la timeline

        //Liste des pools d'events dans lesquels on pourra piocher
        static List<List<Event>> eventPoolsList;

        //On crée la liste des pools d'event
        public static void AddEvents()
        {
            for(int i = 0; i < eventAmountInTimeLine; i++)
            {
                for(int j = 0; j < eventAmountInPool; j++)
                {
                    eventPoolsList[i].Add(new Event()); //J'ai un doute ici
                }
            }
        }

        //On sélectionne un event au hasard par pool et on l'ajoute à notre liste pour la timeline
        public static List<Event> PickEventsFromLists()
        {
            for(int i = 0; i < eventPoolsList.Count; i++)
            {
                Event chosenEvent = eventPoolsList[i][Utils.Dice.GetRandint(0, eventPoolsList[i].Count)];
                eventList.Add(chosenEvent);
            }
            return eventList;
        }
    }
}
